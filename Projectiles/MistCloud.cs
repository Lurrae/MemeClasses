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
	public class MistCloud : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SporeCloud);
			AIType = ProjectileID.SporeCloud;
			Projectile.DamageType = GetInstance<PulleyDamageClass>();
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			target.AddBuff(BuffID.Chilled, 600); // 10 seconds of slowness
		}
	}
}