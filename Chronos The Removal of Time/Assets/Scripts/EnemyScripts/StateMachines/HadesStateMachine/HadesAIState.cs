using System.Collections;
using UnityEngine;

public class HadesAIState<T> : State<T>
{
    protected HadesStateDrivenBrain HadesBrain;

    // Sets up FSM
    public HadesAIState(T stateName, HadesStateDrivenBrain HadesBrain, float minDuration): base(stateName, HadesBrain, minDuration) 
	{
        this.HadesBrain = HadesBrain;
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
