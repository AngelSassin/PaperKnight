using System;
using System.Collections.Generic;
using Modding;
using UnityEngine;

namespace PaperKnight
{
    public partial class PaperKnight : IMenuMod
    {
        public bool ToggleButtonInsideMenu => false;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            List<IMenuMod.MenuEntry> entries = new List<IMenuMod.MenuEntry>();

            entries.Add(new IMenuMod.MenuEntry
            {
                Name = "Unlimited Spinning!",
                Description = "There is no speed cap to how fast enemies can spin when hit repeatedly.",
                Values = new string[] { "On", "Off" },
                Saver = (i) => GlobalSaveData.unlimitedSpinning = i == 0,
                Loader = () => GlobalSaveData.unlimitedSpinning ? 0 : 1
            });

            /*
             
            entries.Add(new IMenuMod.MenuEntry
            {
                Name = "Toggle Paper Knight:",
                Description = "Allow the knight to spin like paper. This will update on room change.",
                Values = new string[] { "On", "Off" },
                Saver = (i) => GlobalSaveData.knightEnabled = i == 0,
                Loader = () => GlobalSaveData.knightEnabled ? 0 : 1
            });

            entries.Add(new IMenuMod.MenuEntry
            {
                Name = "Toggle Paper Enemies:",
                Description = "Allow enemies to spin like paper. This will update on room change.",
                Values = new string[] { "On", "Off" },
                Saver = (i) => GlobalSaveData.enemiesEnabled = i == 0,
                Loader = () => GlobalSaveData.enemiesEnabled ? 0 : 1
            });

            entries.Add(new IMenuMod.MenuEntry
            {
                Name = "Toggle Paper Minions:",
                Description = "Allow charm minions to spin like paper. This will update on room change.",
                Values = new string[] { "On", "Off" },
                Saver = (i) => GlobalSaveData.minionsEnabled = i == 0,
                Loader = () => GlobalSaveData.minionsEnabled ? 0 : 1
            });

      
            entries.Add(new IMenuMod.MenuEntry
            {
                Name = "Toggle Paper NPCs:",
                Description = "Allow NPCs to spin like paper. This will update on room change.",
                Values = new string[] { "On", "Off" },
                Saver = (i) => GlobalSaveData.npcsEnabled = i == 0,
                Loader = () => GlobalSaveData.npcsEnabled ? 0 : 1
            });
            
            */

            

            return entries;
        }
    }
}
