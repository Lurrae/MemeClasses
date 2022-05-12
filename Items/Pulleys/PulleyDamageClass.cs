using Terraria.ModLoader;

namespace MemeClasses.Items.Pulleys
{
	public class PulleyDamageClass : DamageClass
	{
		public override void SetStaticDefaults()
		{
			ClassName.SetDefault("pulley damage");
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Generic)
				return StatInheritanceData.Full;

			return StatInheritanceData.None;
		}

		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			return false;
		}
	}
}