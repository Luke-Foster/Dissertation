using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundMelee<T> : HoundAIState<T>
{
    public HoundMelee(T stateName, HoundStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();
        HoundBrain.MeleeDuration = 0.0f;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        HoundBrain.MeleeDuration = 0.0f;
        HoundBrain.InMeleeRange = false;
    }

    public override void Act()
    {
        HoundBrain.MeleeDuration += Time.deltaTime;
        Vector3 TargetPosition = new Vector3(HoundBrain.ChronosTransform.position.x, HoundBrain.transform.position.y, HoundBrain.ChronosTransform.position.z);
        HoundBrain.transform.LookAt(TargetPosition);

        if (HoundBrain.MeleeDuration >= (0.8f + HoundBrain.MeleeContactIncrease) && HoundBrain.MeleeDuration <= (1.0f + HoundBrain.MeleeContactIncrease))
        {
            Debug.Log("Hound Melee Contact");
            HoundBrain.MeleeContact = true;
        }
        else
        {
            HoundBrain.MeleeContact = false;
        }
    }
}
