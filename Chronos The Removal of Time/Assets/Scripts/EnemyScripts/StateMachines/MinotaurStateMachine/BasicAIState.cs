using UnityEngine;
using System.Collections;

// The AIState script inherits from the State Machine
public class BasicAIState<T> : State<T> {
	// Sets a variable to enable referring to StateDrivenBrain script as brain
	protected BasicStateDrivenBrain brain;

	// Sets up FSM
	public BasicAIState(T stateName, BasicStateDrivenBrain brain, float minDuration): base(stateName, brain, minDuration) 
	{
		this.brain = brain;
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
