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
using HutongGames.Utility;
using HutongGames.Extensions;
using Satchel;
using Language;

namespace PaperKnight
{
    public partial class PaperKnight : Mod
    {
        public const string Version = "0.4.0.2";
        public override string GetVersion() => PaperKnight.Version;
        internal static PaperKnight Instance;
        private FlippableList flippables = new FlippableList();
        private bool flipControllerMutex = false;
        private List<FlipController> betweenSceneFCs = new List<FlipController>();

        public override void Initialize()
        {
            Instance = this;

            ModHooks.TakeHealthHook += KnightDamaged;
            ModHooks.OnEnableEnemyHook += EnemyEnabled;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnActiveSceneChange;

            On.HeroController.Start += HeroStart;
            On.HealthManager.Die += OnDie;
            On.HealthManager.TakeDamage += OnTakeDamage;

            On.KnightHatchling.Awake += OnHatchlingAwake;
            On.PlayMakerFSM.Awake += OnFSMAwake;
            On.PlayMakerFSM.Update += OnFSMUpdate;
            On.HeroController.StartCyclone  += OnCyclone;
            On.HeroController.EndCyclone += EndCyclone;
            On.SpellFluke.DoDamage += OnFlukeDamage;

            On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter += OnIntCompare;
            On.HutongGames.PlayMaker.Actions.IntOperator.OnEnter += OnIntOperator;
            On.HutongGames.PlayMaker.Actions.Collision2dEvent.DoCollisionEnter2D += OnCollision2d;
            On.EnemyDreamnailReaction.RecieveDreamImpact += OnDreamNail;
            
        }

        private void OnFSMUpdate(On.PlayMakerFSM.orig_Update orig, PlayMakerFSM self)
        {
            List<string> preventFlips = FlippableList.preventFlipList;
            List<string> enableFlips = FlippableList.enableFlipList;
            FlipController flip = self.Fsm.Owner.gameObject.GetComponent<FlipController>();
            if (!flip)
            {
                orig(self);
                return;
            }

            if (!flip.preventFlip)
            {
                foreach (string state in preventFlips)
                {
                    string[] stateVars = state.Split('/');

                    if (CleanName(stateVars[0]) != CleanName(self.Fsm.Owner.gameObject.name))
                        continue;
                    if (CleanName(stateVars[1]) != CleanName(self.ActiveStateName))
                        continue;

                    flip.preventFlip = true;
                    break;
                }
            } 
            else
            {
                foreach (string state in enableFlips)
                {
                    string[] stateVars = state.Split('/');

                    if (CleanName(stateVars[0]) != CleanName(self.Fsm.Owner.gameObject.name))
                        continue;
                    if (CleanName(stateVars[1]) != CleanName(self.ActiveStateName))
                        continue;

                    flip.preventFlip = false;
                    break;
                }
            }

            orig(self);
        }

        private void OnFlukeDamage(On.SpellFluke.orig_DoDamage orig, SpellFluke self, GameObject obj, int recursion, bool burst)
        {
            FlipController flip = obj.GetComponent<FlipController>();
            if (flip)
                flip.TriggerHit();

            orig(self, obj, recursion, burst);
        }

        private void OnCollision2d(On.HutongGames.PlayMaker.Actions.Collision2dEvent.orig_DoCollisionEnter2D orig, Collision2dEvent self, Collision2D collisionInfo)
        {
            if (self.storeCollider != null && self.storeCollider.Value != null && self.storeCollider.Value.name.Contains("Grimmball"))
            {
                if (collisionInfo.collider && collisionInfo.collider.gameObject)
                {
                    FlipController flip = collisionInfo.collider.gameObject.GetComponent<FlipController>();
                    if (flip)
                        flip.TriggerHit();
                }
            }

            orig(self, collisionInfo);
        }

