using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesMove<T> : HadesAIState<T>
{
    public HadesMove(T stateName, HadesStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }

    public override void Act()
    {
        Vector3 TargetPosition = new Vector3(HadesBrain.ChronosTransform.position.x, HadesBrain.transform.position.y, HadesBrain.ChronosTransform.position.z);
        HadesBrain.transform.LookAt(TargetPosition);
        HadesBrain.transform.Translate(Vector3.forward * (3 - HadesBrain.MovementSlow) * Time.deltaTime);
    }
}
