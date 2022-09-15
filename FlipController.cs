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
using Satchel;
using static Satchel.AssemblyUtils;

namespace PaperKnight
{
    public class FlipController : MonoBehaviour
    {
        internal float speed = 1;
        internal bool isSpinning = false;
        internal bool wasHit = false;
        internal float spinGoal = 1;
        internal float direction = 1;
        internal float offset = 0;
        internal float rotationCenter = 0;
        internal bool constant = false;
        internal int timer = 0;
        internal const float FLIP_RATE = 0.2F;

        internal GameObject SpriteObject;
        internal List<GameObject> SpriteChildren = new List<GameObject>();
        internal bool isEnemy = false;





        public void Awake()
        {
            this.isEnemy = false;
            this.isSpinning = false;
            this.speed = 1;
            this.direction = this.gameObject.transform.localScale.x;
            this.spinGoal = this.gameObject.transform.localScale.x;
        }

        public void OnDestroy()
        {
            if (this.gameObject && this.gameObject.GetComponent<MeshRenderer>())
                this.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = false;
            if (this.gameObject && this.gameObject.GetComponent<SpriteRenderer>())
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if (SpriteObject)
            {
                if (SpriteObject.GetComponent<MeshRenderer>())
                    SpriteObject.GetComponent<MeshRenderer>().enabled = false;
                if (SpriteObject.GetComponent<SpriteRenderer>())
                    SpriteObject.GetComponent<SpriteRenderer>().enabled = false;
                SpriteObject.SetActive(false);
            }
        }

        public void OnDisable()
        {   
            if (this.gameObject && this.gameObject.GetComponent<MeshRenderer>())
                this.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = false;
            if (this.gameObject && this.gameObject.GetComponent<SpriteRenderer>())
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if (SpriteObject)
            {
                if (SpriteObject.GetComponent<MeshRenderer>())
                    SpriteObject.GetComponent<MeshRenderer>().enabled = false;
                if (SpriteObject.GetComponent<SpriteRenderer>())
                    SpriteObject.GetComponent<SpriteRenderer>().enabled = false;
                SpriteObject.SetActive(false);
            }
        }

