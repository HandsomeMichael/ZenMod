using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ZenMod.Helper;

namespace ZenMod
{
    // this will be used for custom glow mechanic
    public interface ICustomglow{
        void UseGlow(ref Vector2 pos, ref Rectangle? rec, ref Color color,ref float rotation,ref Vector2 orig,ref float scale);
    }
    // the class that handle use glow
    public class ItemUseGlow : ILoadable
    {
        // the data type that will be stored as
        public class ItemGlow
        {
            public Vector2 offset;
            public int type;
            public string texture;
            public ItemGlow(int type, string texture,Vector2 offset) {
                this.type = type;
                this.texture = texture;
                this.offset = offset;
            }
        }

        // the list of glow data
        public static List<ItemGlow> useGlow;

        // get data from an item type
        public static ItemGlow Get(int type) {
            foreach (var item in useGlow){
                if (item.type == type) {
                    return item;
                }
            }
            return null;
        }

        public void Load() => useGlow = new List<ItemGlow>();
        public void Unload() => useGlow = null;

        /// <summary>
		/// make item itself glow when used
		/// </summary>
        public static void SelfGlow(ModItem item,int x = 0, int y = 0,string glow = "_Glow") {
            if (!Main.dedServ){
                useGlow.Add(new ItemGlow(item.item.type,item.Texture+glow,new Vector2(x,y)));
            }
        }

        /// <summary>
		/// make item itself glow when used
		/// </summary>
        public static void SelfGlow(ModItem item,string glow) => SelfGlow(item,0,0,glow);

        /// <summary>
		/// epic stolen code moment
		/// </summary>
        public static readonly PlayerLayer ItemUseGlowLayer = new PlayerLayer("ZenMod(PortedQwerty)", "ItemUseGlow", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
        {
            //prevent shadow draw it
            if (drawInfo.shadow != 0f) {
				return;
			}
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ZenMod.get;
            // prevent draw for empty item and only when item is used
            if (!drawPlayer.HeldItem.IsAir && drawPlayer.itemAnimation > 0)
            {
                // get useglow data, and if it doesnt have one then dont get it
                Item item = drawPlayer.HeldItem;
                var b = Get(item.type);
                if (b != null)
                {
                    Texture2D texture = ModContent.GetTexture(b.texture);
                    float glowOffsetX = b.offset.X;
                    float glowOffsetY = b.offset.Y;
                    Vector2 location = drawInfo.itemLocation;
                    // Draw glow for style 5 ( gun / staff )
                    if (item.useStyle == 5)
                    {
                        // Draw staff glow
                        if (Item.staff[item.type])
                        {
                            float rotation = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
                            int width = 0;
                            Vector2 origin = new Vector2(0f, (float)Main.itemTexture[item.type].Height);

                            if (drawPlayer.gravDir == -1f)
                            {
                                if (drawPlayer.direction == -1)
                                {
                                    rotation += 1.57f;
                                    origin = new Vector2((float)Main.itemTexture[item.type].Width, 0f);
                                    width -= Main.itemTexture[item.type].Width;
                                }
                                else
                                {
                                    rotation -= 1.57f;
                                    origin = Vector2.Zero;
                                }
                            }
                            else if (drawPlayer.direction == -1)
                            {
                                origin = new Vector2((float)Main.itemTexture[item.type].Width, (float)Main.itemTexture[item.type].Height);
                                width -= Main.itemTexture[item.type].Width;
                            }
                            Vector2 pos = new Vector2((float)((int)(location.X - Main.screenPosition.X + origin.X + (float)width)), (float)((int)(location.Y - Main.screenPosition.Y)));
                            Rectangle? rec = new Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height);
                            Color color = Color.White;
                            float scale = item.scale;
                            if (item.modItem != null && item.modItem is ICustomglow glow) {
                                glow.UseGlow(ref pos,ref rec,ref color,ref rotation,ref origin, ref scale);
                            }
                            DrawData value = new DrawData(texture,
                            pos, rec, color, rotation, origin, scale, drawInfo.spriteEffects, 0);
                            Main.playerDrawData.Add(value);
                        }
                        // Draw gun
                        else
                        {
                            Vector2 vector10 = new Vector2((float)(Main.itemTexture[item.type].Width / 2), (float)(Main.itemTexture[item.type].Height / 2));

                            //Vector2 offset = this.DrawPlayerItemPos(drawPlayer.gravDir, item.type);
                            Vector2 offset = new Vector2(10, texture.Height / 2);
                            if (glowOffsetX != 0)
                            {
                                offset.X = glowOffsetX;
                            }

                            offset.Y += glowOffsetY * drawPlayer.gravDir;
                            int num107 = (int)offset.X;
                            vector10.Y = offset.Y;
                            Vector2 origin5 = new Vector2((float)(-(float)num107), (float)(Main.itemTexture[item.type].Height / 2));
                            if (drawPlayer.direction == -1)
                            {
                                origin5 = new Vector2((float)(Main.itemTexture[item.type].Width + num107), (float)(Main.itemTexture[item.type].Height / 2));
                            }

                            //value = new DrawData(Main.itemTexture[item.type], new Vector2((float)((int)(value2.X - Main.screenPosition.X + vector10.X)), (float)((int)(value2.Y - Main.screenPosition.Y + vector10.Y))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height)), item.GetAlpha(color37), drawPlayer.itemRotation, origin5, item.scale, effect, 0);
                            //Main.playerDrawData.Add(value);
                            Vector2 pos = new Vector2((float)((int)(location.X - Main.screenPosition.X + vector10.X)), (float)((int)(location.Y - Main.screenPosition.Y + vector10.Y)));
                            Rectangle? rec = new Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height);
                            Color color = Color.White;
                            float rotation = drawPlayer.itemRotation;
                            float scale = item.scale;

                            if (item.modItem != null && item.modItem is ICustomglow glow) {
                                glow.UseGlow(ref pos,ref rec,ref color,ref rotation,ref origin5, ref scale);
                            }
                            DrawData value = new DrawData(texture, pos, 
                            rec, 
                            color, rotation, origin5, scale, drawInfo.spriteEffects, 0);
                            Main.playerDrawData.Add(value);
                        }
                    }
                    //Draw glow for melee, eat and hold
                    else
                    {
                        Vector2 pos = new Vector2((float)((int)(location.X - Main.screenPosition.X)),
                            (float)((int)(location.Y - Main.screenPosition.Y)));
                        Rectangle? rec = new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height));
                        Color color = Color.White;
                        float rotation = drawPlayer.itemRotation;
                        Vector2 orig = new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height);
                        float scale = item.scale;
                        if (item.modItem != null && item.modItem is ICustomglow glow) {
                            glow.UseGlow(ref pos,ref rec,ref color,ref rotation,ref orig, ref scale);
                        }
                        DrawData value = new DrawData(texture,
                            pos,
                            rec,
                            color,
                            rotation,
                            orig,
                            scale,
                            drawInfo.spriteEffects,
                            0);

                        Main.playerDrawData.Add(value);
                    }
                }
            }
        });
    }
}