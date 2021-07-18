using AssortedCrazyThings.Base;
using AssortedCrazyThings.Effects;
using AssortedCrazyThings.Items.Pets.CuteSlimes;
using AssortedCrazyThings.Items.Weapons;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings
{
    class AssortedCrazyThings : Mod
    {
        //Soul item animated textures
        public const int animatedSoulFrameCount = 6;
        public static Texture2D[] animatedSoulTextures;

        /// <summary>
        /// Soul NPC spawn blacklist
        /// </summary>
        public static int[] soulBuffBlacklist;

        public static int[] harvesterTypes;
        public static int harvesterTalonLeft;
        public static int harvesterTalonRight;

        //Mod Helpers compat
        public static string GithubUserName { get { return "Werebearguy"; } }
        public static string GithubProjectName { get { return "AssortedCrazyThings"; } }

        private void LoadSoulBuffBlacklist()
        {
            List<int> tempList = new List<int>
            {
                NPCID.Bee,
                NPCID.BeeSmall,
                NPCID.BlueSlime,
                NPCID.BlazingWheel,
                NPCID.EaterofWorldsHead,
                NPCID.EaterofWorldsBody,
                NPCID.EaterofWorldsTail,
                NPCID.Creeper,
                NPCID.GolemFistLeft,
                NPCID.GolemFistRight,
                NPCID.PlanterasHook,
                NPCID.PlanterasTentacle,
                NPCID.Probe,
                NPCID.ServantofCthulhu,
                NPCID.SlimeSpiked,
                NPCID.SpikeBall,
                NPCID.TheHungry,
                NPCID.TheHungryII,
            };

            soulBuffBlacklist = tempList.ToArray();
        }

        /// <summary>
        /// Assuming this is called after InitSoulBuffBlacklist.
        /// Adds NPC types to soulBuffBlacklist manually
        /// </summary>
        private void AddToSoulBuffBlacklist()
        {
            if (!ContentConfig.Instance.Bosses)
            {
                return;
            }

            //assuming this is called after InitSoulBuffBlacklist
            List<int> tempList = new List<int>(soulBuffBlacklist)
            {
                ModContent.NPCType<DungeonSoul>(),
                ModContent.NPCType<DungeonSoulFreed>()
            };

            soulBuffBlacklist = tempList.ToArray();
            Array.Sort(soulBuffBlacklist);
        }

        /// <summary>
        /// Fills isModdedWormBodyOrTail with types of modded NPCs which names are ending with Body or Tail (indicating they are part of something)
        /// </summary>
        private void LoadWormList()
        {
            List<int> tempList = new List<int>();

            for (int i = Main.maxNPCTypes; i < NPCLoader.NPCCount; i++)
            {
                ModNPC modNPC = NPCLoader.GetNPC(i);
                if (modNPC != null && (modNPC.GetType().Name.EndsWith("Body") || modNPC.GetType().Name.EndsWith("Tail")))
                {
                    tempList.Add(modNPC.NPC.type);
                }
            }

            AssUtils.isModdedWormBodyOrTail = tempList.ToArray();
            Array.Sort(AssUtils.isModdedWormBodyOrTail);
        }

        private void LoadHarvesterTypes()
        {
            harvesterTypes = new int[5];

            if (!ContentConfig.Instance.Bosses)
            {
                return;
            }

            harvesterTypes[0] = ModContent.NPCType<Harvester1>();
            harvesterTypes[1] = ModContent.NPCType<Harvester2>();
            harvesterTypes[2] = ModContent.NPCType<Harvester>();
            harvesterTypes[3] = harvesterTalonLeft = ModContent.NPCType<HarvesterTalonLeft>();
            harvesterTypes[4] = harvesterTalonRight = ModContent.NPCType<HarvesterTalonRight>();
        }

        private void UnloadHarvesterTypes()
        {
            harvesterTypes = null;
        }

        private void LoadMisc()
        {
            if (!Main.dedServ && ContentConfig.Instance.Bosses)
            {
                animatedSoulTextures = new Texture2D[2];

                animatedSoulTextures[0] = Assets.Request<Texture2D>("Items/CaughtDungeonSoulAnimated").Value;
                animatedSoulTextures[1] = Assets.Request<Texture2D>("Items/CaughtDungeonSoulFreedAnimated").Value;
            }
        }

        private void UnloadMisc()
        {
            animatedSoulTextures = null;

            PetEaterofWorldsBase.wormTypes = null;

            PetDestroyerBase.wormTypes = null;
        }

        public override void Load()
        {
            ConfigurationSystem.Load();

            ShaderManager.Load();

            LoadHarvesterTypes();

            LoadSoulBuffBlacklist();

            LoadMisc();
        }

        public override void Unload()
        {
            ConfigurationSystem.Unload();

            ShaderManager.Unload();

            UnloadHarvesterTypes();

            UnloadMisc();

            GitgudData.Unload();

            EverhallowedLantern.DoUnload();
        }

        public override void PostSetupContent()
        {
            //for things that have to be called after Load() because of Main.projFrames[projectile.type] calls (and similar)
            LoadWormList();

            GitgudData.Load();

            DroneController.DoLoad();

            EverhallowedLantern.DoLoad();

            AddToSoulBuffBlacklist();

            if (ContentConfig.Instance.DroppedPets)
            {
                PetEaterofWorldsBase.wormTypes = new int[]
                {
                ModContent.ProjectileType<PetEaterofWorldsHead>(),
                ModContent.ProjectileType<PetEaterofWorldsBody1>(),
                ModContent.ProjectileType<PetEaterofWorldsBody2>(),
                ModContent.ProjectileType<PetEaterofWorldsTail>()
                };

                PetDestroyerBase.wormTypes = new int[]
                {
                ModContent.ProjectileType<PetDestroyerHead>(),
                ModContent.ProjectileType<PetDestroyerBody1>(),
                ModContent.ProjectileType<PetDestroyerBody2>(),
                ModContent.ProjectileType<PetDestroyerTail>()
                };
            }
        }

        public override void AddRecipeGroups()
        {
            if (ContentConfig.Instance.CuteSlimes && ContentConfig.Instance.Placeables)
            {
                RecipeGroup.RegisterGroup("ACT:RegularCuteSlimes", new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Regular Bottled Slime", new int[]
                {
                ModContent.ItemType<CuteSlimeBlueItem>(),
                ModContent.ItemType<CuteSlimeBlackItem>(),
                ModContent.ItemType<CuteSlimeGreenItem>(),
                ModContent.ItemType<CuteSlimePinkItem>(),
                ModContent.ItemType<CuteSlimePurpleItem>(),
                ModContent.ItemType<CuteSlimeRainbowItem>(),
                ModContent.ItemType<CuteSlimeRedItem>(),
                ModContent.ItemType<CuteSlimeYellowItem>()
                }));
            }

            RecipeGroup.RegisterGroup("ACT:GoldPlatinum", new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ItemID.GoldBar), new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            }));

            RecipeGroup.RegisterGroup("ACT:AdamantiteTitanium", new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Lang.GetItemNameValue(ItemID.AdamantiteBar), new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            }));
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            AssMessageType msgType = (AssMessageType)reader.ReadByte();
            byte playerNumber;
            byte npcNumber;
            AssPlayer aPlayer;
            PetPlayer petPlayer;
            byte changes;
            byte index;

            switch (msgType)
            {
                case AssMessageType.SyncPlayerVanity:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        playerNumber = reader.ReadByte();
                        petPlayer = Main.player[playerNumber].GetModPlayer<PetPlayer>();
                        //no "changes" packet
                        petPlayer.RecvSyncPlayerVanitySub(reader);
                    }
                    break;
                case AssMessageType.ClientChangesVanity:
                    //client and server
                    //getmodplayer error
                    playerNumber = reader.ReadByte();
                    petPlayer = Main.player[playerNumber].GetModPlayer<PetPlayer>();
                    changes = reader.ReadByte();
                    index = reader.ReadByte();
                    petPlayer.RecvClientChangesPacketSub(reader, changes, index);

                    //server transmits to others
                    if (Main.netMode == NetmodeID.Server)
                    {
                        petPlayer.SendClientChangesPacketSub(changes, index, toClient: -1, ignoreClient: playerNumber);
                    }
                    break;
                case AssMessageType.SyncAssPlayer:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        playerNumber = reader.ReadByte();
                        aPlayer = Main.player[playerNumber].GetModPlayer<AssPlayer>();
                        aPlayer.shieldDroneReduction = reader.ReadByte();
                    }
                    break;
                case AssMessageType.ClientChangesAssPlayer:
                    //client and server
                    //getmodplayer error
                    playerNumber = reader.ReadByte();
                    aPlayer = Main.player[playerNumber].GetModPlayer<AssPlayer>();
                    aPlayer.shieldDroneReduction = reader.ReadByte();
                    aPlayer.droneControllerUnlocked = (DroneType)reader.ReadByte();

                    //server transmits to others
                    if (Main.netMode == NetmodeID.Server)
                    {
                        aPlayer.SendClientChangesPacket(toClient: -1, ignoreClient: playerNumber);
                    }
                    break;
                case AssMessageType.ConvertInertSoulsInventory:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        //convert souls in local inventory
                        aPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
                        aPlayer.ConvertInertSoulsInventory();
                    }
                    break;
                case AssMessageType.GitgudLoadCounters:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        GitgudData.RecvCounters(reader);
                    }
                    break;
                case AssMessageType.GitgudChangeCounters:
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        //GitgudData.RecvReset(Main.myPlayer, reader);
                        GitgudData.RecvChangeCounter(reader);
                    }
                    break;
                case AssMessageType.ResetEmpoweringTimerpvp:
                    //client and server
                    playerNumber = reader.ReadByte();
                    aPlayer = Main.player[playerNumber].GetModPlayer<AssPlayer>();
                    aPlayer.ResetEmpoweringTimer(fromServer: true);

                    //server transmits to others
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)AssMessageType.ResetEmpoweringTimerpvp);
                        packet.Write((byte)playerNumber);
                        packet.Send(playerNumber); //send to client
                    }
                    break;
                case AssMessageType.WyvernCampfireKill:
                    npcNumber = reader.ReadByte();
                    if (npcNumber < 0 || npcNumber >= Main.maxNPCs) break;
                    NPC npc = Main.npc[npcNumber];
                    if (npc.type == NPCID.WyvernHead)
                    {
                        DungeonSoulBase.KillInstantly(npc);
                        if (npcNumber < Main.maxNPCs)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcNumber);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < Main.maxNPCs; k++)
                        {
                            NPC other = Main.npc[k];
                            if (other.active && other.type == NPCID.WyvernHead)
                            {
                                DungeonSoulBase.KillInstantly(other);
                                if (k < Main.maxNPCs)
                                {
                                    NetMessage.SendData(MessageID.SyncNPC, number: k);
                                }
                                break;
                            }
                        }
                    }
                    break;
                default:
                    Logger.Debug("Unknown Message type: " + msgType);
                    break;
            }
        }

        //Credit to jopojelly
        /// <summary>
        /// Makes alpha on .png textures actually properly rendered
        /// </summary>
        public static void PremultiplyTexture(Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }
    }

    public enum AssMessageType : byte
    {
        ClientChangesVanity,
        SyncPlayerVanity,
        ClientChangesAssPlayer,
        SyncAssPlayer,
        ConvertInertSoulsInventory,
        GitgudLoadCounters,
        GitgudChangeCounters,
        ResetEmpoweringTimerpvp,
        WyvernCampfireKill
    }

    public enum PetPlayerChanges : byte
    {
        None,
        All,
        Slots,
        PetTypes
    }
}
