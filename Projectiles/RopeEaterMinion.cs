using MemeClasses.Items.Pulleys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Projectiles
{
	public class RopeEaterMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = Projectile.SentryLifeTime * 10;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		float timer = 1;
		bool init = false;
		bool first = false;
		public override void AI()
		{
			Animate();

			Player owner = Main.player[Projectile.owner];
			PulleyPlayer pPlr = owner.GetModPlayer<PulleyPlayer>();

			// Kill projectile once the player is no longer using the pulley
			if (pPlr.ActivePulley == null || pPlr.ActivePulley.type != ItemType<RopeEater>() || !owner.pulley)
			{
				Projectile.Kill();
				return;
			}

			// Also kill the projectile if it's too far away
			if (Projectile.Distance(owner.Center) > 1024f)
			{
				Projectile.Kill();
				return;
			}

			if (!init)
			{
				if (Projectile.ai[0] == 1)
				{
					first = true;
				}
				init = true;
			}

			NPC npc = Projectile.FindTargetWithinRange(800, true);
			if (npc != null)
			{
				// Move towards target, but also stay within range
				Vector2 target = npc.Center;
				Vector2 origin = owner.Center;

				Vector2 dir = origin.DirectionTo(target);
				dir.Normalize();
				Vector2 targ = dir * 196f;

				if (first)
				{
					targ = targ.RotatedBy(MathHelper.ToRadians(-15));
				}
				else
				{
					targ = targ.RotatedBy(MathHelper.ToRadians(15));
				}

				Projectile.ai[0] = targ.X;
				Projectile.ai[1] = targ.Y;

				Vector2 targ2 = owner.Center + new Vector2(Projectile.ai[0], Projectile.ai[1]);

				Projectile.position = Projectile.position.MoveTowards(targ2, 6f);

				timer = 1; // Make the minions immediately move to a new position when the enemy despawns
			}
			else
			{
				float range = 196 * pPlr.PulleySpeed;

				timer--;
				if (timer == 0)
				{
					Vector2 newTarget = Main.rand.NextVector2CircularEdge(range, range);
					Projectile.ai[0] = newTarget.X;
					Projectile.ai[1] = newTarget.Y;
					timer = Main.rand.Next(120, 240);
				}

				Vector2 target = owner.Center + new Vector2(Projectile.ai[0], Projectile.ai[1]);

				float spd = 6f;
				if (Projectile.Distance(owner.Center) > range * 2)
				{
					spd *= 2;
					if (Projectile.Distance(owner.Center) > range * 4)
					{
						spd *= 3; // x6 multiplier on speed in total, since this stacks with the first
					}
				}

				Projectile.position = Projectile.position.MoveTowards(target, spd);
			}

			Projectile.rotation = Projectile.DirectionTo(owner.Center).ToRotation();
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

		public override bool PreDraw(ref Color lightColor)
		{
			Player owner = Main.player[Projectile.owner];

			if (Projectile.Distance(owner.Center) < 1024f)
			{
				Texture2D tex = Request<Texture2D>(Texture + "_Vine").Value;
				Vector2 targetPos = owner.Center;
				Vector2 center = Projectile.Center;
				Vector2 distToProj = targetPos - center;
				float projRotation = distToProj.ToRotation() - MathHelper.PiOver2;
				float distance = distToProj.Length();
				Color drawColor = Projectile.GetAlpha(lightColor);

				while (true) // Continue doing this forever until we break the loop
				{
					if (distance < 32f || float.IsNaN(distance)) // If we're close enough, we can break out of this loop
						break;

					distToProj.Normalize();
					distToProj *= 28f;
					center += distToProj;
					distToProj = targetPos - center;
					distance = distToProj.Length();

					Main.EntitySpriteDraw(tex, center - Main.screenPosition, null, drawColor, projRotation, tex.Size() * 0.5f, 1f, SpriteEffects.None, 0);
				}
			}

			return base.PreDraw(ref lightColor);
		}
	}
}