using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DestinyMod.Content.Projectiles.Boss
{
    public class ForestGuardianDeathRay : ModProjectile
    {
        public float startRot;
        public Vector2 startPos;
        private Texture2D sprite;

        public float rotSpeed = 0.010f; // the rotation speed this is for every difficulty that isnt changed on the AI()
        public override string Texture => "DestinyMod/Assets/ForestGuardianDeathRay";
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;
            Projectile.scale = 0.2f;
            Projectile.timeLeft = 480;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Main.expertMode || Main.masterMode)
            {
                rotSpeed = 0.011f;
            }
            startRot += rotSpeed;
            if (Projectile.timeLeft < 440 && Projectile.timeLeft > 420)
            {
                Projectile.scale += 0.1f;
            }

            if (Projectile.timeLeft < 20 && Projectile.scale > 0)
                Projectile.scale -= 0.1f;

        }

        public override void OnSpawn(IEntitySource source)
        {
            startRot = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            startPos = Projectile.Center;
            Projectile.velocity = Vector2.Zero;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 endPoint = startPos + Vector2.One.RotatedBy(startRot + MathHelper.PiOver4 + MathHelper.Pi) * 2500;
            float point = float.NaN;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startPos, endPoint, 15, ref point))
            {
                return true;
            }
            else return false;
        }

        public override bool PreDraw(ref Color lightColor)  // used to draw the sprite (idk how this works) 
        {
            if (sprite == null)
            {
                sprite = ModContent.Request<Texture2D>(Texture).Value;
            }
            Vector2 pos = startPos - Main.screenPosition;
            int width = (int)(15 * Projectile.scale);
            int height = 2500;
            float rotation = startRot;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 origin = sprite.Size() * 0.5f;

            spriteBatch.End(); 
            spriteBatch.Begin(default, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(sprite, new Rectangle((int)pos.X, (int)pos.Y, width, height), null, Color.Orange, rotation, origin, SpriteEffects.None, 0); 

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}



// How to spawn the projectile??
// well you can use this 

Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<ForestGuardianDeathRay>(), damage, knockback);

// and a example of using this on my boss is this 

        private void DeathRaySpinAttack(int laserAmount, int damage, float knockback)
        {
            int amount = laserAmount;

            if (firstTime == true)
            {
                firstTime = false;
                for (int i = 0; i < amount; i++)
                {
                    Vector2 velocity = new Vector2(0, 12).RotatedBy(MathHelper.TwoPi * i / amount);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<ForestGuardianDeathRay>(), damage, knockback);
                }
            }
        }





