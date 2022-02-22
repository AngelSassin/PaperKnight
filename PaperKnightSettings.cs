using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GlobalEnums;
using Modding;
using UnityEngine;
using UnityEngine.UI;
using Mono.Cecil.Cil;
using MonoMod;
using TMPro;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Language;

namespace PaperKnight
{
    partial class PaperKnight : IGlobalSettings<CustomGlobalSaveData>
    {
        public static CustomGlobalSaveData GlobalSaveData { get; set; } = new CustomGlobalSaveData();

        public void OnLoadGlobal(CustomGlobalSaveData s)
        {
            GlobalSaveData = s ?? GlobalSaveData ?? new CustomGlobalSaveData();
        }

        public CustomGlobalSaveData OnSaveGlobal()
        {
            return GlobalSaveData;
        }
    }

    public class CustomGlobalSaveData
    {
        public bool enemiesEnabled = true;
        public bool knightEnabled = true;
        public bool minionsEnabled = true;
        public bool npcsEnabled = true;
        public bool unlimitedSpinning = false;
    }
}
