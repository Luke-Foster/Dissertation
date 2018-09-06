using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesAbility<T> : HadesAIState<T>
{
    public HadesAbility(T stateName, HadesStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    public override void OnEnter()
    {
        base.OnEnter();
        HadesBrain.HadesAnimController.SetBool("AbilityAnim", true);
        Debug.Log("Entered ability state");
        HadesBrain.AbilityDelay = 0.0f;
        HadesBrain.RootCircle = (GameObject)UnityEngine.Object.Instantiate(HadesBrain.RootAbilityPrefab, HadesBrain.ChronosTransform.position, HadesBrain.ChronosTransform.rotation);   // Instantiates FreezeAbilityAOE Prefab
        HadesBrain.RootCircle.GetComponent<Renderer>().enabled = true;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        HadesBrain.HadesAnimController.SetBool("AbilityAnim", false);
        HadesBrain.randomAbilityTimer = 0.0f;
        HadesBrain.randomAbility = Random.Range(10, 16);
        HadesBrain.AbilityDelay = 0.0f;
    }

    public override void Act()
    {
        HadesBrain.AbilityDelay += Time.deltaTime;

        if (HadesBrain.AbilityDelay >= 1.8f && HadesBrain.AbilityHittingChronos == true)
        {
            HadesBrain.AbilityDamage = true;
            Debug.Log("Chronos on ability when detonated");
            HadesBrain.AbilityActive = false;
        }

        if (HadesBrain.AbilityDelay >= 1.8f && HadesBrain.AbilityHittingChronos == false)
        {
            HadesBrain.AbilityDamage = false;
            Debug.Log("Chronos is not on ability when detonated");
            UnityEngine.Object.Destroy(HadesBrain.RootCircle);
            HadesBrain.AbilityActive = false;
        }
    }
}
