using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.Items.Pets
{
    public class CuteSlimeRainbowNew : CuteSlimeItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bottled Cute Rainbow Slime");
            Tooltip.SetDefault("Summons a friendly Cute Rainbow Slime to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LizardEgg);
            item.shoot = mod.ProjectileType<CuteSlimeRainbowNewProj>();
            item.buffType = mod.BuffType<CuteSlimeRainbowNewBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
        }
    }
}