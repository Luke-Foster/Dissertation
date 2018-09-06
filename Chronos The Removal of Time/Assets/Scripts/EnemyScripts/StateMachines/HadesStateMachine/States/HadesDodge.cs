using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesDodge<T> : HadesAIState<T>
{
    public HadesDodge(T stateName, HadesStateDrivenBrain controller, float minDuration) : base(stateName, controller, minDuration) { }

    int randomDirection;
    float delay = 0.25f;

    public override void OnEnter()
    {
        base.OnEnter();
        randomDirection = Random.Range(1, 3);
        delay = 0.25f;
    }

    public override void OnLeave()
    {
        base.OnLeave();
        HadesBrain.ClickCount = 0;
    }

    public override void Act()
    {
        delay -= Time.deltaTime;

        //Moves Hades in random direction of dodge
        if (randomDirection == 1)
            HadesBrain.transform.Translate(Vector3.left * Time.deltaTime * 40.0f);
        else if (randomDirection == 2)
            HadesBrain.transform.Translate(Vector3.right * Time.deltaTime * 40.0f);


        if (delay < 0)
        {
            HadesBrain.Dodge = false;
        }
    }
}
