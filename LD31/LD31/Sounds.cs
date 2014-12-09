using Cgen.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD31
{
	public static class Sounds
	{
		private static Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

		public static void Load(string name, string filename)
		{
			sounds.Add(name, new Sound(name, filename));
		}

		public static void Play(string name)
		{
			if (sounds.ContainsKey(name))
			{
				sounds[name].Play();
			}
		}
	}
}
