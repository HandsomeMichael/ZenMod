using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.ModLoader;

namespace ZensTweakstest.Sounds.Custom
{
	//OverlapSound base class
	public abstract class OverlapSound : ModSound
	{
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type){
			soundInstance = sound.CreateInstance();
			return soundInstance;
		}
	}

	//Assign as OverlapSound
	public class Beam : OverlapSound {}
}
