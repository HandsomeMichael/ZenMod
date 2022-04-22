using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;

namespace ZenMod
{
    [Label("Sprite Setting")]
    [BackgroundColor(162, 59, 80, 192)]
    public class SpriteSetting : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static SpriteSetting get => ModContent.GetInstance<SpriteSetting>();

        [Label("Zen Textures")]
        [Tooltip("Change some textures into a new one")]
        [BackgroundColor(208, 84, 92, 192)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool ZenTexture;

        [Label("Title Revamp")]
        [Tooltip("Changes the TML title.")]
        [BackgroundColor(208, 84, 92, 192)]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool TitleChange;
    }
}
