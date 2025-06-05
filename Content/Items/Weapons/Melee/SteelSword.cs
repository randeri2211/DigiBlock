using DigiBlock.Content.Items.Materials; // Using our Materials folder
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameContent.Items;
using DigiBlock.Content.Items.Weapons.Magic;

namespace DigiBlock.Content.Items.Weapons.Melee // Where is your code locates
{
    public class SteelSword : ModItem
    {
        private List<ModItem> weapons = new List<ModItem>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // How many items need for research in Journey Mode
        }

        public override void SetDefaults()
        {
            weapons.Add(new ExampleMagicWeapon());
            weapons.Add(new ExampleMagicWeapon());
            // Visual properties
            Item.width = 40; // Width of an item sprite
            Item.height = 40; // Height of an item sprite
            Item.scale = 1f; // Multiplicator of item size, for example is you set this to 2f our sword will be biger twice. IMPORTANT: If you are using numbers with floating point, write "f" in their end, like 1.5f, 3.14f, 2.1278495f etc.
            Item.rare = ItemRarityID.Blue; // The color of item's name in game. Check https://terraria.wiki.gg/wiki/Rarity

            // Combat properties
            Item.damage = 50; // Item damage
            Item.DamageType = DamageClass.Melee; // What type of damage item is deals, Melee, Ranged, Magic, Summon, Generic (takes bonuses from all damage multipliers), Default (doesn't take bonuses from any damage multipliers)
            // useTime and useAnimation often use the same value, but we'll see examples where they don't use the same values
            Item.useTime = 20; // How long the swing lasts in ticks (60 ticks = 1 second)
            Item.useAnimation = 20; // How long the swing animation lasts in ticks (60 ticks = 1 second)
            Item.knockBack = 6f; // How far the sword punches enemies, 20 is maximal value
            Item.autoReuse = true; // Can the item auto swing by holding the attack button
            Item.shoot = ProjectileID.BeeArrow;
            Item.shootSpeed = 16f;
            Item.noMelee = false;



            // Other properties
            Item.value = 10000; // Item sell price in copper coins
            Item.useStyle = ItemUseStyleID.Swing; // This is how you're holding the weapon, visit https://terraria.wiki.gg/wiki/Use_Style_IDs for list of possible use styles
            Item.UseSound = SoundID.Item1; // What sound is played when using the item, all sounds can be found here - https://terraria.wiki.gg/wiki/Sound_IDs
        }

        // Creating item craft
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<SteelShard>(7); // We are using custom material for the craft, 7 Steel Shards
            recipe.AddIngredient(ItemID.Wood, 3); // Also, we are using vanilla material to craft, 3 Wood
            recipe.AddTile(TileID.Anvils); // Crafting station we need for craft, WorkBenches, Anvils etc. You can find them here - https://terraria.wiki.gg/wiki/Tile_IDs
            recipe.Register();
        }

        // public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)

        // {
        // }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Console.WriteLine("weapons count:" + weapons.Count);
            for (int i = 0; i < weapons.Count; i++) //replace 3 with however many projectiles you like
            {
                Console.WriteLine("before type");
                var t = weapons[i];
                Console.WriteLine(t);
                Console.WriteLine("after type");
                // Projectile.NewProjectile(source, position, velocity, t, damage, (int)knockback, player.whoAmI); //create the projectile
            }
            return true;
        }
    }
}