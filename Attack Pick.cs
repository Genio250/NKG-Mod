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
using SFCore;


namespace NKG
{
    public partial class NKG
    {
        public void Attack()
        {
            var Bat = FsmEvent.GetFsmEvent("FIREBATS");
            var Slash = FsmEvent.GetFsmEvent("SLASH");
            var AirDash = FsmEvent.GetFsmEvent("AIR DASH");
            var Spikes = FsmEvent.GetFsmEvent("SPIKES");
            var Pillars = FsmEvent.GetFsmEvent("PILLARS");
            var Dash = FsmEvent.GetFsmEvent("DASH");

            fsm.GetValidState("Move Choice").GetFirstActionOfType<BoolTest>().isFalse = Dash;

            FsmInt DashInt = new FsmInt
            {
                Name = "Ct Dash",
                Value = 0
            };

            fsm.FsmVariables.IntVariables = fsm.FsmVariables.IntVariables
                .Concat(new[] { DashInt })
                .ToArray();

            FsmInt DashIntMiss = new FsmInt
            {
                Name = "Ms Dash",
                Value = 0
            };

            fsm.FsmVariables.IntVariables = fsm.FsmVariables.IntVariables
                .Concat(new[] { DashIntMiss })
                .ToArray();

            fsm.GetValidState("Move Choice").RemoveAction(2);
            fsm.GetValidState("Move Choice").AddAction(new SendRandomEventV3
            {
                events = new FsmEvent[6]
                {
                    Bat, Slash, AirDash, Spikes, Pillars, Dash
                },
                weights = new FsmFloat[6]
                {
                    1, 1, 1, 1, 1, 1
                },
                trackingInts = new FsmInt[6]
                {
                    fsm.FsmVariables.GetFsmInt("Ct Firebats"),
                    fsm.FsmVariables.GetFsmInt("Ct Slash"),
                    fsm.FsmVariables.GetFsmInt("Ct AirDash"),
                    fsm.FsmVariables.GetFsmInt("Ct Spikes"),
                    fsm.FsmVariables.GetFsmInt("Ct Pillar"),
                    fsm.FsmVariables.GetFsmInt("Ct Dash"),
                },
                eventMax = new FsmInt[6]
                {
                    2, 3, 2, 1, 1, 2
                },
                trackingIntsMissed = new FsmInt[6]
                {
                    fsm.FsmVariables.GetFsmInt("Ms Firebats"),
                    fsm.FsmVariables.GetFsmInt("Ms Slash"),
                    fsm.FsmVariables.GetFsmInt("Ms AirDash"),
                    fsm.FsmVariables.GetFsmInt("Ms Spikes"),
                    fsm.FsmVariables.GetFsmInt("Ms Pillar"),
                    fsm.FsmVariables.GetFsmInt("Ms Dash"),
                },
                missedMax = new FsmInt[6]
                {
                    5, 4, 5, 6, 5, 4
                }
            });
        }
    }
}