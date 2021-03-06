using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.Graphics.Shaders;
using Terraria.ObjectData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ZenMod
{
    // interfaces implemented, reject abstract classes return to interfaces
    public interface IPreSaveAndQuit{void PreSaveAndQuit();}
    public interface ILoadOnly{void Load();}
    public interface ILoadable {void Load();void Unload();}
    public interface ILoggable {void Log(Action<string> log);}

    public static class ZenSystemLoader
    {
        // the caches , used for Unload and other hook
        public static List<ILoadable> systems = new List<ILoadable>();

        public static void Load(Mod mod) {
            // this is required
            if (mod.Code == null)
				return;

            //intialize systems
            systems = new List<ILoadable>();
            // loop over mod.Code
            foreach (Type type in mod.Code.GetTypes().OrderBy(type => type.FullName))
			{
                // dont do anything with abstract classes
				if (type.IsAbstract){continue;}

                // Get interfaces
                var interfaces = type.GetInterfaces();

                // load ILoadable and cache it at systems
                if (interfaces.Contains(typeof(ILoadable))) {
                    var instance = (ILoadable)Activator.CreateInstance(type);
                    mod.Logger.InfoFormat($"{mod.Name} Loading System : {type.Name}");
                    instance.Load();
                    systems.Add(instance);
                    continue;
                }

                // load ILoadOnly once and dont cache it
                if (interfaces.Contains(typeof(ILoadOnly))) {
                    var instance = (ILoadOnly)Activator.CreateInstance(type);
                    mod.Logger.InfoFormat($"{mod.Name} Load Only System : {type.Name}");
                    instance.Load();
                }
                
                //type.GetConstructor(Type.EmptyTypes)
                
			}
        }
        public static void PreSaveAndQuit() {
            foreach (var item in systems){
                if (item is IPreSaveAndQuit hook) {
                    ZenMod.Log($"PreSaveQuit System : {hook.GetType().Name}");
                    hook.PreSaveAndQuit();
                }
            }
        }
        public static void Unload() {
            foreach (var item in systems){
                item.Unload();
            }
            systems = null;
        }
        public static void LogAll() {
            foreach (var item in systems){
                if (item is ILoggable hook) {
                    void CreateLog(string text) {
                        ZenMod.Log(text);
                        Main.NewText(text);
                    }
                    Action<string> log = CreateLog;
                    hook.Log(log);
                }
            }
        }
    }
}
