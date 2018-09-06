using System.Collections;
using UnityEngine;

public class HoundAIState<T> : State<T>
{
    protected HoundStateDrivenBrain HoundBrain;

    public HoundAIState(T stateName, HoundStateDrivenBrain HoundBrain, float minDuration): base(stateName, HoundBrain, minDuration) 
	{
        this.HoundBrain = HoundBrain;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }

    public override void OnStateTriggerEnter(Collider collider)
    {

    }

    public override void Monitor()
    {

    }

    public override void Act()
    {

    }
}
