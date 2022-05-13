using MemeClasses.Items.Pulleys;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace MemeClasses.Projectiles
{
	public class ModGlobalProj : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		private bool ChainLightning = false;

		// Make lightning bolts spawned by the Mechanical Pulley "bounce" to nearby enemies
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (projectile.type == ProjectileID.MagnetSphereBolt && source is EntitySource_ItemUse { Context: "MechPulley_Bolt" })
			{
				ChainLightning = true;
				projectile.usesLocalNPCImmunity = true;
				projectile.localNPCHitCooldown = -1;
			}
		}

		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.type == ProjectileID.MagnetSphereBolt && ChainLightning)
			{
				projectile.localNPCImmunity[target.whoAmI] = -1;

				for (int n = 0; n < Main.maxNPCs; n++)
				{
					NPC npc = Main.npc[n];

					if (!npc.CanBeChasedBy() || target.whoAmI == npc.whoAmI || projectile.localNPCImmunity[npc.whoAmI] != 0 || target.Distance(npc.Center) > 256f)
					{
						continue; // Skip any NPCs we can't target (including the one we've already hit)
					}

					Vector2 targetPos = npc.Center;
					Vector2 newVel = targetPos - projectile.Center;
					newVel.Normalize();

					projectile.velocity = newVel * 5f;
					break; // Only bounce to one extra enemy at a time
				}
			}
		}
	}
}