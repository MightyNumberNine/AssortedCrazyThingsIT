using AssortedCrazyThings.Base;
using Terraria;

namespace AssortedCrazyThings.Projectiles.Pets
{
    //check this file for more info vvvvvvvv
    public class HornedSlimeProj : BabySlimeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Horned Slime");
            Main.projFrames[projectile.type] = 6;
            Main.projPet[projectile.type] = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -4;
        }

        public override void MoreSetDefaults()
        {
            //used to set dimensions (if necessary) //also use to set projectile.minion
            projectile.width = 32;
            projectile.height = 30;

            projectile.minion = false;
        }

        public override bool PreAI()
        {
            PetPlayer modPlayer = projectile.GetOwner().GetModPlayer<PetPlayer>(mod);
            if (projectile.GetOwner().dead)
            {
                modPlayer.HornedSlime = false;
            }
            if (modPlayer.HornedSlime)
            {
                projectile.timeLeft = 2;
            }
            return true;
        }
    }
}
