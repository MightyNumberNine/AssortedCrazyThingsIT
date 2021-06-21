using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    [Autoload]
    public class JoyousSlime : AssNPC
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
            NPC.dontTakeDamageFromHostiles = true;
            NPC.alpha = 175;
            NPC.color = new Color(169, 141, 255, 100);
            NPC.catchItem = (short)ModContent.ItemType<JoyousSlimeItem>();
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return null; //TODO NPC return true
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return !projectile.minion;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDay.Chance * 0.015f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A passive variant of slime with a friendly face. At night, it tries to be close to an equally friendly face.")
            });
        }
    }
}
