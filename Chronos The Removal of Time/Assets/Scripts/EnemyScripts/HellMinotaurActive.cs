using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HellMinotaurActive : MonoBehaviour
{
    BasicStateDrivenBrain BSDB;
    HealthManager HM;
    GameObject HellMinotaur;
    GameObject Chronos;
    FreezeTimeAbility FTA;
    MinotaurRewind minotaurRewind;
    Scene currentScene;
    string sceneName;
    public GameObject HealthBar;
    public GameObject MinotaurPrefab;
    public Transform MinotaurSpawn;
    bool ReturnToSpawn = false;
    public float RespawnTimer;
    bool RespawnOnce = false;
    public bool ChronosRegenerates = false;

    void Start ()
    {
        HellMinotaur = GameObject.Find("Minotaur");
        Chronos = GameObject.Find("Chronos");
        ReturnToSpawn = false;
        BSDB = HellMinotaur.GetComponent<BasicStateDrivenBrain>();
        HM = HellMinotaur.GetComponent<HealthManager>();
        FTA = Chronos.GetComponent<FreezeTimeAbility>();
        minotaurRewind = HellMinotaur.GetComponent<MinotaurRewind>();
        RespawnTimer = 4.0f;
    }
	
	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == ("Chronos") && HellMinotaur != null)
        {
            BSDB.enabled = true;
            HealthBar.SetActive(true);
            BSDB.MinotaurAnimController.SetBool("BrainActiveAnim", true);
            ReturnToSpawn = false;
            Debug.Log("Minotaur brain is active");
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == ("Chronos") && HellMinotaur != null && FTA.IsFreezeAbility == false)
        {
            if(minotaurRewind.RewindActive == false)
            {
                BSDB.enabled = true;
                BSDB.MinotaurAnimController.SetBool("BrainActiveAnim", true);
            }
                
            HealthBar.SetActive(true);
            ReturnToSpawn = false;
        }

        if (col.gameObject.name == ("Chronos"))
            ChronosRegenerates = false;
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.name == ("Chronos"))
        { 
            if (HellMinotaur != null)
            {
                BSDB.enabled = false;
                ReturnToSpawn = true;
                Debug.Log("Minotaur brain has been disabled");
            }
            HealthBar.SetActive(false);

            currentScene = SceneManager.GetActiveScene();
            sceneName = currentScene.name;

            if(sceneName == "Tutorial")
                ChronosRegenerates = true;
        }
    }

    void Update()
    {
        if (HellMinotaur != null)
        {
            float distance = Vector3.Distance(HellMinotaur.transform.position, MinotaurSpawn.position);
            RespawnTimer = 4.0f;
            RespawnOnce = false;

            if (ReturnToSpawn == true)
            {
                if (distance >= 0 && distance <= 0.100000f)
                {
                    HellMinotaur.transform.rotation = Quaternion.RotateTowards(HellMinotaur.transform.rotation, MinotaurSpawn.rotation, 2.0f);
                    BSDB.MinotaurAnimController.SetBool("BrainActiveAnim", false);
                }
                else
                {
                    Vector3 TargetPosition = new Vector3(MinotaurSpawn.position.x, HellMinotaur.transform.position.y, MinotaurSpawn.position.z);
                    HellMinotaur.transform.LookAt(TargetPosition);
                    HellMinotaur.transform.Translate(Vector3.forward * (3 - BSDB.MovementSlow) * Time.deltaTime);
                    BSDB.MinotaurAnimController.SetBool("MeleeAnim", false);
                    BSDB.MinotaurAnimController.SetBool("RangedAnim", false);
                    BSDB.MinotaurAnimController.SetBool("BlockAnim", false);
                }
            }
        }
        else
        {
            RespawnTimer -= Time.deltaTime;
            HealthBar.SetActive(false);

            currentScene = SceneManager.GetActiveScene();
            sceneName = currentScene.name;

            if (RespawnTimer <= 0.0f && RespawnOnce == false && sceneName == "Tutorial")
            {
                GameObject NewMinotaur = (GameObject)Instantiate(MinotaurPrefab, MinotaurSpawn.position, MinotaurSpawn.rotation);
                NewMinotaur.name = "Minotaur";
                HellMinotaur = GameObject.Find("Minotaur");
                BSDB = HellMinotaur.GetComponent<BasicStateDrivenBrain>();
                HM = HellMinotaur.GetComponent<HealthManager>();
                HM.MinotaurHealth = HM.MinotaurMAXhealth;
                RespawnOnce = true;
            }
        }
    }
}