        public void OnEnable()
        {
            if (SpriteObject)
            {
                SpriteObject.SetActive(true);
                if (SpriteObject.GetComponent<MeshRenderer>())
                    SpriteObject.GetComponent<MeshRenderer>().enabled = true;
                if (this.gameObject && this.gameObject.GetComponent<MeshRenderer>())
                    this.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = true;
                if (SpriteObject.GetComponent<SpriteRenderer>())
                    SpriteObject.GetComponent<SpriteRenderer>().enabled = true;
                if (this.gameObject && this.gameObject.GetComponent<SpriteRenderer>())
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }





        public void Update()
        {
            if (SpriteObject)
            {
                if (!SpriteObject.name.Contains(this.gameObject.name))
                    SetupSpriteObject();
                

                if (SpriteObject.transform)
                {
                    if (!isSpinning && IsTurning())
                        TriggerTurn();
                    if (!isSpinning)
                        ResetScale();

                    SpriteObject.transform.localScale = new Vector3(direction, SpriteObject.transform.localScale.y, SpriteObject.transform.localScale.z);
                    string parentName = this.gameObject.transform.parent ? this.gameObject.transform.parent.gameObject.name : null;
                    float rotCenter = FlippableList.rotationCenter(this.gameObject.name, parentName);
                    offset = (rotCenter == 0 ? rotCenter : rotCenter - this.gameObject.transform.position.x) * (1 - (direction / Math.Abs(spinGoal)));
                    SpriteObject.transform.position = this.gameObject.transform.position + new Vector3(offset, 0, 0);
                    SpriteObject.transform.rotation = this.gameObject.transform.rotation;
                }

                if (SpriteObject.GetComponent<MeshRenderer>() && this.gameObject && this.gameObject.GetComponent<MeshRenderer>())
                    SpriteObject.GetComponent<MeshRenderer>().enabled = this.gameObject.GetComponent<MeshRenderer>().enabled;
                if (this.gameObject && this.gameObject.GetComponent<MeshRenderer>())
                    this.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = true;
                if (SpriteObject.GetComponent<SpriteRenderer>())
                    SpriteObject.GetComponent<SpriteRenderer>().enabled = true;
                if (this.gameObject && this.gameObject.GetComponent<SpriteRenderer>())
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                if (this.gameObject.name == "Absolute Radiance")
                    this.gameObject.transform.Find("Legs").gameObject.GetComponent<MeshRenderer>().forceRenderingOff = true;

                SpriteObject.GetComponent<SpriteMirror>().ImitateClips();
                foreach (GameObject child in SpriteChildren)
                    child.GetComponent<SpriteMirror>().ImitateClips();

                //SpriteObject.GetComponent<tk2dSpriteAnimator>().Sprite.CurrentSprite.material.SetColor("_Color", new Color(1, 1, 1, 0.4F));
            }
        }

        public void FixedUpdate()
        {
            

            if (timer <= 0)
            {
                if (SpriteObject)
                {
                    if (intersectsDash())
                        TriggerSpin();
                    if (intersectsNail())
                        TriggerHit();
                }
            } 
            else
            {
                timer -= 1;
            }

            if (this.gameObject.name.Equals("Knight"))
            {
                if (HeroController.instance.cState.hazardDeath)
                    SpriteObject.SetActive(false);
                else if (!SpriteObject.activeSelf)
                    SpriteObject.SetActive(true);
            }

            if (isSpinning && SpriteObject)
                IterateSpin();
        }

        private bool intersectsNail()
        {
            if (isEnemy)
                return false;

            if (PaperKnight.GlobalSaveData.unlimitedSpinning ? false : speed > 10)
                return false;

            if (!SpriteObject.GetComponent<MeshRenderer>() && !SpriteObject.GetComponent<SpriteRenderer>())
                return false;

            if (!(SpriteObject.name != "Knight" && SpriteObject.name != "Knight0"))
                return false;

            UnityEngine.Bounds objBounds = SpriteObject.GetComponent<MeshRenderer>() ? SpriteObject.GetComponent<MeshRenderer>().bounds :
                SpriteObject.GetComponent<SpriteRenderer>() ? SpriteObject.GetComponent<SpriteRenderer>().bounds : new UnityEngine.Bounds();
            UnityEngine.Bounds sBounds = new UnityEngine.Bounds(
                new Vector3(objBounds.center.x, objBounds.center.y, 0),
                new Vector3(objBounds.size.x, objBounds.size.y, 0)
            );

            FlippableList.buildNailAttacks();

            if (FlippableList.nailAttacks == null)
                return false;

            foreach (GameObject nailAttack in FlippableList.nailAttacks)
            {
                if (!nailAttack || !nailAttack.activeSelf || nailAttack.name == "Attacks" || !nailAttack.GetComponent<MeshRenderer>().enabled)
                    continue;

                UnityEngine.Bounds attackBounds = nailAttack.GetComponent<MeshRenderer>().bounds;
                UnityEngine.Bounds aBounds = new UnityEngine.Bounds(
                    new Vector3(attackBounds.center.x, attackBounds.center.y, 0),
                    new Vector3(attackBounds.size.x, attackBounds.size.y, 0)
                );

                if (sBounds.Intersects(aBounds))
                    return true;
            }

            return false;
        }

        internal void CreateSpriteObject(GameObject clone)
        {
            this.SpriteObject = clone;
            SpriteObject.AddComponent<SpriteMirror>();
            SpriteObject.GetComponent<SpriteMirror>().mirror = this.gameObject;
            if (this.gameObject.GetComponent<SpriteFlash>())
            {
                SpriteObject.AddComponent<SpriteFlash>();
                this.gameObject.copyComponent<SpriteFlash>(SpriteObject);
            }
        }

        internal void AddSpriteChild(GameObject sprite, GameObject mirror)
        {
            SpriteChildren.Add(sprite);
            sprite.AddComponent<SpriteMirror>();
            sprite.GetComponent<SpriteMirror>().mirror = mirror;
            if (mirror.GetComponent<SpriteFlash>())
            {
                sprite.AddComponent<SpriteFlash>();
                mirror.copyComponent<SpriteFlash>(sprite);
            }
        }

        public bool intersectsDash()
        {
            if (!((HeroController.instance.cState.dashing || HeroController.instance.cState.superDashing) && speed < 5))
                return false;

            if ((!SpriteObject.GetComponent<MeshRenderer>() && !SpriteObject.GetComponent<SpriteRenderer>()) || 
                !HeroController.instance.GetComponent<MeshRenderer>())
                return false;

            GameObject HeroSprite = HeroController.instance.GetComponent<FlipController>().SpriteObject;
            if (!(SpriteObject.name != "Knight" && SpriteObject.name != "Knight0" && HeroSprite))
                return false;
            
            UnityEngine.Bounds objBounds = SpriteObject.GetComponent<MeshRenderer>() ? SpriteObject.GetComponent<MeshRenderer>().bounds :
                SpriteObject.GetComponent<SpriteRenderer>() ? SpriteObject.GetComponent<SpriteRenderer>().bounds : new UnityEngine.Bounds();
            UnityEngine.Bounds sBounds = new UnityEngine.Bounds(
                new Vector3(objBounds.center.x, objBounds.center.y, 0),
                new Vector3(objBounds.size.x, objBounds.size.y, 0)
            );

            UnityEngine.Bounds heroBounds = HeroSprite.GetComponent<MeshRenderer>().bounds;
            UnityEngine.Bounds hBounds = new UnityEngine.Bounds(
                new Vector3(heroBounds.center.x, heroBounds.center.y, 0),
                new Vector3(heroBounds.size.x, heroBounds.size.y, 0)
            );

            if (sBounds.Intersects(hBounds))
                return true;

            foreach (GameObject child in SpriteChildren)
            {
                UnityEngine.Bounds childBounds = child.GetComponent<MeshRenderer>().bounds;
                UnityEngine.Bounds cBounds = new UnityEngine.Bounds(
                    new Vector3(childBounds.center.x, childBounds.center.y, 0),
                    new Vector3(childBounds.size.x, childBounds.size.y, 0)
                );

                if (cBounds.Intersects(hBounds))
                    return true;
            }

            return false;
        }

        public void SetupSpriteObject()
        {
            while (SpriteObject.RemoveComponent<Collider2D>()) { }
            while (SpriteObject.RemoveComponent<Walker>()) { }
            while (SpriteObject.RemoveComponent<Climber>()) { }
            while (SpriteObject.RemoveComponent<Crawler>()) { }
            while (SpriteObject.RemoveComponent<BigBouncer>()) { }
            while (SpriteObject.RemoveComponent<NonBouncer>()) { }

            if (SpriteObject.GetComponent<Rigidbody2D>())
            {
                SpriteObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                SpriteObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }

            SpriteObject.RemoveComponent<HealthManager>();
            SpriteObject.name = this.gameObject.name + "0";
            SpriteObject.layer = this.gameObject.layer;
            SpriteObject.SetActive(true);
            SpriteObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            SpriteObject.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, 0.001f);
        }

