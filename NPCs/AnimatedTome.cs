using AssortedCrazyThings.Items.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    //this version of "retexture" is old and not recommended, refer to DemonEyeFractured or similar

    public class AnimatedTome : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Animated Tome");
            Main.npcFrameCount[NPC.type] = 5;
            Main.npcCatchable[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 16;
            NPC.damage = 13;
            NPC.defense = 2;
            NPC.lifeMax = 16;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.8f;
            NPC.aiStyle = 14;
            NPC.noGravity = true;
            AIType = NPCID.GiantBat;
            NPC.catchItem = (short)ModContent.ItemType<AnimatedTomeItem>();
        }

        public override void FindFrame(int frameHeight)
        {
            if (++NPC.frameCounter >= 8)
            {
                NPC.frameCounter = 0;

                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.DungeonNormal.Chance * 0.01f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.getRect(), ItemID.Book);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int u = 0; u < 8; u++)
                {
                    Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                    Gore gore = Gore.NewGoreDirect(pos, NPC.velocity * 0.5f, Mod.Find<ModGore>("PaperGore").Type, 1f);
                    gore.velocity += new Vector2(Main.rand.NextFloat(3) - 1f, Main.rand.NextFloat(MathHelper.TwoPi) - 0.3f);
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            int i = NPC.whoAmI % 5; //needs to be fixed per NPC instance
            if (i < 4)
            {
                Texture2D texture = Mod.GetTexture("NPCs/AnimatedTome_" + i).Value;
                Vector2 stupidOffset = new Vector2(0f, 0f); //4f
                SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawPos = NPC.Center - Main.screenPosition - Vector2.Zero + stupidOffset;
                spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            }
        }

        public override void PostAI()
        {
            NPC.rotation = NPC.velocity.X * 0.06f;

            if (Main.rand.NextBool(10))
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 15);
            }
        }
    }
}
