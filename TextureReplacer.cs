using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ZenMod.Helper;

namespace ZenMod
{
    public class TextureReplacer : ILoadable {

        public Texture2D OldMiniMap;
        public Texture2D OldTitleLogo;
        public Texture2D OldTitleLogo2;

        public void Load() {
            if (!Main.dedServ){
                // mini map
                OldMiniMap = Main.miniMapFrameTexture;
                if (SpriteSetting.ZenTexture) {
                    Main.miniMapFrameTexture = ModContent.GetTexture("ZenMod/Textures/MiniMapFrameZen");
                }
                // logo
                OldTitleLogo = Main.logoTexture;
                OldTitleLogo2 = Main.logo2Texture;
                if (SpriteSetting.Title) {
                    Main.logoTexture = Main.logo2Texture = ModContent.GetTexture("ZenMod/Textures/LogoZen");
                }
            }
        }
        public void Unload() {
            // unload, we do not need to null cached texture bc its already disposed later
            Main.logoTexture = OldTitleLogo;
            Main.logo2Texture = OldTitleLogo2;
            Main.miniMapFrameTexture = OldMiniMap;
        }
    }
}