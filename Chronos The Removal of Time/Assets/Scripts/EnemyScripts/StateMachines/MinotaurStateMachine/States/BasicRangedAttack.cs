using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangedAttack<T> : BasicAIState<T>
{
    public BasicRangedAttack(T stateName, BasicStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    bool SpawnOnce;

    public override void OnEnter()
    {
        base.OnEnter();
        brain.MinotaurAnimController.SetBool("RangedAnim", true);
        brain.RangedAttackDuration = 0.0f;
        SpawnOnce = false;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        brain.MinotaurAnimController.SetBool("RangedAnim", false);
        brain.randomTimer = 0.0f;
        brain.RangedAttackActive = false;
        brain.randomRangedAttackValue = Random.Range(4.0f, 8.0f);
    }

    public override void Act()
    {
        brain.RangedAttackDuration += Time.deltaTime;

        if (brain.Projectile == null && brain.RangedAttackDuration >= 0.73f && brain.RangedAttackDuration <= 1.32f && SpawnOnce == false)
        {
            brain.Projectile = (GameObject)UnityEngine.Object.Instantiate(brain.ProjectilePrefab, brain.ProjectileSpawn.position, brain.ProjectileSpawn.rotation);
            SpawnOnce = true;
        }

        if (brain.RangedAttackDuration > (0.73f + brain.RangedAttackDurationIncrease) && brain.RangedAttackDuration < (1.33f + brain.RangedAttackDurationIncrease) && brain.Projectile != null)
            brain.Projectile.transform.Translate(Vector3.forward * Time.deltaTime * (40.0f - brain.RangedAttackSlow));

        if (brain.Projectile != null && brain.RangedAttackDuration > (1.33f + brain.RangedAttackDurationIncrease))
            UnityEngine.Object.Destroy(brain.Projectile);
    }
}
