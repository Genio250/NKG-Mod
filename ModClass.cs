using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker.Ecosystem.Utils;
using InControl;
using Modding;
using Satchel;
//using SFCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Satchel.Futils;
using Satchel.Futils.Serialiser;

namespace NKG
{
    public partial class NKG : Mod
    {
        public NKG() : base("NKG") { }
        public override string GetVersion() => "1.0";
        public override void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
        }

        private void ModHooks_HeroUpdateHook()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                //Log(GameObject.Find("Nightmare Grimm Boss").LocateMyFSM("Control").ActiveStateName);
                Log(GameObject.Find("Nightmare Spike").LocateMyFSM("Control").ActiveStateName);

                //Log(GameObject.Find("Nightmare Grimm Boss").LocateMyFSM("Control").FsmVariables.GetFsmBool("First Move").Value);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                //GameObject.Find("Nightmare Grimm Boss").LocateMyFSM("Control").SendEvent("OVER");
                GameObject bat = Satchel.FsmUtil.GetFirstActionOfType<SpawnObjectFromGlobalPool>(fsm, "Firebat 1").gameObject.Value;
                PlayMakerFSM batfsm = bat.LocateMyFSM("Control");
                Log(batfsm.ActiveStateName);
            }
        }

        private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
        {
            if (arg1.name.Contains("Nightmare"))
            {
                RotationCancel();
                GameManager.instance.StartCoroutine(NKGfsm());
            }
        }

        public int finish;
        public GameObject Nkg = null;
        public PlayMakerFSM fsm = null;
        public GameObject Knight = null;

        public IEnumerator NKGfsm()
        {
            yield return new WaitForFinishedEnteringScene();

            Nkg = GameObject.Find("Nightmare Grimm Boss");
            fsm = Nkg.LocateMyFSM("Control");
            Knight = GameObject.Find("Knight");
            Skip(GameObject.Find("Grimm Control").LocateMyFSM("Control"));

            fsm.GetFirstActionOfType<Wait>("Out Pause").time = Constants.OffScreen;

            Dash();
            Attack();
            Spikes();
            Pillars();
            Bats();
    

            //batfsm.ChangeTransition("Init", "HIGH", "Low");
        }

        /*private void Flamer(PlayMakerFSM fsm, string s, int n)
        {
            fsm.InsertAction(s, new SpawnObjectFromGlobalPoolOverTime
            {
                gameObject = fsm.GetValidState("AD Fire").GetFirstActionOfType<SpawnObjectFromGlobalPoolOverTime>().gameObject,
                spawnPoint = fsm.GetValidState("AD Fire").GetFirstActionOfType<SpawnObjectFromGlobalPoolOverTime>().spawnPoint,
                position = fsm.GetValidState("AD Fire").GetFirstActionOfType<SpawnObjectFromGlobalPoolOverTime>().position,
                rotation = fsm.GetValidState("AD Fire").GetFirstActionOfType<SpawnObjectFromGlobalPoolOverTime>().rotation,
                frequency = fsm.GetValidState("AD Fire").GetFirstActionOfType<SpawnObjectFromGlobalPoolOverTime>().frequency
            }, n);
        }*/

        //fsm.GetValidState("Knight 1").GetFirstActionOfType<IntCompare>().integer2 = new FsmInt { Value = 10 };

    }
    }