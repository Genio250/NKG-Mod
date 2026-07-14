using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;
using Satchel;
using HutongGames.PlayMaker;
using System.Reflection;
using HutongGames.PlayMaker.Ecosystem.Utils;
using InControl;


namespace NKG
{
    public partial class NKG
    {
        public void Spikes()
        {
            GameObject holderin = GameObject.Find("Grimm Spike Holder");
            GameObject holderout = GameObject.Instantiate(holderin);
            holderin.name = "Grimm Spike Holder In";
            holderout.name = "Grimm Spike Holder Out";
            holderout.DisableChildren();

            holderin.LocateMyFSM("Spike Control").GetValidState("Ready").GetFirstActionOfType<RandomFloatEither>().value1 = 67.125f;
            holderout.LocateMyFSM("Spike Control").GetValidState("Ready").GetFirstActionOfType<RandomFloatEither>().value1 = 67.125f;

            UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in array)
            {
                if (obj.name.Contains("Nightmare Spike"))
                {
                    GameObject one = GameObject.Instantiate(obj);
                    GameObject two = GameObject.Instantiate(obj);
                    one.name = obj.name + "(Clone1)";
                    one.transform.position = obj.transform.position + new Vector3(1, 0, 0);
                    one.LocateMyFSM("Control").RemoveAction("Init", 1);
                    one.LocateMyFSM("Control").RemoveAction("Init", 3);

                    one.LocateMyFSM("Control").SetState("Init");
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () =>
                    {
                        one.LocateMyFSM("Control").SendEvent("FINISHED");
                    });

                    two.name = obj.name + "(Clone2)";
                    two.transform.position = obj.transform.position + new Vector3(2, 0, 0);
                    two.LocateMyFSM("Control").RemoveAction("Init", 1);
                    two.LocateMyFSM("Control").RemoveAction("Init", 3);

                    if (obj.name.Contains("4") || obj.name.Contains("5") || obj.name.Contains("6") || obj.name.Contains("7") || obj.name.Contains("8"))
                    {
                        one.transform.parent = holderin.transform;
                        two.transform.parent = holderin.transform;
                        obj.transform.parent = holderin.transform;
                    }
                    else
                    {
                        one.transform.parent = holderout.transform;
                        two.transform.parent = holderout.transform;
                        obj.transform.parent = holderout.transform;
                    }

                    two.LocateMyFSM("Control").SetState("Init");

                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () =>
                    {
                        two.LocateMyFSM("Control").SendEvent("FINISHED");
                    });
                }
            }
            GameObject.Find("Nightmare Spike").transform.SetScaleY(1.38f);
            GameObject.Find("Nightmare Spike(Clone1)").transform.SetScaleY(1.38f);
            GameObject.Find("Nightmare Spike(Clone2)").transform.SetScaleY(1.38f);
            GameObject.Find("Nightmare Spike (12)(Clone1)").transform.SetScaleY(1.38f);
            GameObject.Find("Nightmare Spike (12)").transform.SetScaleY(1.38f);

            fsm.AddState("Spike Spawn");
            fsm.ChangeTransition("Move Choice", "SPIKES", "Spike Spawn");
            fsm.AddTransition("Spike Spawn", "OVER", "Spike Return");

            fsm.InsertCustomAction("Spike Spawn", () =>
            {
                int bleh = UnityEngine.Random.Range(0, 2);
                Log(bleh);

                if (bleh == 0)
                {
                    GameObject.Find("Grimm Spike Holder Out").LocateMyFSM("Spike Control").SendEvent("SPIKE ATTACK");
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(Constants.SpikeCooldown, () =>
                        GameObject.Find("Grimm Spike Holder In").LocateMyFSM("Spike Control").SendEvent("SPIKE ATTACK"));
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2 * Constants.SpikeCooldown, () =>
                        GameObject.Find("Grimm Spike Holder Out").LocateMyFSM("Spike Control").SendEvent("SPIKE ATTACK"));
                }
                else
                {
                    GameObject.Find("Grimm Spike Holder In").LocateMyFSM("Spike Control").SendEvent("SPIKE ATTACK");
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(Constants.SpikeCooldown, () =>
                        GameObject.Find("Grimm Spike Holder Out").LocateMyFSM("Spike Control").SendEvent("SPIKE ATTACK"));
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(2 * Constants.SpikeCooldown, () =>
                        GameObject.Find("Grimm Spike Holder In").LocateMyFSM("Spike Control").SendEvent("SPIKE ATTACK"));
                }

                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(4 * Constants.SpikeCooldown, () => fsm.SendEvent("OVER"));//1.85
            }, 0);
        }

        public void RotationCancel()
        {
            UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in array)
            {
                if (obj.name.Contains("Nightmare Spike"))
                {
                    obj.LocateMyFSM("Control").RemoveAction("Init", 1);
                    obj.LocateMyFSM("Control").RemoveAction("Init", 3);

                    obj.LocateMyFSM("Control").SetState("Init");
                    Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () =>
                    {
                        obj.LocateMyFSM("Control").SendEvent("FINISHED");
                    });
                }
            }
        }
    }
}