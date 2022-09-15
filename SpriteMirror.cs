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
    public class SpriteMirror : MonoBehaviour
    {
        internal GameObject mirror;
        private tk2dSpriteAnimator source, destination;
        private string currentSourceClip;
        internal bool playing = false;

        internal void ImitateClips()
        {
            //SpriteFlash flash = mirror.GetComponent<SpriteFlash>();
            //if (flash.enabled)
            //    this.gameObject.copyComponent<SpriteFlash>(mirror);

            if (!mirror.activeSelf || !mirror.GetComponent<MeshRenderer>().enabled)
                this.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = true;
            else this.gameObject.GetComponent<MeshRenderer>().forceRenderingOff = false;

            if (this.gameObject.GetComponent<tk2dSpriteAnimator>() && mirror.GetComponent<tk2dSpriteAnimator>()) 
                if (!this.gameObject.GetComponent<tk2dSpriteAnimator>().IsPlaying(currentSourceClip) && !mirror.GetComponent<tk2dSpriteAnimator>().IsPlaying(currentSourceClip))
                    playing = false;

            if (!mirror.GetComponent<tk2dSpriteAnimator>().Playing)
                this.gameObject.GetComponent<tk2dSpriteAnimator>().Stop();

            mirror.GetComponent<MeshRenderer>().forceRenderingOff = true;

            if (source == null)
                source = mirror.GetComponent<tk2dSpriteAnimator>();
            if (destination == null)
                destination = this.gameObject.GetComponent<tk2dSpriteAnimator>();
            if (source == null || destination == null)
            {
                PaperKnight.Instance.Log("Source or Destination animator is null: " + source + " | " + destination);
                return;
            }

            var clip = source.CurrentClip;
            if (clip == null)
                return;
            if (clip.name != currentSourceClip || (!playing && mirror.GetComponent<tk2dSpriteAnimator>().IsPlaying(clip.name)))
            {
                playing = true;
                currentSourceClip = clip.name;
                destination = this.gameObject.GetComponent<tk2dSpriteAnimator>();
                if (destination != null)
                    destination.Play(currentSourceClip);
            } 
        }
    }
}