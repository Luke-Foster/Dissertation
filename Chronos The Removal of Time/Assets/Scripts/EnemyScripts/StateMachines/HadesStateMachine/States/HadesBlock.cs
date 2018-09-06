using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesBlock<T> : HadesAIState<T>
{
    public HadesBlock(T stateName, HadesStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();
        HadesBrain.HadesAnimController.SetBool("BlockAnim", true);
        HadesBrain.BlockDuration = 0.0f;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        HadesBrain.HadesAnimController.SetBool("BlockAnim", false);
        HadesBrain.ClickCount = 0;
        HadesBrain.BlockDuration = 0.0f;
        HadesBrain.Block = false;
        HadesBrain.Blocking = false;
    }

    public override void Act()
    {
        Debug.Log("Blocking");
        HadesBrain.Blocking = true;
        HadesBrain.BlockDuration += Time.deltaTime;
    }
}
