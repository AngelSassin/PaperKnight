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
    public class FlipController : MonoBehaviour
    {
        internal float speed = 1;
        internal bool isSpinning = false;
        internal bool wasHit = false;
        internal float spinGoal = 1;
        internal float direction = 1;
        internal const float FLIP_RATE = 0.2F;
        internal float color = 1;
        //internal tk2dSpriteAnimator sa;
        //internal Material mat = new Material(Shader.Find("Sprites/Default"));



        public void Awake()
        {
            //sa = this.gameObject.GetComponent<tk2dSpriteAnimator>();

            this.isSpinning = false;
            this.speed = 1;
            this.direction = this.gameObject.transform.localScale.x > 0 ? 1 : -1;
            this.spinGoal = direction;
        }

        public void FixedUpdate()
        {
            if (!isSpinning && IsTurning())
                TriggerTurn();

            IterateSpin();

            PerformShaders();
        }



        private void PerformShaders()
        {
            //Shader resizeShader = Instantiate(PaperKnight.Instance.ab.LoadAsset<Shader>("resizeshader"));
            //sa.Sprite.CurrentSprite.material = new Material(resizeShader);
            //sa.Sprite.CurrentSprite.material.SetVector("_Scaling", new Vector4(1, 0.5F, 1, 1));

            //Shader colorShader = Shader.Find("Sprites/Default");
            //sa.Sprite.CurrentSprite.material = new Material(colorShader);

            //this.gameObject.GetComponent<tk2dSpriteAnimator>().Sprite.CurrentSprite.material = new Material(Shader.Find("Sprites/Default"));
            //this.gameObject.GetComponent<tk2dSpriteAnimator>().Sprite.CurrentSprite.material.SetVector("_Scaling", new Vector4(0.7F, 0.5F, 0.7F, 0.5F));
            //this.gameObject.GetComponent<tk2dSpriteAnimator>().Sprite.CurrentSprite.material.SetFloat("_Align", 0.0F);
            //PaperKnight.Instance.Log("Shader | " + this.gameObject.name + " | " + this.gameObject.GetComponent<tk2dSpriteAnimator>().Sprite.CurrentSprite.material.shader.name);
            //this.gameObject.GetComponent<tk2dSpriteAnimator>().Sprite.CurrentSprite.material = mat;
            if (color >= 1)
                color = 0;
            else color += 0.01F;
            this.gameObject.GetComponent<tk2dSpriteAnimator>().Sprite.CurrentSprite.material.SetColor("_Color", new Color(1, color, 1, 0.5F));
        }

        public void ResetScale()
        {
            if (this.gameObject)
            {
                //PaperKnight.Instance.Log("Fix Look Direction: " + this.gameObject.name + "   " + this.gameObject.transform.localScale.x);
                isSpinning = false;
                direction = spinGoal;
                speed = 1;
                wasHit = false;
                this.gameObject.transform.localScale = new Vector3(direction, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            }
        }

        public bool IsTurning()
        {
            if (direction >= 0 && this.gameObject.transform.localScale.x < 0)
                return true;
            if (direction <= 0 && this.gameObject.transform.localScale.x > 0)
                return true;
            return false;
        }

        private void IterateSpin()
        {
            float current = direction;

            if (spinGoal == current && (!wasHit || (wasHit && speed <= 1)))
            {
                EndSpin();
                return;
            }

            if (wasHit && spinGoal == current)
                spinGoal *= -1;

            if (current > spinGoal)
                current -= (float)Math.Abs(Math.Sin((current * (Math.PI / 2)) + Math.PI / 2) * FLIP_RATE * speed) + (0.015F * speed);
            else if (current < spinGoal)
                current += (float)Math.Abs(Math.Sin((current * (Math.PI / 2)) + Math.PI / 2) * FLIP_RATE * speed) + (0.015F * speed);
            if (Math.Abs(current - spinGoal) < 0.1 || Math.Abs(current) > Math.Abs(spinGoal))
                current = spinGoal;

            this.gameObject.transform.localScale = new Vector3(current, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            direction = this.gameObject.transform.localScale.x;

            if (wasHit && speed > 1)
                speed -= 0.1F * (PaperKnight.GlobalSaveData.unlimitedSpinning ? 1 : speed / 3);
            if (speed < 1)
                speed = 1;

            //PaperKnight.Instance.Log("Update Spin: " + this.gameObject.name + ":   speed " + speed + "     current " + current + "    goal " + spinGoal);
        }

        private void EndSpin()
        {
            speed = 1;
            isSpinning = false;
            wasHit = false;
        }

        internal void TriggerTurn()
        {
            //PaperKnight.Instance.Log("Turning: " + this.gameObject.name + "   " + direction + " to " + this.gameObject.transform.localScale.x);
            speed = 1;
            isSpinning = true;
            spinGoal = direction > 0 ? -1 : 1;
            wasHit = false;
            //PaperKnight.Instance.Log(">> Start Turn Flip: " + this.gameObject.name + "   " + this.gameObject.transform.localScale.x + " to " + spinGoal);
        }

        internal void TriggerHit()
        {
            speed += 5;
            isSpinning = true;
            spinGoal = direction > 0 ? -1 : 1;
            wasHit = true;
            //PaperKnight.Instance.Log(">> Start Hit Flip: " + this.gameObject.name + "   " + this.gameObject.transform.localScale.x + " to " + spinGoal);
        }
    }
}
