using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Meatball : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meatball");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
        }

        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 26;
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 25;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 25f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = 1;
            aiType = NPCID.ToxicSludge;
            animationType = NPCID.ToxicSludge;
			Main.npcCatchable[mod.NPCType("Meatball")] = true;
			npc.catchItem = (short)mod.ItemType("MeatballItem");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode) return SpawnCondition.Crimson.Chance * 0.05f;
            return SpawnCondition.Crimson.Chance * 0.2f;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Vertebrae);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.life <= 0 && Main.rand.NextBool(10))
                {
                    NPC.NewNPC((int)npc.position.X, (int)npc.position.Y - 16, mod.NPCType("MeatballsEye"));
                }
            }
        }
    }
}
