using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AbilityTriggerDetection : MonoBehaviour
{
    GameObject minotaur;
    GameObject chronos;
    GameObject HellHound;
    GameObject Hades;
    public BasicStateDrivenBrain BSDB;
    public HoundStateDrivenBrain HSDB;
    public HadesStateDrivenBrain HadesSDB;
    FreezeTimeAbility FTA;
    SlowTimeAbility STA;
    public HealthManager HM; 
    public bool minotaurHit = false;
    public bool HoundHit = false;
    public bool HadesHit = false;
    Scene currentScene;
    string sceneName;

    void Start()
    {
        //Initialises Minotaur brain, Health Manager and Freeze time ability
        minotaur = GameObject.FindGameObjectWithTag("Enemy");
        chronos = GameObject.Find("Chronos");
        HellHound = GameObject.Find("HellHound");
        BSDB = minotaur.GetComponent<BasicStateDrivenBrain>();
        FTA = chronos.GetComponent<FreezeTimeAbility>();
        STA = chronos.GetComponent<SlowTimeAbility>();
        HM = minotaur.GetComponent<HealthManager>();
        HSDB = HellHound.GetComponent<HoundStateDrivenBrain>();

        if (sceneName == "BossBattle")
        {
            Hades = GameObject.Find("Hades");
            HadesSDB = Hades.GetComponent<HadesStateDrivenBrain>();
        }

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        minotaurHit = false;
        HoundHit = false;
        HadesHit = false;
    }

    void OnTriggerStay(Collider col)
    {
        if (this.gameObject.name == ("HellMinotaur") && col.gameObject.name == ("FreezeAbilityAOE(Clone)") && FTA.IsEnabled == true && FTA.IsFreezeAbility == true)
        {
            Debug.Log("Minotaur has been frozen");
            minotaur.GetComponent<HealthManager>().MinotaurHealth -= 20;    // Reduces Minotaur health by 20
            BSDB.MinotaurAnimController.enabled = false;
            BSDB.enabled = false;        // Disables Minotaur brain
            minotaurHit = true;          // Reads that the Minotaur has been hit 
            FTA.IsEnabled = false;
        }

        if (this.gameObject.name == ("HellMinotaur") && col.gameObject.name == ("SlowAbilityAOE(Clone)") && STA.IsEnabled == true && STA.IsSlowAbility == true)
        {
            Debug.Log("Minotaur has been slowed");
            minotaur.GetComponent<HealthManager>().MinotaurHealth -= 10;    // Reduces Minotaur health by 10
            // Changes variable in BSDB to slow AI
            BSDB.MovementSlow = 1.5f;
            BSDB.RangedAttackSlow = 20.0f;
            BSDB.RangedAttackDurationIncrease = 0.6f;
            BSDB.MeleeDurationIncrease = 1.2f;
            BSDB.MeleeContactIncrease = 1.0f;
            BSDB.BlockDurationIncrease = 0.8f;
            BSDB.BlockDelayIncrease = 0.2f;
            BSDB.MinotaurAnimController.speed = 0.5f;
            STA.SlowCircle.transform.parent = minotaur.transform;
            minotaurHit = true;          // Reads that the Minotaur has been hit 
            STA.IsEnabled = false;
        }

        if (this.gameObject.name == ("HellHoundModel") && col.gameObject.name == ("FreezeAbilityAOE(Clone)") && FTA.IsEnabled == true && FTA.IsFreezeAbility == true)
        {
            Debug.Log("Hound has been frozen");
            HellHound.GetComponent<HealthManager>().HoundHealth -= 20;    // Reduces Hound health by 20

            if(sceneName == "BossBattle")
            {
                HSDB.enabled = false;
                HoundHit = true;
            }

            FTA.IsEnabled = false;
        }

        if (this.gameObject.name == ("HellHoundModel") && col.gameObject.name == ("SlowAbilityAOE(Clone)") && STA.IsEnabled == true && STA.IsSlowAbility == true)
        {
            Debug.Log("Hound has been slowed");
            if(sceneName == "BossBattle")
            {
                HellHound.GetComponent<HealthManager>().HoundHealth -= 10;    // Reduces Minotaur health by 10
                HSDB.MovementSlow = 1.5f;
                HSDB.MeleeDurationIncrease = 1.2f;
                HSDB.MeleeContactIncrease = 1.0f;
            }
            STA.SlowCircle.transform.parent = HellHound.transform;
            HoundHit = true;
            STA.IsEnabled = false;
        }


        if (this.gameObject.name == ("HadesModel") && col.gameObject.name == ("FreezeAbilityAOE(Clone)") && FTA.IsEnabled == true && FTA.IsFreezeAbility == true)
        {
            Debug.Log("Hades has been frozen");
            Hades = GameObject.Find("Hades");
            Hades.GetComponent<HealthManager>().HadesHealth -= 20;    // Reduces Hades health by 20
            HadesSDB = Hades.GetComponent<HadesStateDrivenBrain>();
            HadesSDB.HadesAnimController.enabled = false;
            HadesSDB.enabled = false;        // Disables Hades brain
            HadesHit = true;          // Reads that Hades has been hit 
            FTA.IsEnabled = false;
        }

        if (this.gameObject.name == ("HadesModel") && col.gameObject.name == ("SlowAbilityAOE(Clone)") && STA.IsEnabled == true && STA.IsSlowAbility == true)
        {
            Debug.Log("Hades has been slowed");
            Hades = GameObject.Find("Hades");
            Hades.GetComponent<HealthManager>().HadesHealth -= 10;    // Reduces Hades health by 10
            // Changes variable in HadesSDB to slow AI
            HadesSDB = Hades.GetComponent<HadesStateDrivenBrain>();
            HadesSDB.MovementSlow = 1.5f;
            HadesSDB.RangedAttackSlow = 20.0f;
            HadesSDB.RangedAttackDurationIncrease = 0.6f;
            HadesSDB.MeleeDurationIncrease = 1.2f;
            HadesSDB.MeleeContactIncrease = 1.0f;
            HadesSDB.BlockDurationIncrease = 0.8f;
            HadesSDB.BlockDelayIncrease = 0.2f;
            HadesSDB.HadesAnimController.speed = 0.5f;
            STA.SlowCircle.transform.parent = Hades.transform;
            HadesHit = true;          // Reads that the Hades has been hit 
            STA.IsEnabled = false;
        }
    }
}
