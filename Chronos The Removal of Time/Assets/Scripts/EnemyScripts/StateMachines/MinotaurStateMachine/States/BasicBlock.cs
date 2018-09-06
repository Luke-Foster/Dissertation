using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBlock<T> : BasicAIState<T>
{
    public BasicBlock(T stateName, BasicStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();
        brain.MinotaurAnimController.SetBool("BlockAnim", true);
        brain.BlockDuration = 0.0f;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        brain.MinotaurAnimController.SetBool("BlockAnim", false);
        brain.ClickCount = 0;
        brain.BlockDuration = 0.0f;
        brain.Block = false;
        brain.Blocking = false;
    }

    public override void Act()
    {
        Debug.Log("Blocking");
        brain.Blocking = true;
        brain.BlockDuration += Time.deltaTime;
    }
}
