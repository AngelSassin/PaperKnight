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

            return entries;
        }
    }
}
