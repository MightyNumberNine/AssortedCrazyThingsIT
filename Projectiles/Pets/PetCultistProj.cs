using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Projectiles.Pets
{
    public class PetCultistProj : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/Projectiles/Pets/PetCultistProj_0"; //temp
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tiny Cultist");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            projectile.aiStyle = -1;
            projectile.width = 20;
            projectile.height = 26;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer modPlayer = player.GetModPlayer<PetPlayer>(mod);
            if (player.dead)
            {
                modPlayer.PetCultist = false;
            }
            if (modPlayer.PetCultist)
            {
                projectile.timeLeft = 2;
            }
            AssAI.FlickerwickPetAI(projectile, lightPet: false, lightDust: false, reverseSide: true, vanityPet: true, veloXToRotationFactor: 0.5f, offsetX: 16f, offsetY: (player.statLife < player.statLifeMax2 / 2)? -26f: 2f);
            AssAI.FlickerwickPetDraw(projectile, 8, 8);
        }
    }
}
