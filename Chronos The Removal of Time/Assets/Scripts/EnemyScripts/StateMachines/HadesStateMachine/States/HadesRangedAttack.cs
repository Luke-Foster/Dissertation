using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesRangedAttack<T> : HadesAIState<T>
{
    public HadesRangedAttack(T stateName, HadesStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    bool SpawnOnce;

    public override void OnEnter()
    {
        base.OnEnter();
        HadesBrain.HadesAnimController.SetBool("RangedAnim", true);
        HadesBrain.RangedAttackDuration = 0.0f;
        SpawnOnce = false;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        HadesBrain.HadesAnimController.SetBool("RangedAnim", false);
        HadesBrain.randomTimer = 0.0f;
        HadesBrain.RangedAttackActive = false;
        HadesBrain.randomRangedAttackValue = Random.Range(4.0f, 8.0f);
    }

    public override void Act()
    {
        HadesBrain.RangedAttackDuration += Time.deltaTime;

        if (HadesBrain.Projectile == null && HadesBrain.RangedAttackDuration >= 1.0f && SpawnOnce == false)
        {
            HadesBrain.Projectile = (GameObject)UnityEngine.Object.Instantiate(HadesBrain.ProjectilePrefab, HadesBrain.ProjectileSpawn.position, HadesBrain.ProjectileSpawn.rotation);
            SpawnOnce = true;
        }

        if (HadesBrain.RangedAttackDuration > (1.0f + HadesBrain.RangedAttackDurationIncrease) && HadesBrain.RangedAttackDuration < (1.6f + HadesBrain.RangedAttackDurationIncrease) && HadesBrain.Projectile != null)
            HadesBrain.Projectile.transform.Translate(Vector3.forward * Time.deltaTime * (40.0f - HadesBrain.RangedAttackSlow));

        if (HadesBrain.Projectile != null && HadesBrain.RangedAttackDuration > (1.6f + HadesBrain.RangedAttackDurationIncrease))
            UnityEngine.Object.Destroy(HadesBrain.Projectile);
    }
}
