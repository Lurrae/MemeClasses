using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using System;

namespace MemeClasses.Items
{
    internal class MagicIceRod : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.IceRod);
            Item.mana = 2;
            Item.shoot = ModContent.ProjectileType<Projectiles.IceRope>();
        }
    }

	internal class IceRope : ModProjectile
	{


		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.IceBlock);
		}

		Tile tile4;

		public override void AI()
		{
			int num148;
			int num147;

			if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
			{
				Projectile.alpha = 255;
			}
			Dust dust99;
			Dust dust195;
			if (Projectile.ai[1] < 0f)
			{
				if (Projectile.timeLeft > 60)
				{
					Projectile.timeLeft = 60;
				}
				if (Projectile.velocity.X > 0f)
				{
					Projectile.rotation += 0.3f;
				}
				else
				{
					Projectile.rotation -= 0.3f;
				}
				int num134 = (int)(Projectile.position.X / 16f) - 1;
				int num135 = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 2;
				int num136 = (int)(Projectile.position.Y / 16f) - 1;
				int num137 = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 2;
				if (num134 < 0)
				{
					num134 = 0;
				}
				if (num135 > Main.maxTilesX)
				{
					num135 = Main.maxTilesX;
				}
				if (num136 < 0)
				{
					num136 = 0;
				}
				if (num137 > Main.maxTilesY)
				{
					num137 = Main.maxTilesY;
				}
				int num138 = (int)Projectile.position.X + 4;
				int num139 = (int)Projectile.position.Y + 4;
				Vector2 vector15 = default(Vector2);
				for (int num141 = num134; num141 < num135; num141++)
				{
					for (int num142 = num136; num142 < num137; num142++)
					{
						if (!(Main.tile[num141, num142] != null))
						{
							continue;
						}
						tile4 = Main.tile[num141, num142];
						if (!tile4.HasTile)
						{
							continue;
						}
						tile4 = Main.tile[num141, num142];
						if (tile4.TileType == 127)
						{
							continue;
						}
						bool[] tileSolid2 = Main.tileSolid;
						tile4 = Main.tile[num141, num142];
						if (!tileSolid2[tile4.TileType])
						{
							continue;
						}
						bool[] tileSolidTop2 = Main.tileSolidTop;
						tile4 = Main.tile[num141, num142];
						if (!tileSolidTop2[tile4.TileType])
						{
							vector15.X = num141 * 16;
							vector15.Y = num142 * 16;
							if ((float)(num138 + 8) > vector15.X && (float)num138 < vector15.X + 16f && (float)(num139 + 8) > vector15.Y && (float)num139 < vector15.Y + 16f)
							{
								Projectile.Kill();
							}
						}
					}
				}
				int num143 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod);
				Main.dust[num143].noGravity = true;
				dust99 = Main.dust[num143];
				dust195 = dust99;
				dust195.velocity *= 0.3f;
				return;
			}
			if (Projectile.ai[0] < 0f)
			{
				if (Projectile.ai[0] == -1f)
				{
					for (int num144 = 0; num144 < 10; num144++)
					{
						int num145 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod, 0f, 0f, 0, default(Color), 1.1f);
						Main.dust[num145].noGravity = true;
						dust99 = Main.dust[num145];
						dust195 = dust99;
						dust195.velocity *= 1.3f;
					}
				}
				else if (Main.rand.NextBool(30))
				{
					int num146 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod, 0f, 0f, 100);
					dust99 = Main.dust[num146];
					dust195 = dust99;
					dust195.velocity *= 0.2f;
				}
				num147 = (int)Projectile.position.X / 16;
				num148 = (int)Projectile.position.Y / 16;
				if (!(Main.tile[num147, num148] == null))
				{
					tile4 = Main.tile[num147, num148];
					if (tile4.HasTile)
					{
						goto IL_8aee;
					}
				}
				Projectile.Kill();
				goto IL_8aee;
			}
			int num149 = (int)(Projectile.position.X / 16f) - 1;
			int num150 = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 2;
			int num152 = (int)(Projectile.position.Y / 16f) - 1;
			int num153 = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 2;
			if (num149 < 0)
			{
				num149 = 0;
			}
			if (num150 > Main.maxTilesX)
			{
				num150 = Main.maxTilesX;
			}
			if (num152 < 0)
			{
				num152 = 0;
			}
			if (num153 > Main.maxTilesY)
			{
				num153 = Main.maxTilesY;
			}
			int num154 = (int)Projectile.position.X + 4;
			int num155 = (int)Projectile.position.Y + 4;
			Vector2 vector16 = default(Vector2);
			for (int num156 = num149; num156 < num150; num156++)
			{
				for (int num157 = num152; num157 < num153; num157++)
				{
					if (!(Main.tile[num156, num157] != null))
					{
						continue;
					}
					tile4 = Main.tile[num156, num157];
					if (!tile4.HasUnactuatedTile)
					{
						continue;
					}
					tile4 = Main.tile[num156, num157];
					if (tile4.TileType == 127)
					{
						continue;
					}
					bool[] tileSolid3 = Main.tileSolid;
					tile4 = Main.tile[num156, num157];
					if (!tileSolid3[tile4.TileType])
					{
						continue;
					}
					bool[] tileSolidTop3 = Main.tileSolidTop;
					tile4 = Main.tile[num156, num157];
					if (!tileSolidTop3[tile4.TileType])
					{
						vector16.X = num156 * 16;
						vector16.Y = num157 * 16;
						if ((float)(num154 + 8) > vector16.X && (float)num154 < vector16.X + 16f && (float)(num155 + 8) > vector16.Y && (float)num155 < vector16.Y + 16f)
						{
							Projectile.Kill();
						}
					}
				}
			}
			if (Projectile.lavaWet)
			{
				Projectile.Kill();
			}
			if (!Projectile.active)
			{
				return;
			}
			int num158 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.IceRod);
			Main.dust[num158].noGravity = true;
			dust99 = Main.dust[num158];
			dust195 = dust99;
			dust195.velocity *= 0.3f;
			int num159 = (int)Projectile.ai[0];
			int num160 = (int)Projectile.ai[1];
			if (WorldGen.InWorld(num159, num160) && WorldGen.SolidTile(num159, num160))
			{
				if (Math.Abs(Projectile.velocity.X) > Math.Abs(Projectile.velocity.Y))
				{
					if (Projectile.Center.Y < (float)(num160 * 16 + 8) && WorldGen.InWorld(num159, num160 - 1) && !WorldGen.SolidTile(num159, num160 - 1))
					{
						num160--;
					}
					else if (WorldGen.InWorld(num159, num160 + 1) && !WorldGen.SolidTile(num159, num160 + 1))
					{
						num160++;
					}
					else if (WorldGen.InWorld(num159, num160 - 1) && !WorldGen.SolidTile(num159, num160 - 1))
					{
						num160--;
					}
					else if (Projectile.Center.X < (float)(num159 * 16 + 8) && WorldGen.InWorld(num159 - 1, num160) && !WorldGen.SolidTile(num159 - 1, num160))
					{
						num159--;
					}
					else if (WorldGen.InWorld(num159 + 1, num160) && !WorldGen.SolidTile(num159 + 1, num160))
					{
						num159++;
					}
					else if (WorldGen.InWorld(num159 - 1, num160) && !WorldGen.SolidTile(num159 - 1, num160))
					{
						num159--;
					}
				}
				else if (Projectile.Center.X < (float)(num159 * 16 + 8) && WorldGen.InWorld(num159 - 1, num160) && !WorldGen.SolidTile(num159 - 1, num160))
				{
					num159--;
				}
				else if (WorldGen.InWorld(num159 + 1, num160) && !WorldGen.SolidTile(num159 + 1, num160))
				{
					num159++;
				}
				else if (WorldGen.InWorld(num159 - 1, num160) && !WorldGen.SolidTile(num159 - 1, num160))
				{
					num159--;
				}
				else if (Projectile.Center.Y < (float)(num160 * 16 + 8) && WorldGen.InWorld(num159, num160 - 1) && !WorldGen.SolidTile(num159, num160 - 1))
				{
					num160--;
				}
				else if (WorldGen.InWorld(num159, num160 + 1) && !WorldGen.SolidTile(num159, num160 + 1))
				{
					num160++;
				}
				else if (WorldGen.InWorld(num159, num160 - 1) && !WorldGen.SolidTile(num159, num160 - 1))
				{
					num160--;
				}
			}
			if (Projectile.velocity.X > 0f)
			{
				Projectile.rotation += 0.3f;
			}
			else
			{
				Projectile.rotation -= 0.3f;
			}
			if (Main.myPlayer != Projectile.owner)
			{
				return;
			}
			int num161 = (int)((Projectile.position.X + (float)(Projectile.width / 2)) / 16f);
			int num164 = (int)((Projectile.position.Y + (float)(Projectile.height / 2)) / 16f);
			bool flag45 = false;
			if (num161 == num159 && num164 == num160)
			{
				flag45 = true;
			}
			if (((Projectile.velocity.X <= 0f && num161 <= num159) || (Projectile.velocity.X >= 0f && num161 >= num159)) && ((Projectile.velocity.Y <= 0f && num164 <= num160) || (Projectile.velocity.Y >= 0f && num164 >= num160)))
			{
				flag45 = true;
			}
			if (!flag45)
			{
				return;
			}
			if (WorldGen.PlaceTile(num159, num160, 127, mute: false, forced: false, Projectile.owner))
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, (int)Projectile.ai[0], (int)Projectile.ai[1], 127);
				}
				Projectile.damage = 0;
				Projectile.ai[0] = -1f;
				Projectile.velocity *= 0f;
				Projectile.alpha = 255;
				Projectile.position.X = num159 * 16;
				Projectile.position.Y = num160 * 16;
				Projectile.netUpdate = true;
			}
			else
			{
				Projectile.ai[1] = -1f;
			}
			return;

		IL_8aee:
			Projectile.ai[0] -= 1f;
			if (!(Projectile.ai[0] <= -900f) || (Main.myPlayer != Projectile.owner && Main.netMode != NetmodeID.Server))
			{
				return;
			}
			tile4 = Main.tile[num147, num148];
			if (!tile4.HasTile)
			{
				return;
			}
			tile4 = Main.tile[num147, num148];
			if (tile4.TileType == 127)
			{
				WorldGen.KillTile(num147, num148);
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, num147, num148);
				}
				Projectile.Kill();
			}
			return;
		}
	}

	public class MagicIceRope : ModTile
	{

		public override void SetStaticDefaults()
		{


			Main.tileSolid[Type] = false;
			Main.tileRope[Type] = true;

			DustType = DustID.IceRod;


			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(CanPlace, -1, 0, true);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.addTile(Type);
		}
		public static int CanPlace(int i, int j, int type, int style, int direction, int alternative)
		{
			return 0;
		}
	}
}
