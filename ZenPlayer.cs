using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using System.Collections.Generic;
using System;
using Terraria.GameInput;

namespace ZenMod.Items
{
    public class ZenPlayer : ModPlayer {
        public override void ModifyDrawLayers(List<PlayerLayer> layers) {
			int itemLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HeldItem"));
            if (itemLayer != -1){
                ItemUseGlow.ItemUseGlowLayer.visible = true;
                layers.Insert(itemLayer + 1, ItemUseGlow.ItemUseGlowLayer);
            }
		}
    }
}