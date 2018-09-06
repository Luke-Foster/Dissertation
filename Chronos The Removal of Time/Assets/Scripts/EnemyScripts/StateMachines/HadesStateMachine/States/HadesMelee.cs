using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesMelee<T> : HadesAIState<T>
{
    public HadesMelee(T stateName, HadesStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();
        HadesBrain.HadesAnimController.SetBool("MeleeAnim", true);
        HadesBrain.MeleeDuration = 0.0f;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        HadesBrain.HadesAnimController.SetBool("MeleeAnim", false);
        HadesBrain.MeleeDuration = 0.0f;
        HadesBrain.InMeleeRange = false;
    }

    public override void Act()
    {
        HadesBrain.MeleeDuration += Time.deltaTime;
        Vector3 TargetPosition = new Vector3(HadesBrain.ChronosTransform.position.x, HadesBrain.transform.position.y, HadesBrain.ChronosTransform.position.z);
        HadesBrain.transform.LookAt(TargetPosition);

        if (HadesBrain.MeleeDuration >= (1.066f + HadesBrain.MeleeContactIncrease) && HadesBrain.MeleeDuration <= (1.133f + HadesBrain.MeleeContactIncrease))
        {
            Debug.Log("Hades Melee Contact");
            HadesBrain.MeleeContact = true;
        }
        else
        {
            HadesBrain.MeleeContact = false;
        }
    }
}