        private void OnDreamNail(On.EnemyDreamnailReaction.orig_RecieveDreamImpact orig, EnemyDreamnailReaction self)
        {
            FlipController flip = self.gameObject.GetComponent<FlipController>();
            if (flip)
                flip.TriggerHit();

            orig(self);
        }

        private void OnCyclone(On.HeroController.orig_StartCyclone orig, HeroController self)
        {
            FlipController flip = HeroController.instance.GetComponent<FlipController>();
            if (flip)
                flip.TriggerConstant(2.5F);
            orig(self);
        }

        private void EndCyclone(On.HeroController.orig_EndCyclone orig, HeroController self)
        {
            FlipController flip = HeroController.instance.GetComponent<FlipController>();
            if (flip)
                flip.ResetScale();
            orig(self);
        }





        private void OnFSMAwake(On.PlayMakerFSM.orig_Awake orig, PlayMakerFSM self)
        {
            orig(self);

            if (flipControllerMutex)
                return;
            flipControllerMutex = true;

            foreach (string flipName in FlippableList.objectFlipList)
            {
                string goName = CleanName(self.gameObject.name);

                if (goName.Equals(CleanName(flipName)))
                {
                    CreateSpriteObject(self.gameObject, false);
                    break;
                }
            }

            flipControllerMutex = false;
        }

        private void OnHatchlingAwake(On.KnightHatchling.orig_Awake orig, KnightHatchling self)
        {
            orig(self);
            if (flipControllerMutex) 
                return;
            flipControllerMutex = true;
            CreateSpriteObject(self.gameObject, false);
            flipControllerMutex = false;
        }

        private void OnIntOperator(On.HutongGames.PlayMaker.Actions.IntOperator.orig_OnEnter orig, IntOperator self)
        {
            if (self.Fsm.GameObject.name.Equals("Enemy Damager") && self.Fsm.Name.Equals("Attack"))
            {
                FlipController flip = self.Fsm.GetFsmGameObject("Enemy").Value.GetComponent<FlipController>();
                if (flip)
                    flip.TriggerHit();
            }

            orig(self);
        }

        private void OnIntCompare(On.HutongGames.PlayMaker.Actions.IntCompare.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntCompare self)
        {
            if (self.Fsm.Name.Contains("Hatchling Spawn") && self.integer2.Value == 4)
                self.integer2.Value = 8;
            orig(self);
        }

        private void HeroStart(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);
            CreateSpriteObject(self.gameObject, false);
        }

        public bool EnemyEnabled(GameObject enemy, bool isDead)
        {
            if (isDead)
                return isDead;

            if (enemy.name.Equals("Head") && enemy.transform.parent.gameObject.name.Contains("False Knight"))
                return false;

            CreateSpriteObject(enemy, true);
            return false;
        }

        private void OnActiveSceneChange(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Menu_Title")
                return;

            foreach (FlipController fc in Resources.FindObjectsOfTypeAll<FlipController>())
            {
                if (fc.gameObject.name != "Knight")
                {
                    if (!fc.gameObject)
                    {
                        GameObject.Destroy(fc);
                        continue;
                    }
                    betweenSceneFCs.Add(fc);
                    fc.enabled = false;
                } else fc.ResetScale();
            }

            foreach (FlipController fc in betweenSceneFCs)
            {
                if (fc.gameObject)
                    fc.enabled = true;
            }
            betweenSceneFCs.Clear();

            if (arg0.name != null)
            {
                List<GameObject> gameObjectsInScene = arg1.GetAllGameObjects();
                foreach (GameObject go in gameObjectsInScene)
                {
                    if (!go.activeSelf)
                        continue;

                    if (CleanName(go.name).Equals(CleanName("Grub")) || CleanName(go.name).Contains(CleanName("Grub Saved")))
                    {
                        CreateSpriteObject(go, false);
                        continue;
                    }

                    foreach (string flipName in FlippableList.sceneFlipList)
                    {
                        string[] flipSplit = flipName.Split('/');

                        string sceneName = CleanName(flipSplit[0]);
                        string objName = CleanName(flipSplit[flipSplit.Length - 1]);

                        if (!CleanName(arg1.name).Equals(CleanName(sceneName)) || !CleanName(go.name).Equals(CleanName(objName)))
                            continue;

                        if (flipSplit.Length == 1)
                        {
                            CreateSpriteObject(go, false);
                            continue;
                        }

                        string parent = CleanName(flipSplit[flipSplit.Length - 2]);

                        if (CleanName(parent) == CleanName(sceneName) || (go.transform.parent && CleanName(parent).Equals(CleanName(go.transform.parent.gameObject.name))))
                            CreateSpriteObject(go, false);
                    }
                }
            }
        }





