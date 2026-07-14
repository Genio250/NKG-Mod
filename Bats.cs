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
        public void Bats()
        {
            Batter("Firebat 1");
            Batter("Firebat 2");
            Batter("Firebat 3");
            Batter("Firebat 4");

            fsm.CopyState("Firebat 1", "Firebat 1 Sub");
            fsm.ChangeTransition("Firebat 1", "END", "Firebat 1 Sub");
            fsm.GetFirstActionOfType<SendEventByName>("Firebat 1 Sub").sendEvent = "LOW";
            fsm.GetFirstActionOfType<Wait>("Firebat 1").time = 0;
            fsm.GetFirstActionOfType<Wait>("Firebat 1 Sub").time = Constants.BatCooldown;

            fsm.RemoveAction("Firebat 2", 4);
            fsm.InsertCustomAction("Firebat 2", () =>
            {
                fsm.FsmVariables.GetFsmGameObject("Projectile").Value.LocateMyFSM("Control").SendEvent("MID");
            }, 4);
            fsm.GetFirstActionOfType<Wait>("Firebat 2").time = Constants.BatCooldown;

            fsm.CopyState("Firebat 3", "Firebat 3 Sub");
            fsm.ChangeTransition("Firebat 3", "FINISHED", "Firebat 3 Sub");
            fsm.GetFirstActionOfType<SendEventByName>("Firebat 3 Sub").sendEvent = "LOW";
            fsm.GetFirstActionOfType<Wait>("Firebat 3").time = 0;
            fsm.GetFirstActionOfType<Wait>("Firebat 3 Sub").time = Constants.BatCooldown;

            fsm.RemoveAction("Firebat 4", 7);
            fsm.InsertCustomAction("Firebat 4", () =>
            {
                fsm.FsmVariables.GetFsmGameObject("Projectile").Value.LocateMyFSM("Control").SendEvent("MID");
            }, 7);
            fsm.GetFirstActionOfType<Wait>("Firebat 4").time = Constants.BatCooldown;

            fsm.GetFirstActionOfType<Wait>("FB Cast End").time = Constants.AfterBats;
        }

        public void Batter(string s)
        {
            fsm.InsertCustomAction(s, () =>
            {
                GameObject bat = fsm.FsmVariables.GetFsmGameObject("Projectile").Value;
                PlayMakerFSM batfsm = bat.LocateMyFSM("Control");
                batfsm.CopyState("High", "Low");
                fsm.AddTransition("Init", "LOW", "Low");
                batfsm.GetFirstActionOfType<SetVector3XYZ>("Low").y = new FsmFloat { Value = 3 };
                batfsm.GetFirstActionOfType<SetVector3XYZ>("High").y = new FsmFloat { Value = 6 };
                batfsm.GetFirstActionOfType<SetVector3XYZ>("Mid").y = new FsmFloat { Value = 3 };
            }, 3);
        }
    }
}