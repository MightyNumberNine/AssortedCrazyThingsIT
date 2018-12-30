using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;

namespace AssortedCrazyThings.Items.Pets
{
	public class CuteSlimeRainbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Rainbow Slime");
			Tooltip.SetDefault("Summons a friendly Cute Rainbow Slime to follow you."
						+ "\nLegacy Appearance");
		}
		
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.LizardEgg);
			item.shoot = mod.ProjectileType<CuteSlimeRainbowPet>();
			item.buffType = mod.BuffType<CuteSlimeRainbowBuff>();
			item.rare = -11;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "CuteSlimeRainbowNew");
			recipe.AddTile(TileID.Solidifier);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}