using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMelee<T> : BasicAIState<T>
{
    public BasicMelee(T stateName, BasicStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();
        brain.MinotaurAnimController.SetBool("MeleeAnim", true);
        brain.MeleeDuration = 0.0f;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        brain.MinotaurAnimController.SetBool("MeleeAnim", false);
        brain.MeleeDuration = 0.0f;
        brain.InMeleeRange = false;
    }

    public override void Act()
    {
        brain.MeleeDuration += Time.deltaTime;
        Vector3 TargetPosition = new Vector3(brain.ChronosTransform.position.x, brain.transform.position.y, brain.ChronosTransform.position.z);
        brain.transform.LookAt(TargetPosition);

        if (brain.MeleeDuration >= (0.7000f + brain.MeleeContactIncrease) && brain.MeleeDuration <= (0.7666f + brain.MeleeContactIncrease))
        {
            Debug.Log("Minotaur Melee Contact");
            brain.MeleeContact = true;
        }
        else
        {
            brain.MeleeContact = false;
        }
    }
}