        private int KnightDamaged(int damage)
        {
            GameObject hero = HeroController.instance.gameObject;
            hero.GetComponent<FlipController>().TriggerHit();

            return damage;
        }

        private void OnTakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            orig(self, hitInstance);

            GameObject obj = self.gameObject;
            if (obj && obj.GetComponent<FlipController>())
                obj.GetComponent<FlipController>().TriggerHit();
        }

        private void OnDie(On.HealthManager.orig_Die orig, HealthManager self, float? attackDirection, AttackTypes attackType, bool ignoreEvasion)
        {
            //if (self.gameObject.name != "Knight")
                //self.gameObject.GetComponent<FlipController>().enabled = false;
            orig(self, attackDirection, attackType, ignoreEvasion);
        }





        public string CleanName(string name)
        {
            string output = name.Trim();
            output = output.ToLower();
            if (output.IndexOf('(') > 0)
                output = output.Substring(0, output.IndexOf('('));
            return output.Trim();
        }

        public void CreateSpriteObject(GameObject obj, bool isEnemy)
        {
            if (obj.name.Contains("Pigeon"))
                return;

            if (obj.GetComponent<FlipController>() && !obj.GetComponent<FlipController>().SpriteObject)
                while (obj.RemoveComponent<FlipController>()) { }

            GameObject clone;
            if (obj.name == "Knight")
            {
                HeroController.instance.gameObject.SetActive(false);
                clone = obj.createCompanionFromPrefab(false);
                HeroController.instance.gameObject.SetActive(true);
                clone.RemoveComponent<HeroController>();
                clone.RemoveComponent<HeroAnimationController>();
                clone.RemoveComponent<HeroAudioController>();
                clone.RemoveComponent<ConveyorMovementHero>();
            }
            else clone = obj.createCompanionFromPrefab(false);

            if (!obj.GetComponent<FlipController>())
                obj.AddComponent<FlipController>();
            if (!obj.GetComponent<FlipController>().SpriteObject)
                obj.GetComponent<FlipController>().CreateSpriteObject(clone);
            obj.GetComponent<FlipController>().isEnemy = isEnemy;
            

            obj.GetComponent<FlipController>().direction = obj.transform.localScale.x > 0 ? 1 : -1;
            obj.GetComponent<FlipController>().spinGoal = obj.transform.localScale.x > 0 ? 1 : -1;
            obj.GetComponent<FlipController>().enabled = true;

            if (FlippableList.flippableChildren.ContainsKey(obj.name))
            {
                foreach (string childName in FlippableList.flippableChildren[obj.name])
                {
                    Transform spriteChildTransform = obj.GetComponent<FlipController>().SpriteObject.transform.Find(childName);
                    Transform mainChildTransform = obj.transform.Find(childName);
                    if (spriteChildTransform && mainChildTransform)
                    {
                        if (obj.name.Equals("False Knight Dream") && childName.Equals("Hitter"))
                            continue;
                        obj.GetComponent<FlipController>().AddSpriteChild(spriteChildTransform.gameObject, mainChildTransform.gameObject);
                        spriteChildTransform.gameObject.SetActive(true);
                        mainChildTransform.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = true;
                        spriteChildTransform.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = false;
                    }
                }
            }
        }
    }
}
