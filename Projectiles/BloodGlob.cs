using MemeClasses.Items.Pulleys;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Projectiles
{
	public class BloodGlob : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = GetInstance<PulleyDamageClass>();
			Projectile.penetrate = 5;
		}

		public override void AI()
		{
			Projectile.rotation++;
			Projectile.velocity.Y++;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.penetrate++; // Don't lower pierce when hitting an enemy
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != Projectile.velocity.X)
			{
				Projectile.position.X += Projectile.velocity.X;
				Projectile.velocity.X = -oldVelocity.X;
			}

			if (oldVelocity.Y != Projectile.velocity.Y)
			{
				Projectile.position.Y += Projectile.velocity.Y;
				Projectile.velocity.Y = -oldVelocity.Y * 0.7f;
			}

			SoundEngine.PlaySound(SoundID.Splash, Projectile.Center);
			Projectile.penetrate--; // Lower pierce when hitting a tile, that way it'll die after bouncing five times
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Splash, Projectile.Center);
		}
	}
}