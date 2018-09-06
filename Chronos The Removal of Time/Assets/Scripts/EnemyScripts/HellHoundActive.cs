using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HellHoundActive : MonoBehaviour
{
    public GameObject HealthBar;
    Scene currentScene;
    string sceneName;
    HoundStateDrivenBrain HSDB;
    GameObject hellHound;
    HoundRewind houndRewind;
    FreezeTimeAbility FTA;
    GameObject Chronos;
    bool ReturnToSpawn = false;
    public Transform HoundSpawn;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        Chronos = GameObject.Find("Chronos");
        hellHound = GameObject.Find("HellHound");

        if(sceneName == "BossBattle")
            HSDB = hellHound.GetComponent<HoundStateDrivenBrain>();

        houndRewind = hellHound.GetComponent<HoundRewind>();
        FTA = Chronos.GetComponent<FreezeTimeAbility>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == ("Chronos") && hellHound != null)
        {
            HealthBar.SetActive(true);

            if(sceneName == "BossBattle")
                ReturnToSpawn = false;

            if (sceneName == "BossBattle")
            {
                HSDB.enabled = true;
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == ("Chronos") && hellHound != null && FTA.IsFreezeAbility == false)
        {
            if(sceneName == "BossBattle")
                if (houndRewind.RewindActive == false)
                    HSDB.enabled = true;

            if (sceneName == "BossBattle")
                ReturnToSpawn = false;

            HealthBar.SetActive(true);
        }

        if (hellHound == null)
            HealthBar.SetActive(false);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == ("Chronos"))
        {
            HealthBar.SetActive(false);

            if(hellHound != null)
                ReturnToSpawn = true;

            if (sceneName == "BossBattle")
            {
                if(hellHound != null)
                    HSDB.enabled = false;
            }
        }
    }

    void Update()
    {
        if(sceneName == "BossBattle")
        {
            if (hellHound != null)
            {
                HoundSpawn = GameObject.Find("HoundSpawn").transform;

                float distance = Vector3.Distance(hellHound.transform.position, HoundSpawn.position);

                if (ReturnToSpawn == true)
                {
                    if (distance >= 0 && distance <= 0.100000f)
                    {
                        hellHound.transform.rotation = Quaternion.RotateTowards(hellHound.transform.rotation, HoundSpawn.rotation, 2.0f);
                    }
                    else
                    {
                        Vector3 TargetPosition = new Vector3(HoundSpawn.position.x, hellHound.transform.position.y, HoundSpawn.position.z);
                        hellHound.transform.LookAt(TargetPosition);
                        hellHound.transform.Translate(Vector3.forward * (3 - HSDB.MovementSlow) * Time.deltaTime);
                    }
                }
            }
            else
            {
                HealthBar.SetActive(false);
            }
        }
    }
}
