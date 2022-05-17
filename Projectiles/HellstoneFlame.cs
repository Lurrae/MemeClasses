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
	public class HellstoneFlame : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = GetInstance<PulleyDamageClass>();
			Projectile.timeLeft = 240;
		}

		private int ShotCooldown = 1;
		public override void AI()
		{
			Animate();
			Player player = Main.player[Projectile.owner];
			PulleyPlayer pPlr = player.GetModPlayer<PulleyPlayer>();

			NPC target = Projectile.FindTargetWithinRange(800f, true);
			if (target != null)
			{
				ShotCooldown--;

				if (ShotCooldown == 0)
				{
					ShotCooldown = (int)Math.Max(30, 90 - (2 * pPlr.PulleySpeed));
					Vector2 targetPos = target.Center;
					Vector2 velocity = targetPos - Projectile.Center;
					velocity.Normalize();

					SoundEngine.PlaySound(SoundID.Item42, Projectile.Center);
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity * 12f, ProjectileType<HellstoneFlameShot>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
		}

		private void Animate()
		{
			Projectile.frameCounter++;

			if (Projectile.frameCounter >= 6)
			{
				Projectile.frame++;
				if (Projectile.frame > Main.projFrames[Type] - 1)
					Projectile.frame = 0;

				Projectile.frameCounter = 0;
			}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			target.AddBuff(BuffID.OnFire, 240);
		}
	}

	public class HellstoneFlameShot : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.DamageType = GetInstance<PulleyDamageClass>();
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			target.AddBuff(BuffID.OnFire, 60);
		}
	}
}