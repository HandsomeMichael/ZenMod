using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.Localization; 
using Terraria.Utilities;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.DataStructures;
using ZenMod.Helper;

namespace ZenMod.Items
{
	public class PrimordialThunder : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("Unleash thunder on enemies");
        }
        public override void SetDefaults() 
		{
			item.damage = 120;
			item.magic = true;
			item.knockBack = 4;
			item.noMelee = true;
			item.noUseGraphic = true;

			item.width = 26;
			item.height = 26;
			item.mana = 10;

			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = Item.sellPrice(gold: 12);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item71;
			item.autoReuse = true;
			item.useTurn = false;

			item.shoot = ModContent.ProjectileType<Primordial>();
			item.shootSpeed = 15f;
		}

		const int dye = ItemID.MirageDye;
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor,Color itemColor, Vector2 origin, float scale) {
			spriteBatch.BeginDyeShader(dye,item,true,true);return true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			spriteBatch.BeginDyeShader(dye,item,true,true);return true;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor,Color itemColor, Vector2 origin, float scale) => spriteBatch.BeginNormal(true,true);
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => spriteBatch.BeginNormal(true,true);

		private void Spawndust(Vector2 pos) {
			for (int i = 0; i < 50; i++) {
				Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
				Dust d = Dust.NewDustPerfect(pos, 182, speed * 5, Scale: 1.5f);
				d.noGravity = true;
			}
		}
		//public override void UpdateInventory(Player player) {}
		public override bool Shoot(Player player, ref Vector2 pos, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, pos) < 700f) {
					damage += 1;
					Spawndust(npc.Center);
					if (npc.realLife != -1) {
						damage /= 2;
						if (Main.rand.NextBool(10)) {damage = 0;}
					}
					if (damage > 0)Projectile.NewProjectile(npc.Center + new Vector2(0,(200*-3)+50),Vector2.Zero,type,damage,knockBack,player.whoAmI);
				}
			}
			return false;
		}
	}
	public class Primordial : ModProjectile
    {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("The Zenic Thunder");
        }
        public override void SetDefaults()
        {
			//projectile.scale = 2;
            projectile.width = 30;
            projectile.height = 200;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.light = 1;
            projectile.timeLeft = 6000;
			projectile.tileCollide = false;
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
			Player player = Main.player[projectile.owner];
			target.immune[projectile.owner] = 5;
			if (target.realLife != -1) {
				target.immune[projectile.owner] = 15;
			}
		}
		Vector2 oldpos;
        public override void AI()
        {
			int shake = 5;
			if (projectile.ai[0] == 0f) {
				//the sound
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Beam").WithPitchVariance(.5f),projectile.position);
				oldpos = projectile.position;
				projectile.ai[0] = Main.rand.NextFloat(1f,7f);
				projectile.ai[1] = 1f;
				projectile.alpha = 255;
				projectile.scale = 4;
				if (Main.rand.NextBool(5)) {
					Projectile.NewProjectile(projectile.position + new Vector2(Main.rand.Next(-shake, shake + 1), Main.rand.Next(-shake, shake + 1)),
					projectile.velocity,projectile.type,projectile.damage/2,projectile.knockBack,projectile.owner);
				}
				else {
					projectile.damage *= 2;
				}
				projectile.height *= (int)projectile.scale;
				projectile.spriteDirection = Main.rand.Next(-1,2);
			}
			shake = 3;
			//shake
			projectile.position = Vector2.Lerp(projectile.position,oldpos,0.1f);
			projectile.position += new Vector2(Main.rand.Next(-shake, shake + 1), Main.rand.Next(-shake, shake + 1));
			
			projectile.scale -= 0.005f;
			projectile.alpha -= 20;

			if (projectile.alpha < 5) {
				projectile.Kill();
			}
			projectile.ai[1] -= 0.1f;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
			var lol1 = ModContent.GetTexture(Texture);
			var lol2 = ModContent.GetTexture(Texture+"_Glow");
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == -1) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			float beh = (((float)projectile.alpha*100f)/255f)/100f;
			Color[] colors = {Color.White,Color.Red,Color.Blue,Color.Green,Color.Yellow,Color.Pink,Color.Orange};
			Color color = colors[(int)projectile.ai[0]]*beh;
			Vector2 pos = projectile.Center - Main.screenPosition;
			spriteBatch.Draw(lol1, pos, null, color, projectile.rotation, lol1.Size()/2, projectile.scale, spriteEffects, 0f);
			spriteBatch.BeginGlow(true);
			spriteBatch.Draw(lol2, pos, null, color, projectile.rotation, lol2.Size()/2, projectile.scale, spriteEffects, 0f);
			spriteBatch.BeginNormal(true);
			return false;
		}
    }
}