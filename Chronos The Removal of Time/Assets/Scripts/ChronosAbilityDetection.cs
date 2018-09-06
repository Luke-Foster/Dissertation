using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronosAbilityDetection : MonoBehaviour
{
    GameObject Hades;
    GameObject Chronos;
    HadesStateDrivenBrain HadesSDB;
    PlayerController PC;
    HealthManager HM;
    public bool RootEffect = false;
    public bool DamageOnce = false;
    public float RootDuration = 0.0f;

    void Start()
    {
        Hades = GameObject.Find("Hades");
        HadesSDB = Hades.GetComponent<HadesStateDrivenBrain>();
        Chronos = GameObject.Find("Chronos");
        HM = Chronos.GetComponent<HealthManager>();
        PC = Chronos.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "HadesAbilityAOE(Clone)")
        {
            HadesSDB.AbilityHittingChronos = true;
            Debug.Log("Ability hitting chronos");
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name == "HadesAbilityAOE(Clone)")
        {
            HadesSDB.AbilityHittingChronos = true;
            Debug.Log("Ability hitting chronos");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "HadesAbilityAOE(Clone)")
        {
            HadesSDB.AbilityHittingChronos = false;
        }
    }

    void Update()
    {
        if (HadesSDB.AbilityDamage == true && DamageOnce == false)
        {
            Debug.Log("Damage being dealt");
            HM.ChronosHealth -= 20;
            RootEffect = true;
            RootDuration = 0.0f;
            HadesSDB.RootCircle.GetComponent<Renderer>().enabled = false;
            DamageOnce = true;
        }

        if(RootEffect == true)
        {
            RootDuration += Time.deltaTime;

            if (RootDuration < 3.0f)
                PC.RootActive = true;

            if(RootDuration >= 3.0f)
            {
                PC.RootActive = false;
                HadesSDB.AbilityDamage = false;
                Destroy(HadesSDB.RootCircle);
                DamageOnce = false;
                RootEffect = false;
            }
        }
    }
}
