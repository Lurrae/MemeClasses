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
	public class Splinter : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = GetInstance<PulleyDamageClass>();
			Projectile.penetrate = 6;
		}

		public override void AI()
		{
			Projectile.rotation++;
			Projectile.velocity.Y++;
		}
	}
}