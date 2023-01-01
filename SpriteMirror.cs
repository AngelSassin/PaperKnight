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
        internal bool playing = false;

        internal void MoveToPosition(float period)
        {
            float offset = (mirror.transform.parent.position.x - mirror.transform.position.x) * period;
            this.gameObject.transform.position = mirror.transform.position + new Vector3(offset, 0, 0);
        }

        internal void ImitateClips()
        {
            if (!mirror)
                return;

            MeshRenderer objMesh = this.gameObject.GetComponent<MeshRenderer>();
            MeshRenderer mirrorMesh = mirror.GetComponent<MeshRenderer>();

            tk2dSpriteAnimator objAnimator = this.gameObject.GetComponent<tk2dSpriteAnimator>();
            tk2dSpriteAnimator mirrorAnimator = mirror.GetComponent<tk2dSpriteAnimator>();

            if (mirrorMesh)
            {
                if (!mirror.activeSelf || !mirrorMesh.enabled)
                    objMesh.forceRenderingOff = true;
                else objMesh.forceRenderingOff = false;

                mirrorMesh.forceRenderingOff = true;
            }

            if (objAnimator && mirrorAnimator)
            {
                if (mirrorAnimator.ClipTimeSeconds < objAnimator.ClipTimeSeconds)
                    objAnimator.Stop();
                if (mirrorAnimator.CurrentClip != null && objAnimator.CurrentClip != mirrorAnimator.CurrentClip)
                    objAnimator.Play(mirrorAnimator.CurrentClip);
                if (mirrorAnimator.Playing)
                    objAnimator.SetFrame(mirrorAnimator.CurrentFrame);
            }
        }
    }
}