        /*
        private void ImitateClips()
        {
            if (source == null)
                source = this.gameObject.GetComponent<tk2dSpriteAnimator>();
            if (destination == null)
                destination = SpriteObject.GetComponent<tk2dSpriteAnimator>();
            if (source == null || destination == null)
            {
                PaperKnight.Instance.Log("Source or Destination animator is null: " + source + " | " + destination);
                return;
            }

            var clip = source.CurrentClip;
            if (clip == null)
                return;
            if (clip.name != currentSourceClip || (!playing && this.gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying(clip.name)))
            {
                playing = true;
                currentSourceClip = clip.name;
                destination = SpriteObject.GetComponent<tk2dSpriteAnimator>();
                if (destination != null)
                    destination.Play(currentSourceClip);
            }
        }
        */

        private void IterateSpin()
        {
            float current = direction;

            if (spinGoal == current && (!wasHit || (wasHit && speed < 1.2)))
            {
                if (spinGoal * this.gameObject.transform.localScale.x > 0)
                {
                    EndSpin();
                    return;
                }
                if (!wasHit && isSpinning)
                    spinGoal *= -1;
            }

            if (wasHit && spinGoal == current)
                spinGoal *= -1;

            if (current > spinGoal)
                current += (float) (Math.Abs(Math.Sin((current * (Math.PI / 2)) + Math.PI / 2) * FLIP_RATE * speed) + (0.015F * speed)) * spinGoal;
            else if (current < spinGoal)
                current += (float) (Math.Abs(Math.Sin((current * (Math.PI / 2)) + Math.PI / 2) * FLIP_RATE * speed) + (0.015F * speed)) * spinGoal;
            if (Math.Abs(current - spinGoal) < 0.2 || Math.Abs(current) > Math.Abs(spinGoal))
                current = spinGoal;

            direction = current;

            if (speed > 1 && wasHit && !constant)
                speed -= 0.16F * (PaperKnight.GlobalSaveData.unlimitedSpinning ? 0.75F : speed / 3);
            if (speed > 1 && wasHit && constant)
                speed -= 0.03F;
            if (speed < 1)
                speed = 1;

            TieObjectsTogether();
            
            //PaperKnight.Instance.Log(current + "/" + Math.Abs(spinGoal) + "   " + this.gameObject.transform.position + " + " + offset);
            //PaperKnight.Instance.Log(SpriteObject.name + "   speed " + speed + "     current " + current + "    goal " + spinGoal + "   parent " + this.gameObject.transform.localScale.x);
        }

