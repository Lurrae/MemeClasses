using MemeClasses.Items.Accessories;
using MemeClasses.Items.Pulleys;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace LurreniaMod
{
	// Pretty much all of the code for generating chest loot was adapted from Clicker Class' ClickerWorld.cs file
	public enum ChestType
	{
		Wood,
		Gold,
		LockedGold,
		Shadow,
		LockedShadow,
		Barrel,
		TrashCan,
		Ebonwood,
		RichMahogany,
		Pearlwood,
		Ivy,
		Frozen,
		LivingWood,
		Skyware,
		ShadeWood,
		Webbed,
		Lihzahrd,
		Water,
		JungleBiome,
		CorruptBiome,
		CrimsonBiome,
		HallowBiome,
		IceBiome,
		LockedJungleBiome,
		LockedCorruptBiome,
		LockedCrimsonBiome,
		LockedHallowBiome,
		LockedIceBiome,
		Dynasty,
		Honey,
		Steampunk,
		PalmWood,
		GlowingMushroom,
		BorealWood,
		BlueSlime,
		GreenDungeon,
		LockedGreenDungeon,
		PinkDungeon,
		LockedPinkDungeon,
		BlueDungeon,
		LockedBlueDungeon,
		Bone,
		Cactus,
		Flesh,
		Obsidian,
		Pumpkin,
		Spooky,
		Glass,
		Martian,
		Meteorite,
		Granite,
		Marble,
		Crystal,
		PirateGolden,
		// From this index onwards, everything here is part of TileID.Containers2
		CrystalReal,
		PirateGoldenReal,
		Spider,
		Lesion,
		DeadMans,
		Solar,
		Vortex,
		Nebula,
		Stardust,
		GolfBall,
		Sandstone,
		Bamboo,
		DesertBiome,
		LockedDesertBiome
	}

	public class ChestLoot : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			// Extra chest loot
			int GenIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
			if (GenIndex != -1)
				tasks.Insert(GenIndex++, new PassLegacy("MemeClasses: New Chest Loot", GenerateChestLoot));
		}

		private void GenerateChestLoot(GenerationProgress progress, GameConfiguration config)
		{
			progress.Message = "MemeClasses: New Chest Loot";

			// The following chests have the same loot tables as Gold Chests with no variations
			HashSet<ChestType> SameAsGold = new()
			{
				ChestType.RichMahogany,
				ChestType.DeadMans
			};

			// Same as above, but these can have additional items not normally included in Gold Chests
			HashSet<ChestType> LikeGold = new()
			{
				ChestType.GlowingMushroom,
				ChestType.Granite,
				ChestType.Marble
			};

			Dictionary<ChestType, List<int>> chestLoot = new()
			{
				{ ChestType.Gold, new List<int> { } }, // Only uncommented so the game won't freak out while generating the chest loot
				//{ ChestType.Frozen, new List<int> { ItemType<MistPulley>() } },
				{ ChestType.GlowingMushroom, new List<int> { ItemType<ShroomPulley>() } },
				//{ ChestType.Granite, new List<int> { ItemType<GranitePulley>() } },
				//{ ChestType.Marble, new List<int> { ItemType<MarblePulley>() } }
			};

			// Which chests should have new items added to them?
			// This should always contain every chest type in the "chestLoot" dictionary
			HashSet<ChestType> UsedChestTypes = new()
			{
				// Because we did not include Gold Chests here, those won't generate items. Which is good, because they have no items and might break things
				//ChestType.Frozen,
				ChestType.GlowingMushroom,
				//ChestType.Granite,
				//ChestType.Marble
			};

			Dictionary<ChestType, List<Chest>> chestLists = new();

			for (int c = 0; c < Main.maxChests; c++)
			{
				Chest chest = Main.chest[c];
				if (chest == null || !WorldGen.InWorld(chest.x, chest.y, 42)) // Exclude chests that are out of bounds, since then players wouldn't be able to get the item
					continue;

				Tile chestTile = Main.tile[chest.x, chest.y];
				if (chest.item == null || (chestTile.TileType != TileID.Containers && chestTile.TileType != TileID.Containers2))
					continue;

				int chestTypeOffset = 0;
				if (chestTile.TileType == TileID.Containers2)
					chestTypeOffset = 54; // This is needed so that we can add this to the Containers2 chests' IDs, since they're stored on a separate spritesheet

				int typeNum = chestTile.TileFrameX / 36 + chestTypeOffset;

				ChestType type = (ChestType)typeNum;
				if (UsedChestTypes.Contains(type))
				{
					// For The Worthy makes Floating Islands spawn with Locked Gold Chests instead of Skyplate Chests, so an exception has to be made for those
					if (type == ChestType.LockedGold && chestTile.WallType == WallID.DiscWall && WorldGen.getGoodWorldGen)
						type = ChestType.Skyware;

					// Wooden Chests that spawn in the dungeon have a guaranteed Golden Key as a primary item, so we ignore those
					if (type == ChestType.Wood && Main.wallDungeon[chestTile.WallType])
						continue;

					// Make Dead Man's/Rich Mahogany Chests contain the same items as ordinary Gold Chests
					if (SameAsGold.Contains(type))
						type = ChestType.Gold;

					// Pyramids are one of the rarest structures in the game, and have some of the best loot of any pre-boss chest, so we do NOT want to overwrite their loot
					if (type == ChestType.Gold && chestTile.WallType == WallID.SandstoneBrick)
						continue;

					// Create a new list if one doesn't exist yet
					if (!chestLists.ContainsKey(type))
						chestLists[type] = new List<Chest>();

					chestLists[type].Add(chest);
				}
			}

			foreach (var chestType in UsedChestTypes)
			{
				if (chestLists.ContainsKey(chestType))
					ReplacePrimaryChestLoot(chestLists[chestType], chestLoot[chestType], LikeGold.Contains(chestType), chestLoot[ChestType.Gold]);
			}
		}

		private static void ReplacePrimaryChestLoot(IList<Chest> chestList, IList<int> primaryItems, bool likeGold, IList<int> goldItems, int rareSlots = 1, Func<int, IList<Chest>> generateChestFunc = null)
		{
			Dictionary<int, List<Chest>> chestsWithItem = new(); // Allows us to check how many chests contain a given item
			List<Chest> availableChests = new(); // A list of chests which can be modified

			int itemCount = primaryItems.Count;
			int itemChoice = 0;
			int slot = 0;

			// First things first, look through all the chests we were provided with, and catalog every item within them
			// Store them in the chestsWithItem dictionary so that we can find which chests contain what item later
			for (int i = 0; i < chestList.Count; i++)
			{
				Chest chest = chestList[i];
				Item chestItem = chest.item[slot];
				// Ignore chests which have no item to replace
				if (chestItem == null || chestItem.IsAir)
					continue;

				// Create a new list for items we haven't found before
				if (!chestsWithItem.ContainsKey(chestItem.type))
					chestsWithItem[chestItem.type] = new List<Chest>();

				chestsWithItem[chestItem.type].Add(chest);
			}

			// Next step: Look through everything we've found so far, and remove one random chest from each item's list- this removed chest will not be modified
			// That way, we guarantee that every vanilla item will have spawned in at least one chest in the world
			// This should also do the same for every modded item that spawns in these chests too, provided that those mods modify chest loot before us
			foreach (var list in chestsWithItem.Values)
			{
				list.RemoveAt(WorldGen.genRand.Next(list.Count));
				if (list.Count > 0)
					availableChests.AddRange(list);
			}

			int itemsInChestsCount = chestsWithItem.Keys.Count;

			// Now, we have to check if we found enough chests to add every item we want to generate
			// If we didn't, we have to get a little bit creative- if we have a function to generate chests, we use that
			// Otherwise, we can just add our item to an available slot of a vanilla chest
			// In theory, I think this should only happen with Floating Islands on a small world, since small worlds only have three Floating Islands, and vanilla has three items that can be obtained there
			if (availableChests.Count < itemCount)
			{
				int neededChests = itemCount - availableChests.Count;
				if (generateChestFunc != null) // This is for if we were given a function to use instead of just yeeting our items into a slot of a random chest
				{
					IList<Chest> chests = generateChestFunc(neededChests);
					availableChests.AddRange(chests);
					for (int i = 0; i < chests.Count; i++)
						chestList.Add(chests[i]);
				}
				// If we don't have a function provided to us to generate chests automatically, we just look through all the available chests a second time
				// This time, though, we move every item from the second slot onwards over one, leaving the second slot open for our item to be added there instead
				// This does unfortunately mean that if a chest is completely full, the last item will get deleted, but chests should never be that full when the world generates anyways
				else
				{
					availableChests = new List<Chest>(chestList);
					while (availableChests.Count > neededChests)
						availableChests.RemoveAt(WorldGen.genRand.Next(availableChests.Count));

					for (int i = 0; i < availableChests.Count; i++)
					{
						Chest chest = availableChests[i];
						for (int k = chest.item.Length - 1; k < slot + rareSlots; k--)
						{
							chest.item[k] = chest.item[k - 1];
						}

						chest.item[slot + rareSlots] = new Item();
					}

					slot += rareSlots;
					rareSlots = 1;
				}
			}

			// Now, finally, we can generate our items!
			while (availableChests.Count > 0)
			{
				// First, gotta get a random chest from the list of available ones...
				int index = WorldGen.genRand.Next(availableChests.Count);
				Chest chest = availableChests[index];

				// Have we already added at least one of every item we have to add?
				// If so, give a random chance to generate a duplicate somewhere in the world
				if (itemChoice < itemCount || WorldGen.genRand.Next(itemsInChestsCount + itemCount) < itemCount)
				{
					int tempRareSlots = rareSlots;
					if (chest.item[slot].type == ItemID.FlareGun) // Make sure that if we find a Flare Gun, we remove the Flares that come with it as well
					{
						tempRareSlots = 2;
					}

					while (tempRareSlots > 1)
					{
						for (int i = slot + tempRareSlots - 1; i < chest.item.Length - 1; i++)
						{
							chest.item[i] = chest.item[i + 1];
						}

						chest.item[^1] = new Item();
						tempRareSlots--;
					}
					if (itemChoice > itemCount)
					{
						int itemType = WorldGen.genRand.Next(primaryItems);
						// We have no Gold Chest items yet, so I'm commenting this out for now
						/*if (likeGold && Main.rand.NextBool(10)) // 1/10 chance for Mushroom/Granite/Marble Chests to contain Gold Chest loot
						{
							itemType = WorldGen.genRand.Next(goldItems);
						}*/

						chest.item[slot].SetDefaults(itemType);
						chest.item[slot].Prefix(-1);
					}
					else
					{
						chest.item[slot].SetDefaults(primaryItems[itemChoice]);
						chest.item[slot].Prefix(-1);
						itemChoice++;
					}
				}
				availableChests.RemoveAt(index);
			}
			return;
		}
	}
}
