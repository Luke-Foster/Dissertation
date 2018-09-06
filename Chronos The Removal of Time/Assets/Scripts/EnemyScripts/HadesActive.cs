using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HadesActive : MonoBehaviour
{
    HadesStateDrivenBrain HadesSDB;
    GameObject Hades;
    GameObject Chronos;
    FreezeTimeAbility FTA;
    HadesRewind hadesRewind;
    Scene currentScene;
    string sceneName;
    public GameObject HealthBar;
    bool ReturnToSpawn = false;
    public Transform HadesSpawn;

    void Start ()
    {
        Hades = GameObject.Find("Hades");
        Chronos = GameObject.Find("Chronos");
        HadesSDB = Hades.GetComponent<HadesStateDrivenBrain>();
        FTA = Chronos.GetComponent<FreezeTimeAbility>();
        hadesRewind = Hades.GetComponent<HadesRewind>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == ("Chronos") && Hades != null)
        {
            HadesSDB.enabled = true;
            HealthBar.SetActive(true);
            HadesSDB.HadesAnimController.SetBool("BrainActiveAnim", true);
            ReturnToSpawn = false;
            Debug.Log("Hades brain is active");
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == ("Chronos") && Hades != null && FTA.IsFreezeAbility == false)
        {
            if (hadesRewind.RewindActive == false)
            {
                HadesSDB.enabled = true;
                HadesSDB.HadesAnimController.SetBool("BrainActiveAnim", true);
            }
                
            ReturnToSpawn = false;
            HealthBar.SetActive(true);
        }

        if (Hades == null)
            HealthBar.SetActive(false);
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == ("Chronos"))
        {
            if (Hades != null)
            {
                HadesSDB.enabled = false;
                ReturnToSpawn = true;
                Debug.Log("Hades brain has been disabled");
            }
            HealthBar.SetActive(false);
        }
    }

    void Update()
    {
        if (Hades != null)
        {
            HadesSpawn = GameObject.Find("HadesSpawn").transform;

            float distance = Vector3.Distance(Hades.transform.position, HadesSpawn.position);

            if (ReturnToSpawn == true)
            {
                if (distance >= 0 && distance <= 0.100000f)
                {
                    Hades.transform.rotation = Quaternion.RotateTowards(Hades.transform.rotation, HadesSpawn.rotation, 2.0f);
                    HadesSDB.HadesAnimController.SetBool("BrainActiveAnim", false);
                }
                else
                {
                    Vector3 TargetPosition = new Vector3(HadesSpawn.position.x, HadesSpawn.transform.position.y, HadesSpawn.position.z);
                    Hades.transform.LookAt(TargetPosition);
                    Hades.transform.Translate(Vector3.forward * (3 - HadesSDB.MovementSlow) * Time.deltaTime);
                    HadesSDB.HadesAnimController.SetBool("MeleeAnim", false);
                    HadesSDB.HadesAnimController.SetBool("RangedAnim", false);
                    HadesSDB.HadesAnimController.SetBool("BlockAnim", false);
                    HadesSDB.HadesAnimController.SetBool("AbilityAnim", false);
                }
            }
        }
        else
        {
            HealthBar.SetActive(false);
        }
    }
}
