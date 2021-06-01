using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class JoyousSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Joyous Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.ToxicSludge];
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 32;
            NPC.damage = 7;
            NPC.defense = 2;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 25f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 1;
            AIType = NPCID.ToxicSludge;
            AnimationType = NPCID.ToxicSludge;
            NPC.friendly = true;
            NPC.alpha = 175;
            NPC.color = new Color(169, 141, 255, 100);
            NPC.catchItem = (short)ModContent.ItemType<JoyousSlimeItem>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDay.Chance * 0.015f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Gel);
        }
    }
}
