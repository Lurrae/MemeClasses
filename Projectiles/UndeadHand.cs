using MemeClasses.Items.Pulleys;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Projectiles
{
	public class UndeadHand : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 28;
			Projectile.timeLeft = Projectile.SentryLifeTime;
		}

		private bool IsRight // Was this projectile summoned by right-clicking?
		{
			get => Projectile.ai[0] == 1f;
			set => Projectile.ai[0] = value ? 1f : 0f;
		}

		private bool PlayerGrabbed;
		public override void AI()
		{
			if (IsRight)
			{
				Player player = Main.player[Projectile.owner];
				PulleyPlayer pPlr = player.GetModPlayer<PulleyPlayer>();

				Projectile other = null;
				for (int p = 0; p < Main.maxProjectiles; p++)
				{
					Projectile proj = Main.projectile[p];
					if (proj.active && proj.type == Projectile.type && proj.whoAmI != Projectile.whoAmI && proj.ai[0] == 0)
					{
						other = proj;
						break;
					}
				}

				if (other != null && Projectile.Distance(other.Center) < 2048f && Collision.CanHitLine(Projectile.position, 1, 1, other.position, 1, 1))
				{
					Vector2 vect = player.Size + new Vector2(0, 16);
					Vector2 vect2 = player.position - new Vector2(0, 48);

					if (player.controlJump || player.mount.Active || player.grapCount > 0 || !Collision.CheckAABBvLineCollision(vect2, vect, Projectile.Center, other.Center))
					{
						PlayerGrabbed = false;
						pPlr.MovingOnZipline = false;
						pPlr.OnZipline = false;
						return;
					}

					if (Collision.CheckAABBvLineCollision(vect2, vect, Projectile.Center, other.Center) && (PlayerGrabbed || player.controlUp || player.controlDown) && !player.mount.Active && player.grapCount <= 0)
					{
						int num38 = (int)(player.position.X + (player.width / 2)) / 16;
						int num40 = (int)(player.position.Y - 8f) / 16;
						bool alreadyOnRope = false;
						if ((!Main.tile[num38, num40 - 1].IsActuated && Main.tileRope[Main.tile[num38, num40 - 1].TileType]) || (!Main.tile[num38, num40 + 1].IsActuated && Main.tileRope[Main.tile[num38, num40 + 1].TileType]))
						{
							alreadyOnRope = true;
						}

						// Dismount the player if they jump, have grabbed onto a rope, are no longer on the zipline, are mounted/grappling, or press left/right on a vertical zipline or down on a horizontal one
						// Also check if they got hit by a stunning debuff, because those kick them off ropes too
						if (player.controlJump || alreadyOnRope || !Collision.CheckAABBvLineCollision(vect2, vect, Projectile.Center, other.Center) ||
							player.mount.Active || player.grapCount > 0 ||
							(Projectile.position.X == other.position.X && (player.controlLeft || player.controlRight)) ||
							(Projectile.position.Y == other.position.Y && player.controlDown) ||
							player.frozen || player.stoned || player.webbed)
						{
							PlayerGrabbed = false;
							pPlr.MovingOnZipline = false;
							pPlr.OnZipline = false;
							return;
						}

						// Snap player position when they first grab onto the zipline
						if (!PlayerGrabbed)
						{
							Vector2 oldPos = player.position;
							if (Projectile.position.X == other.position.X)
							{
								player.position.X = Projectile.position.X + (player.direction == 1 ? 3 : -10);
							}
							else if (Projectile.position.Y == other.position.Y)
							{
								player.position.Y = Projectile.position.Y + 24;
							}
							else
							{
								var closestPos = player.position.ClosestPointOnLine(Projectile.Center, other.Center);
								player.position = closestPos + new Vector2(player.direction == 1 ? 3 : -10, 16);
							}

							// Let's double-check that we didn't just force the player into a block
							if (Collision.SolidCollision(player.position, player.width, player.height))
							{
								player.position = oldPos; // Undo the snap effect
								return; // Don't run any below code, since the player technically never grabbed onto the rope
							}
						}
						else // Only AFTER the first frame do we need to do this, as otherwise it'll mess with the position snapping
						{
							player.position -= player.velocity; // Prevent velocity-based movement
						}

						// Store player's previous position to determine "velocity" later
						// Also set velocity to zero so that doesn't mess with us
						player.oldPosition = player.position;
						player.velocity = Vector2.Zero;

						// Set some visual stuff
						PlayerGrabbed = true;
						player.pulley = true;
						player.pulleyDir = 2;
						pPlr.OnZipline = true;
						player.wingFrame = 0;
						player.bodyFrame.Y = player.bodyFrame.Height;
						player.legFrameCounter = 0.0; // Lock the player's leg animation to a singular frame (this should stop them from "walking" on air)

						Vector2 distToProj = other.Center - Projectile.Center;
						distToProj.Normalize();

						bool closeX = Math.Abs(Projectile.Center.X - other.Center.X) < 64f;
						bool closeY = Math.Abs(Projectile.Center.Y - other.Center.Y) < 64f;
						bool farX = Math.Abs(Projectile.Center.X - other.Center.X) > 64f;
						bool farY = Math.Abs(Projectile.Center.Y - other.Center.Y) > 64f;

						// Handle player movement left/right
						if ((player.controlLeft || player.controlRight) && (farX || closeY))
						{
							#region Lots of Variables
							bool flag = player.controlRight && player.Right.Distance(Projectile.Left) > 40f;
							bool flag2 = player.controlLeft && player.Left.Distance(other.Right) > 40f;
							bool flag3 = player.controlRight && player.Right.Distance(other.Left) > 40f;
							bool flag4 = player.controlLeft && player.Left.Distance(Projectile.Right) > 40f;
							bool condition = Projectile.Center.X > other.Center.X && (flag || flag2);
							bool condition2 = flag3 || flag4;
							bool tooClose = (player.controlRight && player.Right.Distance(Projectile.Left) < 40f) || (player.controlRight && player.Right.Distance(other.Left) < 40f) || (player.controlLeft && player.Left.Distance(Projectile.Right) < 40f) || (player.controlLeft && player.Left.Distance(other.Right) < 40f);
							#endregion

							if (condition)
							{
								player.position += distToProj * 8f * pPlr.PulleySpeed * (player.controlRight ? -1f : 1f);
							}
							else if (condition2)
							{
								player.position -= distToProj * 8f * pPlr.PulleySpeed * (player.controlRight ? -1f : 1f);
							}

							// Reset player position if they would be going inside a block or going too close to either end of the zipline
							if (Collision.SolidCollision(player.position, player.width, player.height) || tooClose)
							{
								player.position = player.oldPosition;
							}
						}
						// Handle player movement up/down
						else if ((player.controlUp || player.controlDown) && (farY || closeX))
						{
							#region Lots More Variables
							float SlimyPulleyMod = 1f;
							float SlimyPulleyMod2 = 1f;
							if (pPlr.ActivePulley != null)
							{
								SlimyPulleyMod = player.controlDown && pPlr.ActivePulley.type == ItemType<SlimePulley>() ? 1.6f : 1f;
								SlimyPulleyMod2 = player.controlDown && pPlr.ActivePulley.type == ItemType<SlimePulley>() ? 2f : 1f;
							}
							
							bool flag = player.controlUp && player.Top.Distance(other.Bottom) > 16f;
							bool flag2 = player.controlDown && player.Bottom.Distance(Projectile.Top) > 8f * SlimyPulleyMod2;
							bool flag3 = player.controlUp && player.Top.Distance(Projectile.Bottom) > 16f;
							bool flag4 = player.controlDown && player.Bottom.Distance(other.Top) > 8f * SlimyPulleyMod2;
							bool condition = Projectile.Center.Y > other.Center.Y && (flag || flag2);
							bool condition2 = flag3 || flag4;
							bool tooClose = (player.controlUp && player.Top.Distance(other.Bottom) < 16f) || (player.controlDown && player.Bottom.Distance(Projectile.Top) < 8f * SlimyPulleyMod2) || (player.controlUp && player.Top.Distance(Projectile.Bottom) < 16f) || (player.controlDown && player.Bottom.Distance(other.Top) < 8f * SlimyPulleyMod2);
							#endregion

							if (condition)
							{
								player.position -= distToProj * 8f * pPlr.PulleySpeed * SlimyPulleyMod * (player.controlUp ? -1f : 1f);
							}
							else if (condition2)
							{
								player.position += distToProj * 8f * pPlr.PulleySpeed * (player.controlUp ? -1f : 1f);
							}

							if (Collision.SolidCollision(player.position, player.width, player.height) || tooClose)
							{
								player.position = player.oldPosition;
							}
						}

						if (player.position != player.oldPosition)
						{
							pPlr.MovingOnZipline = true; // Triggers movement-based pulley effects

							float velocity = (float)player.position.Distance(player.oldPosition);
							player.legFrame.Y = player.legFrame.Height * 5; // Animate the player's legs

							player.pulleyFrameCounter += velocity * 0.75f;
							if (player.pulleyFrameCounter >= 10f)
							{
								player.pulleyFrame++;
								player.pulleyFrameCounter = 0f;

								if (player.pulleyFrame > 1)
									player.pulleyFrame = 0;
							}
						}
						else
						{
							pPlr.MovingOnZipline = false;
						}
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (IsRight)
			{
				Projectile other = null;
				for (int p = 0; p < Main.maxProjectiles; p++)
				{
					Projectile proj = Main.projectile[p];
					if (proj.active && proj.type == Projectile.type && proj.whoAmI != Projectile.whoAmI && proj.ai[0] == 0)
					{
						other = proj;
						break;
					}
				}

				if (other != null && Projectile.Distance(other.Center) < 2048f) // Other tether has to be on-screen for them to connect
				{
					if (!Collision.CanHitLine(Projectile.position, 1, 1, other.position, 1, 1))
					{
						Vector2 vect = Projectile.Center;
						while (!Collision.SolidCollision(vect, 1, 1) && vect.Distance(other.Center) > 32f)
						{
							Vector2 dirTo = vect.DirectionTo(other.Center);
							dirTo.Normalize();
							vect += dirTo * 16;
						}

						Texture2D texture = TextureAssets.Cd.Value;
						Main.EntitySpriteDraw(texture, vect - Main.screenPosition, null, Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
						return base.PreDraw(ref lightColor);
					}

					Texture2D tex = Request<Texture2D>(Texture + "_Tether").Value;
					Vector2 targetPos = other.Center;
					Vector2 center = Projectile.Center;
					Vector2 distToProj = targetPos - center;
					float projRotation = distToProj.ToRotation() - MathHelper.PiOver2;
					float distance = distToProj.Length();
					Color drawColor;

					while (true) // Continue doing this forever until we break the loop
					{
						if (distance < 12f || float.IsNaN(distance)) // If we're close enough, we can break out of this loop
							break;

						distToProj.Normalize();
						distToProj *= 8f;
						center += distToProj;
						distToProj = targetPos - center;
						distance = distToProj.Length();

						Projectile dummy = new();
						dummy.Center = center;
						drawColor = Lighting.GetColor(dummy.Center.ToTileCoordinates(), Color.White);

						Main.EntitySpriteDraw(tex, center - Main.screenPosition, null, drawColor, projRotation, tex.Size() * 0.5f, 1f, SpriteEffects.None, 0);
					}

					// We also need to draw the left one so the rope draws behind it
					// Aiui the DrawBehind() function has to either affect all projectiles of a given type, or none at all, so we can't use that
					tex = Request<Texture2D>(Texture).Value;
					drawColor = Lighting.GetColor(other.Center.ToTileCoordinates(), Color.White);
					Main.EntitySpriteDraw(tex, other.Center - Main.screenPosition, null, drawColor, other.rotation, tex.Size() * 0.5f, 1f, SpriteEffects.None, 0);
				}
			}

			return base.PreDraw(ref lightColor);
		}
	}
}