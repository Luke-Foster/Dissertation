using System.Collections;
using UnityEngine;

public class HoundMove<T> : HoundAIState<T>
{
    public HoundMove(T stateName, HoundStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

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
        Vector3 TargetPosition = new Vector3(HoundBrain.ChronosTransform.position.x, HoundBrain.transform.position.y, HoundBrain.ChronosTransform.position.z);
        HoundBrain.transform.LookAt(TargetPosition);
        HoundBrain.transform.Translate(Vector3.forward * (3 - HoundBrain.MovementSlow) * Time.deltaTime);
    }
}
