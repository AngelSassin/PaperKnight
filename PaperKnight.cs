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
    public partial class PaperKnight : Mod
    {
        public const string Version = "0.1.1.0";
        public override string GetVersion() => PaperKnight.Version;
        internal static PaperKnight Instance;

        internal List<GameObject> enemies = new List<GameObject>();
        internal AssetBundle ab = null; 

        public override void Initialize()
        {
            LoadBundle();
            Instance = this;

            ModHooks.TakeHealthHook += KnightDamaged;
            ModHooks.OnEnableEnemyHook += EnemyEnabled;
            ModHooks.SceneChanged += OnSceneChange;

            On.HeroController.Start += HeroStart;
            On.HealthManager.TakeDamage += OnTakeDamage;
        }

        private void LoadBundle()
        {
            string bundleN = "resizeshader";
            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (string res in asm.GetManifestResourceNames())
            {
                using (Stream s = asm.GetManifestResourceStream(res))
                {
                    if (s == null) continue;
                    string bundleName = Path.GetExtension(res).Substring(1);
                    if (bundleName != bundleN) continue;
                    Log("Loading bundle " + bundleName);
                    // Allows us to directly load from stream.
                    ab = AssetBundle.LoadFromStream(s); // Store this somewhere you can access again.
                }
            }
        }




        private void OnSceneChange(string obj)
        {
            enemies.Clear();

            // Update Knight's FlipController
            if (GlobalSaveData.knightEnabled && !HeroController.instance.gameObject.GetComponent<FlipController>())
                HeroController.instance.gameObject.AddComponent<FlipController>();
            else if (GlobalSaveData.knightEnabled && !HeroController.instance.gameObject.GetComponent<FlipController>().enabled)
                HeroController.instance.gameObject.GetComponent<FlipController>().enabled = true;
            else if (!GlobalSaveData.knightEnabled && HeroController.instance.gameObject.GetComponent<FlipController>())
                HeroController.instance.gameObject.GetComponent<FlipController>().enabled = false;
        }

        


        private int KnightDamaged(int damage)
        {
            if (GlobalSaveData.knightEnabled)
            {
                GameObject hero = HeroController.instance.gameObject;
                if (!hero.GetComponent<FlipController>())
                    hero.AddComponent<FlipController>();
                hero.GetComponent<FlipController>().TriggerHit();
            }

            return damage;
        }

        private void OnTakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            if (GlobalSaveData.enemiesEnabled)
            {
                GameObject obj = self.gameObject;
                if (obj)
                    if (hitInstance.AttackType.Equals(AttackTypes.Nail) || hitInstance.AttackType.Equals(AttackTypes.NailBeam))
                        obj.GetComponent<FlipController>().TriggerHit();
            }

            orig(self, hitInstance);
        }




        private void HeroStart(On.HeroController.orig_Start orig, HeroController self)
        {
            if (GlobalSaveData.knightEnabled)
            {
                GameObject hero = self.gameObject;
                if (!hero.GetComponent<FlipController>())
                    hero.AddComponent<FlipController>();
                hero.GetComponent<FlipController>().direction = hero.transform.localScale.x > 0 ? 1 : -1;
                hero.GetComponent<FlipController>().spinGoal = hero.transform.localScale.x > 0 ? 1 : -1;

                /*
                if (!hero.GetComponent<tk2dSpriteAnimator>())
                    hero.AddComponent<tk2dSpriteAnimator>();
                Shader resizeShader = GameManager.Instantiate(ab.LoadAsset<Shader>("resizeshader"));
                Shader s = Shader.Find("Sprites/Default");
                Material m = new Material(s);
                foreach (tk2dSpriteDefinition sprite in hero.GetComponent<tk2dSpriteAnimator>().Sprite.Collection.spriteDefinitions)
                    sprite.material = m;
                hero.GetComponent<FlipController>().mat = m;
                */
            }

            orig(self);
        }

        public bool EnemyEnabled(GameObject enemy, bool isDead)
        {
            if (isDead)
                return isDead;

            if (PaperKnight.GlobalSaveData.enemiesEnabled)
                enemy.AddComponent<FlipController>();
            enemies.Add(enemy);
            enemy.GetComponent<FlipController>().direction = enemy.transform.localScale.x > 0 ? 1 : -1;
            enemy.GetComponent<FlipController>().spinGoal = enemy.transform.localScale.x > 0 ? 1 : -1;

            /*
            if (!enemy.GetComponent<tk2dSpriteAnimator>())
                enemy.AddComponent<tk2dSpriteAnimator>();
            //Shader resizeShader = ab.LoadAsset<Shader>("resizeshader");
            Shader s = Shader.Find("Sprites/Default");
            Material m = new Material(s);
            foreach (tk2dSpriteDefinition sprite in enemy.GetComponent<tk2dSpriteAnimator>().Sprite.Collection.spriteDefinitions)
                sprite.material = m;
            enemy.GetComponent<FlipController>().mat = m;
            */

            return false;
        }
    }
}