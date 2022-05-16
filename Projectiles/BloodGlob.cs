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
			Projectile.penetrate = 2;
		}

		public override void AI()
		{
			Projectile.rotation++;
			Projectile.velocity.Y += 0.5f;

			if (Projectile.ai[0] == 0)
			{
				Projectile.Kill();
			}
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
			Projectile.ai[0]--; // Projectile can only bounce a few times
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Splash, Projectile.Center);
		}
	}
}