
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ZenMod
{
	public class ZenMod : Mod
	{
		public static ZenMod get => ModContent.GetInstance<ZenMod>();
		public static void Log(string text) => get.Logger.InfoFormat(text);
		
		public override void Load() {
			ZenSystemLoader.Load(this);
		}
		public override void Unload() {
			ZenSystemLoader.Unload();
		}
	}
}