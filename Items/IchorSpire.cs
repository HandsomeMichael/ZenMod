using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ZenMod;

namespace ZenMod.Items
{
    public class IchorSpire : ModItem
    {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Ichor Spire");
            Tooltip.SetDefault("Great you cloged the toilet without the paper!");
            ItemUseGlow.SelfGlow(this);
        }
        public override void SetDefaults(){
            item.width = 64;
            item.height = 64;
            item.shoot = ProjectileID.IchorSplash;
            item.shootSpeed = 5f;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.buyPrice(gold: 1);
            item.value = Item.sellPrice(silver: 50);
            item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTime = 36;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.damage = 70;
            item.knockBack = 1.5f;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI){
            Texture2D texture = ModContent.GetTexture(Texture+"_Glow");
            Vector2 pos = new Vector2(item.position.X - Main.screenPosition.X + item.width * 0.5f,item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f);
            spriteBatch.Draw(texture,pos,null,Color.White,rotation,texture.Size()/2f,scale,SpriteEffects.None,0f);
        }
        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimstoneBlock, 30);
            recipe.AddIngredient(ItemID.Ichor, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit){
            target.AddBuff(BuffID.Ichor, 60);
        }
    }
}
