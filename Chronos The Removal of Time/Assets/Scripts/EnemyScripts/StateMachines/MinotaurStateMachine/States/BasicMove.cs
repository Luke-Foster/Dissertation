using System.Collections;
using UnityEngine;

public class BasicMove<T> : BasicAIState<T>
{
    public BasicMove(T stateName, BasicStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

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
        Vector3 TargetPosition = new Vector3(brain.ChronosTransform.position.x, brain.transform.position.y, brain.ChronosTransform.position.z);
        brain.transform.LookAt(TargetPosition);
        brain.transform.Translate(Vector3.forward * (3 - brain.MovementSlow) * Time.deltaTime);
    }
}
