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
        public void Dash()
        {
            fsm.AddState("Dash Sider");
            fsm.CopyState("Slash Tele In", "Dash Tele In");
            fsm.CopyState("Slash Antic", "Dash Antic");
            fsm.CopyState("G Dash", "Dashing");
            fsm.CopyState("G Dash Recover", "Dash Finish");
            fsm.RemoveAction("Dashing", 7);

            fsm.AddTransition("Move Choice", "DASH", "Dash Sider");
            fsm.AddTransition("Dash Sider", "OVER", "Dash Tele In");
            fsm.ChangeTransition("Dash Tele In", "FINISHED", "Dash Antic");
            fsm.ChangeTransition("Dash Antic", "FINISHED", "Dashing");
            fsm.RemoveTransition("Dashing", "FINISHED");
            fsm.AddTransition("Dashing", "OVER", "Dash Finish");

            fsm.InsertCustomAction("Dash Sider", () =>
            {
                fsm.FsmVariables.GetFsmBool("First Move").Value = true;
            }, 0);

            fsm.InsertCustomAction("Dash Sider", () =>
            { // 77, 86, 94
                if (Knight.transform.position.x <= 77)
                {
                    fsm.GetValidState("Dash Tele In").GetFirstActionOfType<SetPosition>().x = new FsmFloat { Value = 99.5f };
                    finish = 73;
                }
                else if (Knight.transform.position.x >= 94)
                {
                    fsm.GetValidState("Dash Tele In").GetFirstActionOfType<SetPosition>().x = new FsmFloat { Value = 67.9f };
                    finish = 100;
                }
                else
                {
                    float Side = UnityEngine.Random.Range(-1f, 1f);
                    if (Side >= 0)
                    {
                        fsm.GetValidState("Dash Tele In").GetFirstActionOfType<SetPosition>().x = new FsmFloat { Value = 99.5f };
                        finish = 73;
                    }
                    else
                    {
                        fsm.GetValidState("Dash Tele In").GetFirstActionOfType<SetPosition>().x = new FsmFloat { Value = 67.9f };
                        finish = 100;
                    }
                }
                Log(finish);
                fsm.SendEvent("OVER");
            }, 1);

            fsm.InsertCustomAction("Dashing", () =>
            {
                ModHooks.HeroUpdateHook += DashStop;
            }, 7);

            Log(fsm.GetFirstActionOfType<SetVelocity2d>("Dashing").x.Value);

            fsm.GetFirstActionOfType<SetVelocity2d>("Dashing").x.Value = Constants.DashSpeed;

            fsm.GetFirstActionOfType<Wait>("Dash Antic").time = Constants.DashTelegraph;
        }

        public void DashStop()
        {
            if (Math.Abs(GameObject.Find("Nightmare Grimm Boss").transform.position.x - finish) <= 0.5f)
            {
                Log(Math.Abs(GameObject.Find("Nightmare Grimm Boss").transform.position.x - finish));
                GameObject.Find("Nightmare Grimm Boss").LocateMyFSM("Control").SendEvent("OVER");
                ModHooks.HeroUpdateHook -= DashStop;
            }
        }
    }
}