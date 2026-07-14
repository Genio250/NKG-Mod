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
        public void Pillars()
        {
            FsmGameObject Projectile2 = new FsmGameObject
            {
                Name = "Projectile2",
                Value = null
            };

            fsm.FsmVariables.GameObjectVariables = fsm.FsmVariables.GameObjectVariables
                .Concat(new[] { Projectile2 })
                .ToArray();

            Satchel.FsmUtil.GetFirstActionOfType<SetIntValue>(fsm.GetValidState("Pillar Antic")).intValue = 3;
            Satchel.FsmUtil.GetFirstActionOfType<SpawnObjectFromGlobalPool>(fsm.GetValidState("Pillar")).position = new FsmVector3 
                { Value = new Vector3(Constants.PillarSplit, 0, 0) };
            Satchel.FsmUtil.GetFirstActionOfType<Wait>(fsm.GetValidState("Pillar")).time = Constants.PillarCooldown;
            Satchel.FsmUtil.GetFirstActionOfType<Wait>(fsm.GetValidState("Pillar End")).time = Constants.AfterPillar;
            SFCore.Utils.FsmUtil.AddAction(fsm.GetValidState("Pillar"), new SpawnObjectFromGlobalPool
            {
                gameObject = Satchel.FsmUtil.GetFirstActionOfType<SpawnObjectFromGlobalPool>(fsm.GetValidState("Pillar")).gameObject,
                spawnPoint = Knight,
                position = new FsmVector3 { Value = new Vector3(-Constants.PillarSplit, 0, 0) },
                rotation = new FsmVector3 { Value = new Vector3(0, 0, 0) },
                storeObject = Projectile2
            });
            fsm.GetValidState("Pillar").AddCustomAction(() =>
            {
                Projectile2.Value.transform.SetPositionY(fsm.FsmVariables.GetFsmFloat("Ground Y").Value - 3.2f);
            });
        }
    }
}