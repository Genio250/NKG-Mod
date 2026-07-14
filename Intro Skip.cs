using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using Modding;
using Satchel;
using Satchel.Futils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NKG
{
    public partial class NKG
    {
        public void TrySetStateWaitValue(PlayMakerFSM fsm, string stateName, float value)
        {
            try
            {
                Wait wait = fsm.Fsm.GetState(stateName)?.Actions?.OfType<Wait>().FirstOrDefault();
                if (wait != null)
                {
                    wait.time.Value = value;
                }
            }
            catch
            {
                Modding.Logger.Log("Hi");
            }
        }

        public void Skip(PlayMakerFSM control)
        {
            TrySetStateWaitValue(control, "Pause", 0.5f);
            TrySetStateWaitValue(control, "Pan Over", 0.5f);
            TrySetStateWaitValue(control, "Eye 1", 0.1f);
            TrySetStateWaitValue(control, "Eye 2", 0.1f);
            TrySetStateWaitValue(control, "Pan Over 2", 0.1f);
            TrySetStateWaitValue(control, "Eye 3", 0.1f);
            TrySetStateWaitValue(control, "Eye 4", 0.1f);
            TrySetStateWaitValue(control, "Silhouette", 0.1f);
            TrySetStateWaitValue(control, "Silhouette 2", 0.1f);
            TrySetStateWaitValue(control, "Title Up", 0.1f);
            TrySetStateWaitValue(control, "Title Up 2", 0.1f);
            TrySetStateWaitValue(control, "Defeated Pause", 0.1f);
            TrySetStateWaitValue(control, "Defeated Start", 0.1f);
            TrySetStateWaitValue(control, "Explode Start", 0.1f);
            TrySetStateWaitValue(control, "Silhouette Up", 0.1f);
            TrySetStateWaitValue(control, "Ash Away", 0.1f);
            TrySetStateWaitValue(control, "Fade", 0.1f);
        }
    }
}