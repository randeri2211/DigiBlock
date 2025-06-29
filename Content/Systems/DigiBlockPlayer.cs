using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Digimon;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text.Json; 
using Microsoft.Xna.Framework;
using DigiBlock.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;


namespace DigiBlock.Content.Systems
{
    public class DigiBlockPlayer : ModPlayer
    {
        public float digimonMaxHPPercent;
        public float digimonDefensePercent;
        public float digimonEXPPercent;
        public bool digimonAllSpecialAbilities;

        public override void ResetEffects()
        {
            digimonMaxHPPercent = 1f;
            digimonDefensePercent = 1f;
            digimonEXPPercent = 1f;
            digimonAllSpecialAbilities = false;
        }


    }
}