        private void TieObjectsTogether()
        {
            string name = this.gameObject.name;

            if (name == "queen_0022_a" || name == "queen_0023_a" || name == "Queen")
            {
                FlipController q = GameObject.Find("Queen").GetComponent<FlipController>();
                FlipController q22 = GameObject.Find("queen_0022_a").GetComponent<FlipController>();
                FlipController q23 = GameObject.Find("queen_0023_a").GetComponent<FlipController>();
                q.speed = q22.speed;
                q.isSpinning = q22.isSpinning;
                q.direction = q22.direction;
                q.wasHit = q22.wasHit;
                q.constant = q22.constant;
                q.spinGoal = q22.spinGoal;
                q23.speed = q22.speed;
                q23.isSpinning = q22.isSpinning;
                q23.direction = q22.direction;
                q23.wasHit = q22.wasHit;
                q23.constant = q22.constant;
                q23.spinGoal = q22.spinGoal;
            }
        }

        public void ResetScale()
        {
            if (this.gameObject)
            {
                isSpinning = false;
                direction = this.gameObject.transform.localScale.x;
                spinGoal = this.gameObject.transform.localScale.x;
                speed = 1;
                offset = 0;
                wasHit = false;
                constant = false;
                SpriteObject.transform.localScale = new Vector3(direction, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            }
        }

        private void EndSpin()
        {
            speed = 1;
            offset = 0;
            isSpinning = false;
            wasHit = false;
            direction = this.gameObject.transform.localScale.x;
            spinGoal = this.gameObject.transform.localScale.x;
            constant = false;
            SpriteObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, SpriteObject.transform.localScale.y, SpriteObject.transform.localScale.z);
        }

        public bool IsTurning()
        {
            if (direction >= 0 && this.gameObject.transform.localScale.x < 0)
                return true;
            if (direction <= 0 && this.gameObject.transform.localScale.x > 0)
                return true;
            return false;
        }

        internal void TriggerTurn()
        {
            speed = 1 + (this.gameObject.name == "Knight" ? 0.5F : 0);
            isSpinning = true;
            wasHit = false;
        }

        internal void TriggerSpin()
        {
            speed = 10;
            isSpinning = true;
            spinGoal = this.gameObject.transform.localScale.x * (this.gameObject.transform.localScale.x > 0 ? -1 : 1);
            wasHit = true;
            constant = false;
            timer = 10;
        }

        internal void TriggerHit()
        {
            speed += 2.8F;
            if (!PaperKnight.GlobalSaveData.unlimitedSpinning)
                speed = Math.Min(10, speed);
            isSpinning = true;
            spinGoal = this.gameObject.transform.localScale.x * (this.gameObject.transform.localScale.x > 0 ? -1 : 1);
            wasHit = true;
            constant = false;
            timer = 5;
        }

        internal void TriggerConstant()
        {
            speed = 2.5F;
            isSpinning = true;
            spinGoal = this.gameObject.transform.localScale.x * (this.gameObject.transform.localScale.x > 0 ? -1 : 1);
            wasHit = true;
            constant = true;
        }
    }
}
