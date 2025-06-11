using System;
using System.Text;
using Terraria.ModLoader;
using System.IO;
using ReLogic.Content;


namespace DigiBlock
{
	public class DigiBlock : Mod
	{
        public static ModKeybind OpenEvolutionGraphUIKeybind;

		public override void Load()
		{
			OpenEvolutionGraphUIKeybind = KeybindLoader.RegisterKeybind(this, "Toggle Evolution Graph", "P");
		}

		public override void Unload()
		{
			OpenEvolutionGraphUIKeybind = null;
		}
	}
}
