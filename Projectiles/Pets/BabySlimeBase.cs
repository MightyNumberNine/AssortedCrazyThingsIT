using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public abstract class BabySlimeBase : ModProjectile
    {
        public bool shootSpikes = false;
        private static readonly byte shootDelay = 60; //either +1 or +0 every tick, so effectively every 90 ticks
        public byte flyingFrameSpeed = 6;
        public byte walkingFrameSpeed = 20;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BabySlime);
            projectile.aiStyle = -1; //26
            //aiType = ProjectileID.BabySlime;
            projectile.alpha = 50;

            //set those in moresetdefaults in the projectile that inherits from this
            //projectile.width = 38;
            //projectile.height = 40;

            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;

            flyingFrameSpeed = 6;
            walkingFrameSpeed = 20;

            MoreSetDefaults();

            projectile.minionSlots = projectile.minion ? 1f : 0f;
        }

        public virtual void MoreSetDefaults()
        {
            //used to set dimensions (if necessary)
            //also used to set projectile.minion
        }

        public override bool MinionContactDamage()
        {
            return projectile.minion ? true : false;
        }

        public override void AI()
        {
            BabySlimeAI();
        }

        public void Draw()
        {
            if (projectile.ai[0] != 0)
            {
                if (projectile.velocity.X > 0.5f)
                {
                    projectile.spriteDirection = -1;
                }
                else if (projectile.velocity.X < -0.5f)
                {
                    projectile.spriteDirection = 1;
                }

                projectile.frameCounter++;
                if (projectile.frameCounter > flyingFrameSpeed)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame < 2 || projectile.frame > 5)
                {
                    projectile.frame = 2;
                }
                projectile.rotation = projectile.velocity.X * 0.1f;
            }
            else
            {
                if (projectile.direction == -1)
                {
                    projectile.spriteDirection = 1;
                }
                if (projectile.direction == 1)
                {
                    projectile.spriteDirection = -1;
                }

                if (projectile.velocity.Y >= 0f && projectile.velocity.Y <= 0.8f)
                {
                    if (projectile.velocity.X == 0f)
                    {
                        projectile.frameCounter++;
                    }
                    else
                    {
                        projectile.frameCounter += 3;
                    }
                }
                else
                {
                    projectile.frameCounter += 5;
                }
                if (projectile.frameCounter >= walkingFrameSpeed)
                {
                    projectile.frameCounter -= walkingFrameSpeed;
                    projectile.frame++;
                }
                if (projectile.frame > 1)
                {
                    projectile.frame = 0;
                }
                if (projectile.wet && Main.player[projectile.owner].position.Y + Main.player[projectile.owner].height < projectile.position.Y + projectile.height && projectile.localAI[0] == 0f)
                {
                    if (projectile.velocity.Y > -4f)
                    {
                        projectile.velocity.Y -= 0.2f;
                    }
                    if (projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y *= 0.95f;
                    }
                }
                else
                {
                    projectile.velocity.Y += 0.4f;
                }
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                }
                projectile.rotation = 0f;
            }
        }

        public byte pickedTexture = 1;

        public byte PickedTexture
        {
            get
            {
                return (byte)(pickedTexture - 1);
            }
            set
            {
                pickedTexture = (byte)(value + 1);
            }
        }

        public bool HasTexture
        {
            get
            {
                return PickedTexture != 0;
            }
        }

        public byte ShootTimer
        {
            get
            {
                return (byte)projectile.localAI[1];
            }
            set
            {
                projectile.localAI[1] = value;
            }
        }

        public void BabySlimeAI()
        {
            //projectile.damage = Damage;
            //Main.NewText(projectile.damage);

            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;

            int num = projectile.minion ? 10 : 25;
            if (!projectile.minion) projectile.minionPos = 0;
            int num2 = 40 * (projectile.minionPos + 1) * Main.player[projectile.owner].direction;
            if (Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) < projectile.position.X + (projectile.width / 2) - num + num2)
            {
                flag = true;
            }
            else if (Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) > projectile.position.X + (projectile.width / 2) + num + num2)
            {
                flag2 = true;
            }

            if (projectile.ai[1] == 0f)
            {
                int num38 = 500;
                num38 += 40 * projectile.minionPos;
                if (projectile.localAI[0] > 0f)
                {
                    num38 += 600;
                }

                if (Main.player[projectile.owner].rocketDelay2 > 0)
                {
                    projectile.ai[0] = 1f;
                }

                Vector2 vector6 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                float num39 = Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) - vector6.X;
                float num40 = Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2) - vector6.Y;
                float num41 = (float)Math.Sqrt((num39 * num39 + num40 * num40));
                if (num41 > 2000f)
                {
                    projectile.position.X = Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) - (projectile.width / 2);
                    projectile.position.Y = Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2) - (projectile.height / 2);
                }
                else if (num41 > num38 || (Math.Abs(num40) > 300f && (projectile.localAI[0] <= 0f)))
                {
                    if (num40 > 0f && projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    if (num40 < 0f && projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    projectile.ai[0] = 1f;
                }
            }

            if (projectile.ai[0] != 0f)
            {
                float veloDelta = 0.2f;
                int num43 = 200;

                projectile.tileCollide = false;
                float desiredVeloX = Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) - projectile.Center.X;

                desiredVeloX -= 40 * Main.player[projectile.owner].direction;
                float num45 = 700f;

                bool flag6 = false;
                int num46 = -1;

                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].CanBeChasedBy(this))
                    {
                        float num48 = Main.npc[k].position.X + (Main.npc[k].width / 2);
                        float num49 = Main.npc[k].position.Y + (Main.npc[k].height / 2);
                        float num50 = Math.Abs(Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) - num48) + Math.Abs(Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2) - num49);
                        if (num50 < num45)
                        {
                            if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
                            {
                                num46 = k;
                            }
                            flag6 = true;
                            break;
                        }
                    }
                }

                if (!flag6)
                {
                    desiredVeloX -= 40 * projectile.minionPos * Main.player[projectile.owner].direction;
                }
                if (flag6 && num46 >= 0)
                {
                    projectile.ai[0] = 0f;
                }

                float desiredVeloY = Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2) - projectile.Center.Y;

                float num52 = (float)Math.Sqrt((double)(desiredVeloX * desiredVeloX + desiredVeloY * desiredVeloY));
                float num53 = 10f;
                //float num54 = num52;

                if (num52 < num43 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + projectile.height <= Main.player[projectile.owner].position.Y + Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    if (projectile.velocity.Y < -6f)
                    {
                        projectile.velocity.Y = -6f;
                    }
                }
                if (num52 < 60f)
                {
                    desiredVeloX = projectile.velocity.X;
                    desiredVeloY = projectile.velocity.Y;
                }
                else
                {
                    num52 = num53 / num52;
                    desiredVeloX *= num52;
                    desiredVeloY *= num52;
                }

                if (projectile.velocity.X < desiredVeloX)
                {
                    projectile.velocity.X += veloDelta;
                    if (projectile.velocity.X < 0f)
                    {
                        projectile.velocity.X += veloDelta * 1.5f;
                    }
                }
                if (projectile.velocity.X > desiredVeloX)
                {
                    projectile.velocity.X -= veloDelta;
                    if (projectile.velocity.X > 0f)
                    {
                        projectile.velocity.X -= veloDelta * 1.5f;
                    }
                }
                if (projectile.velocity.Y < desiredVeloY)
                {
                    projectile.velocity.Y += veloDelta;
                    if (projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y += veloDelta * 1.5f;
                    }
                }
                if (projectile.velocity.Y > desiredVeloY)
                {
                    projectile.velocity.Y -= veloDelta;
                    if (projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y -= veloDelta * 1.5f;
                    }
                }
            }
            else //projectile.ai[0] == 0f
            {
                Vector2 toTarget = Vector2.Zero;

                float num87 = 40 * projectile.minionPos;
                int num88 = 60;
                projectile.localAI[0] -= 1f;
                if (projectile.localAI[0] < 0f)
                {
                    projectile.localAI[0] = 0f;
                }
                if (projectile.ai[1] > 0f)
                {
                    projectile.ai[1] -= 1f;
                }
                else
                {
                    float targetX = projectile.position.X;
                    float targetY = projectile.position.Y;
                    float distance = 100000f;
                    float num92 = distance;
                    int targetNPC = -1;

                    //------------------------------------------------------------------------------------
                    //DISABLE MINION TARGETING------------------------------------------------------------
                    //------------------------------------------------------------------------------------

                    if (projectile.minion)
                    {
                        NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
                        if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(this))
                        {
                            float x = ownerMinionAttackTargetNPC2.Center.X;
                            float y = ownerMinionAttackTargetNPC2.Center.Y;
                            float num94 = Math.Abs(projectile.position.X + (projectile.width / 2) - x) + Math.Abs(projectile.position.Y + (projectile.height / 2) - y);
                            if (num94 < distance)
                            {
                                if (targetNPC == -1 && num94 <= num92)
                                {
                                    num92 = num94;
                                    targetX = x;
                                    targetY = y;
                                }
                                if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
                                {
                                    distance = num94;
                                    targetX = x;
                                    targetY = y;
                                    targetNPC = ownerMinionAttackTargetNPC2.whoAmI;
                                }
                            }
                        }
                        if (targetNPC == -1)
                        {
                            for (int k = 0; k < 200; k++)
                            {
                                if (Main.npc[k].CanBeChasedBy(this))
                                {
                                    float num96 = Main.npc[k].Center.X;
                                    float num97 = Main.npc[k].Center.Y;
                                    float between = Math.Abs(projectile.Center.X - num96) + Math.Abs(projectile.Center.Y - num97);
                                    if (between < distance)
                                    {
                                        if (targetNPC == -1 && between <= num92)
                                        {
                                            num92 = between;
                                            targetX = num96;
                                            targetY = num97;
                                        }
                                        if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
                                        {
                                            distance = between;
                                            targetX = num96;
                                            targetY = num97;
                                            targetNPC = k;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (targetNPC == -1 && num92 < distance)
                    {
                        distance = num92;
                    }
                    else if (targetNPC >= 0) //has target
                    {
                        toTarget = new Vector2(targetX, targetY) - projectile.Center;
                    }

                    float num104 = 300f;
                    if ((double)projectile.position.Y > Main.worldSurface * 16.0)
                    {
                        num104 = 150f;
                    }

                    if (distance < num104 + num87 && targetNPC == -1)
                    {
                        float num105 = targetX - (projectile.position.X + (projectile.width / 2));
                        if (num105 < -5f)
                        {
                            flag = true;
                            flag2 = false;
                        }
                        else if (num105 > 5f)
                        {
                            flag2 = true;
                            flag = false;
                        }
                    }

                    //bool flag9 = false;

                    if (targetNPC >= 0 && distance < 800f + num87)
                    {
                        projectile.friendly = true;
                        projectile.localAI[0] = num88;
                        float num106 = targetX - (projectile.position.X + (projectile.width / 2));
                        if (num106 < -10f)
                        {
                            flag = true;
                            flag2 = false;
                        }
                        else if (num106 > 10f)
                        {
                            flag2 = true;
                            flag = false;
                        }
                        if (targetY < projectile.Center.Y - 100f && num106 > -50f && num106 < 50f && projectile.velocity.Y == 0f)
                        {
                            float num107 = Math.Abs(targetY - projectile.Center.Y);
                            //jumping velocities
                            if (num107 < 100f) //120f
                            {
                                projectile.velocity.Y = -10f;
                            }
                            else if (num107 < 210f)
                            {
                                projectile.velocity.Y = -13f;
                            }
                            else if (num107 < 270f)
                            {
                                projectile.velocity.Y = -15f;
                            }
                            else if (num107 < 310f)
                            {
                                projectile.velocity.Y = -17f;
                            }
                            else if (num107 < 380f)
                            {
                                projectile.velocity.Y = -18f;
                            }
                        }

                        //---------------------------------------------------------------------
                        //drop through platforms
                        //only drop if targets center y is lower than the minions bottom y, and only if its less than 300 away from the target horizontally
                        if (Main.npc[targetNPC].Center.Y > projectile.Bottom.Y && Math.Abs(toTarget.X) < 300)
                        {
                            int tilex = (int)(projectile.position.X / 16f);
                            int tiley = (int)((projectile.position.Y + projectile.height + 15f) / 16f);

                            if (TileID.Sets.Platforms[Framing.GetTileSafely(tilex, tiley).type] &&
                                TileID.Sets.Platforms[Framing.GetTileSafely(tilex + 1, tiley).type] &&
                                ((projectile.direction == -1) ? TileID.Sets.Platforms[Framing.GetTileSafely(tilex + 2, tiley).type] : true))
                            {
                                //AssUtils.Print("drop " + Main.time);
                                //Main.NewText("drop");
                                projectile.netUpdate = true;
                                projectile.position.Y += 1f;
                            }
                        }

                        if (shootSpikes)
                        {
                            //PickedTexture * 3 makes it so theres a small offset for minion shooting based on their texture, which means that if you have different slimes out,
                            //they don't all shoot in sync
                            if (ShootTimer > (shootDelay + PickedTexture * 3) && distance < 200f &&
                                Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[targetNPC].position, Main.npc[targetNPC].width, Main.npc[targetNPC].height))
                            {
                                if (Main.netMode != 1)
                                {
                                    for (int k = 0; k < 3; k++)
                                    {
                                        Vector2 vector6 = new Vector2(k - 1, -2f);
                                        vector6.X *= 1f + Main.rand.Next(-40, 41) * 0.02f;
                                        vector6.Y *= 1f + Main.rand.Next(-40, 41) * 0.02f;
                                        vector6.Normalize();
                                        vector6 *= 3f + Main.rand.Next(-40, 41) * 0.01f;
                                        Projectile.NewProjectile(projectile.Center.X, projectile.Bottom.Y - 8f, vector6.X, vector6.Y, mod.ProjectileType<SlimePackMinionSpike>(), projectile.damage / 2, 0f, Main.myPlayer, ai1: PickedTexture);
                                        ShootTimer = (byte)(PickedTexture * 3);
                                    }
                                }
                            }
                            if (ShootTimer <= shootDelay + PickedTexture * 3) ShootTimer = (byte)(ShootTimer + Main.rand.Next(2));
                        }
                    }
                    else
                    {
                        projectile.friendly = false;
                    }
                }

                if (projectile.ai[1] != 0f)
                {
                    flag = false;
                    flag2 = false;
                }
                else if (projectile.localAI[0] == 0f)
                {
                    projectile.direction = Main.player[projectile.owner].direction;
                }

                projectile.tileCollide = true;

                float num110 = 0.2f;
                float num111 = 6f;

                if (num111 < Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y))
                {
                    num110 = 0.3f;
                    num111 = Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y);
                }

                if (flag)
                {
                    if (projectile.velocity.X > -3.5f)
                    {
                        projectile.velocity.X -= num110;
                    }
                    else
                    {
                        projectile.velocity.X -= num110 * 0.25f;
                    }
                }
                else if (flag2)
                {
                    if (projectile.velocity.X < 3.5f)
                    {
                        projectile.velocity.X += num110;
                    }
                    else
                    {
                        projectile.velocity.X += num110 * 0.25f;
                    }
                }
                else
                {
                    projectile.velocity.X *= 0.9f;
                    if (projectile.velocity.X >= 0f - num110 && projectile.velocity.X <= num110)
                    {
                        projectile.velocity.X = 0f;
                    }
                }

                if (flag | flag2)
                {
                    int num112 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                    int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
                    if (flag)
                    {
                        num112--;
                    }
                    if (flag2)
                    {
                        num112++;
                    }
                    num112 += (int)projectile.velocity.X;
                    if (WorldGen.SolidTile(num112, j))
                    {
                        flag4 = true;
                    }
                }
                if (Main.player[projectile.owner].position.Y + Main.player[projectile.owner].height - 8f > projectile.position.Y + projectile.height)
                {
                    flag3 = true;
                }
                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
                if (projectile.velocity.Y == 0f)
                {
                    if (!flag3 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
                    {
                        int num113 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                        int j2 = (int)(projectile.position.Y + (projectile.height / 2)) / 16 + 1;
                        if (flag)
                        {
                            int num30 = num113;
                            num113 = num30 - 1;
                        }
                        if (flag2)
                        {
                            int num30 = num113;
                            num113 = num30 + 1;
                        }
                        WorldGen.SolidTile(num113, j2);
                    }
                    if (flag4)
                    {
                        int x = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                        int y = (int)(projectile.position.Y + projectile.height) / 16 + 1;
                        if (WorldGen.SolidTile(x, y) || Main.tile[x, y].halfBrick() || Main.tile[x, y].slope() > 0)
                        {
                            try
                            {
                                x = (int)(projectile.position.X + (projectile.width / 2)) / 16;
                                y = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
                                if (flag)
                                {
                                    x--;
                                }
                                if (flag2)
                                {
                                    x++;
                                }
                                x += (int)projectile.velocity.X;
                                if (!WorldGen.SolidTile(x, y - 1) && !WorldGen.SolidTile(x, y - 2))
                                {
                                    projectile.velocity.Y = -5.1f;
                                }
                                else if (!WorldGen.SolidTile(x, y - 2))
                                {
                                    projectile.velocity.Y = -7.1f;
                                }
                                else if (WorldGen.SolidTile(x, y - 5))
                                {
                                    projectile.velocity.Y = -11.1f;
                                }
                                else if (WorldGen.SolidTile(x, y - 4))
                                {
                                    projectile.velocity.Y = -10.1f;
                                }
                                else
                                {
                                    projectile.velocity.Y = -9.1f;
                                }
                            }
                            catch
                            {
                                projectile.velocity.Y = -9.1f;
                            }
                        }
                    }
                    else if (flag | flag2)
                    {
                        projectile.velocity.Y -= 6f;
                    }
                }
                if (projectile.velocity.X > num111)
                {
                    projectile.velocity.X = num111;
                }
                if (projectile.velocity.X < 0f - num111)
                {
                    projectile.velocity.X = 0f - num111;
                }
                if (projectile.velocity.X < 0f)
                {
                    projectile.direction = -1;
                }
                if (projectile.velocity.X > 0f)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X > num110 && flag2)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X < 0f - num110 && flag)
                {
                    projectile.direction = -1;
                }
            }

            Draw();
        }
    }
}
