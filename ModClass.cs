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
                //Log(GameObject.Find("Nightmare Spike").LocateMyFSM("Control").ActiveStateName);
                var afterburn = Resources.FindObjectsOfTypeAll<GameObject>()
                    .FirstOrDefault(x => x.name == "Pt Afterburn");

                var copy = GameObject.Instantiate(afterburn);
                copy.transform.position = Knight.transform.position;
                copy.SetActive(true);

                //Log(GameObject.Find("Nightmare Grimm Boss").LocateMyFSM("Control").FsmVariables.GetFsmBool("First Move").Value);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                //GameObject.Find("Nightmare Grimm Boss").LocateMyFSM("Control").SendEvent("OVER");
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

        public GameObject flameprefab = null;
        public GameObject afterburnprefab = null;
        public GameObject spikeprefab = null;

        public IEnumerator NKGfsm()
        {
            yield return new WaitForFinishedEnteringScene();

            Nkg = GameObject.Find("Nightmare Grimm Boss");
            fsm = Nkg.LocateMyFSM("Control");
            Knight = GameObject.Find("Knight");
            Skip(GameObject.Find("Grimm Control").LocateMyFSM("Control"));

            flameprefab = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "Nightmare UP Ball");
            afterburnprefab = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "Pt Afterburn");

            fsm.GetFirstActionOfType<Wait>("Out Pause").time = Constants.OffScreen;

            Dash();
            Attack();
            Spikes();
            Pillars();
            Bats();


            fsm.InsertCustomAction("AD Tele In", () =>
            {
                fsm.FsmVariables.GetFsmFloat("Tele X").Value = Knight.transform.position.x;
            }, 3);

            fsm.GetFirstActionOfType<SetPosition>("AD Tele In").y = 18f;
            fsm.GetFirstActionOfType<SetVelocityAsAngle>("AD Fire").angle = 270;

            fsm.ChangeTransition("GD Antic", "NEXT", "Tele Out");

            GameObject ball2 = GameObject.Instantiate(afterburnprefab);

            fsm.AddState("GD Explosion");
            fsm.ChangeTransition("AD Fire", "LAND", "GD Explosion");
            fsm.AddTransition("GD Explosion", "OVER", "GD Antic");

            fsm.InsertCustomAction("GD Explosion", () =>
            {
                ball2.hideFlags = HideFlags.None;
                ball2.transform.position = Nkg.transform.position - new Vector3(0, 1, 0);
                ball2.transform.localScale = new Vector3 (1, 1, 1);

                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.1f, () => ball2.GetComponent<CircleCollider2D>().enabled = true);

                var wawar = ball2.GetComponent<ParticleSystem>().main;
                wawar.startLifetime = 0.4f;
                wawar.startSizeX = 10f;
                wawar.startSizeY = 10f;
                ball2.GetComponent<ParticleSystem>().Clear();
                ball2.GetComponent<ParticleSystem>().Play();

                Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(0.5f, () => ball2.GetComponent<CircleCollider2D>().enabled = false);

                for(int i=0; i<3; i++)
                {
                    GameObject flame1 = GameObject.Instantiate(flameprefab);
                    flame1.transform.localScale = new Vector3(1.7f, 1.7f, 1);
                    //flame1.name += "DeleteTag";

                    flame1.transform.position = Nkg.transform.position + new Vector3(0, 1, 0);
                    Rigidbody2D rb1 = flame1.GetComponent<Rigidbody2D>();
                    rb1.velocity = new Vector2(20f, 30f);

                    GameObject flame2 = GameObject.Instantiate(flameprefab);
                    flame2.transform.localScale = new Vector3(1.7f, 1.7f, 1);

                    flame2.transform.position = Nkg.transform.position + new Vector3(0, 1, 0);
                    Rigidbody2D rb2 = flame2.GetComponent<Rigidbody2D>();
                    rb2.velocity = new Vector2(-20f, 30f);

                    GameObject flame3 = GameObject.Instantiate(flameprefab);
                    flame3.transform.localScale = new Vector3(2f, 2f, 1);

                    flame3.transform.position = Nkg.transform.position - new Vector3 (0, 1, 0);
                    Rigidbody2D rb3 = flame3.GetComponent<Rigidbody2D>();
                    rb3.velocity = new Vector2(40f, -10f);

                    GameObject flame4 = GameObject.Instantiate(flameprefab);
                    flame4.transform.localScale = new Vector3(2f, 2f, 1);

                    flame4.transform.position = Nkg.transform.position - new Vector3(0, 1, 0);
                    Rigidbody2D rb4 = flame4.GetComponent<Rigidbody2D>();
                    rb4.velocity = new Vector2(-40f, -10f);
                }
                float spa = Constants.DiveSpikeSpacing;
                for (float i = -3*spa; i <= 3*spa; i += spa)
                {
                    if(i!= 0 && Nkg.transform.position.x + i >= 68 && Nkg.transform.position.x + i <= 103)
                    {
                        GameObject side1 = GameObject.Instantiate(spikeprefab);
                        side1.transform.position = new Vector3(Nkg.transform.position.x + i, 4.6f, 0);
                        PlayMakerFSM fide1 = side1.LocateMyFSM("Control");
                        fide1.ChangeTransition("Dormant", "SPIKES READY", "Ready");
                        fide1.SendEvent("SPIKES READY");
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(Constants.DiveSpikeSpawn, () => fide1.SendEvent("SPIKES UP"));
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(Constants.DiveSpikeDecay, () => fide1.SendEvent("SPIKES DOWN"));
                        Satchel.CoroutineHelper.WaitForSecondsBeforeInvoke(Constants.DiveSpikeDecay + 1, () => GameObject.Destroy(side1));
                    }
                }

                fsm.SendEvent("OVER");
            }, 0);

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