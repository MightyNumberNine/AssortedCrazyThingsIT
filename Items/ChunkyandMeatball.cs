using Terraria;

namespace AssortedCrazyThings.Items
{
	[Content(ContentType.HostileNPCs)]
	public abstract class ChunkyandMeatballItem : AssItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = 1;
		}
	}

	public class ChunkysEyeItem : ChunkyandMeatballItem
	{

	}

	public class MeatballsEyeItem : ChunkyandMeatballItem
	{

	}
}
