using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FreezeTimeAbility : MonoBehaviour
{
    float sensitivityY;
    public GameObject FreezeAbilityPrefab;
    public Transform FreezeAbilitySpawn;
    public Transform ChronosTransform;
    public GameObject FreezeCircle;
    float AbilityDuration = 0.0f;
    AbilityTriggerDetection ATD;
    AbilityTriggerDetection HoundATD;
    AbilityTriggerDetection HadesATD;
    public GameObject hellMinotaur;
    GameObject hellHound;
    GameObject Hades;
    bool InitiateFreeze = false;
    PlayerController PC;
    public GameObject ThreeActive;
    public GameObject ThreeCoolDownTextActive;
    public Text ThreeCoolDownText;
    float Cooldown = 8.0f;
    bool CooldownActive = false;
    public bool IsFreezeAbility = false;
    float FreezeDuration = 0.0f;
    public bool IsEnabled = false;
    bool PlaceAbility = false;
    Scene currentScene;
    string sceneName;

    void Start ()
    {
        sensitivityY = 3.0f;
        hellMinotaur = GameObject.Find("HellMinotaur");
        hellHound = GameObject.Find("HellHoundModel");
        ATD = hellMinotaur.GetComponent<AbilityTriggerDetection>();   // Initialises the reference to AbilityTriggerDetection script through the hellMinotaur GameObject
        HoundATD = hellHound.GetComponent<AbilityTriggerDetection>();
        PC = GetComponent<PlayerController>();
        PlaceAbility = false;
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }
	
	void Update ()
    {
        if (PlaceAbility == false && PC.grounded == true && CooldownActive == false)
        {
            
            if (Input.GetKeyDown("3"))    // Returns true when 3 is first pressed
            {
                FreezeCircle = (GameObject)Instantiate(FreezeAbilityPrefab, FreezeAbilitySpawn.position, ChronosTransform.rotation);   // Instantiates FreezeAbilityAOE Prefab
                FreezeCircle.GetComponent<Renderer>().enabled = true;    
                AbilityDuration = 0.0f;
                Cooldown = 8.0f;
                ThreeActive.SetActive(true);
            }

            if (Input.GetKey("3"))
            {
                if(FreezeCircle != null)
                    FreezeCircle.transform.parent = ChronosTransform;       // Prefab becomes a child object of Chronos

                IsEnabled = false; 

                float distance = Vector3.Distance(FreezeCircle.transform.position, ChronosTransform.position);   // Calculates the distance between Chronos and the prefab
                Vector3 forward = ChronosTransform.transform.TransformDirection(Vector3.forward);   // Finds direction Chronos is facing
                Vector3 toOther = FreezeCircle.transform.position - ChronosTransform.position;      // Finds magnitude

                if (distance < 30.00f && Vector3.Dot(forward, toOther) > 0)         // Makes sure you can only move prefab when it's in positive direction and up to 30 in distance 
                    FreezeCircle.transform.Translate(Vector3.forward * Input.GetAxis("Mouse Y") * sensitivityY);
                else if (Input.GetAxis("Mouse Y") > 0 && distance < 30.00f && Vector3.Dot(forward, toOther) >= 0)    // Can only move mouse up when prefab is at Chronos' feet 
                    FreezeCircle.transform.Translate(Vector3.forward * Input.GetAxis("Mouse Y") * sensitivityY);
                else if (Input.GetAxis("Mouse Y") < 0 && distance >= 30.00f && Vector3.Dot(forward, toOther) > 0)    // Can only move mouse down when prefab is at a distance of 30  
                    FreezeCircle.transform.Translate(Vector3.forward * Input.GetAxis("Mouse Y") * sensitivityY);
                else if (distance >= 30.00f)      // Ensures prefab stays at 30 if it ends up over 30 distance
                    distance = 30.00f;
                else if (Vector3.Dot(forward, toOther) < 0)   // Ensures prefab stays at Chronos' feet if it were to go negative
                    FreezeCircle.transform.position = ChronosTransform.position;
                else
                    FreezeCircle.transform.position = ChronosTransform.position;
            }

            if (Input.GetKeyUp("3"))
            {
                if (FreezeCircle != null)
                    FreezeCircle.transform.parent = null;   // Detaches prefab from Chronos 
                PC.StopAnim = true;
                PC.ChronosAnimController.SetBool("Freeze", true);
                InitiateFreeze = true;
                PlaceAbility = true;      // Ability has been placed now
            }
        }
        else
        {
            if (InitiateFreeze == true)
            {
                AbilityDuration += Time.deltaTime;

                if (AbilityDuration >= 1.0f)
                {
                    FreezeDuration = 0.0f;
                    IsEnabled = true;
                    FreezeCircle.GetComponent<Renderer>().enabled = false;     // Hides prefab
                    IsFreezeAbility = true;   
                    CooldownActive = true;
                    PC.StopAnim = false;
                    PC.ChronosAnimController.SetBool("Freeze", false);
                    AbilityDuration = 0.0f;
                }
            }
        }

        if (IsFreezeAbility == true)
        {
            InitiateFreeze = false;

            FreezeDuration += Time.deltaTime;
            if (FreezeDuration >= 3.0f)
            {
                hellMinotaur = GameObject.Find("HellMinotaur");
                if (hellMinotaur != null)
                    ATD = hellMinotaur.GetComponent<AbilityTriggerDetection>();
                PlaceAbility = false;

                if (ATD.minotaurHit == true && hellMinotaur != null && HoundATD.HoundHit == false)
                {
                    ATD.BSDB.enabled = true;
                    ATD.BSDB.MinotaurAnimController.enabled = true;
                }

                if (sceneName == "BossBattle")
                {
                    Hades = GameObject.Find("HadesModel");
                    if(Hades != null)
                        HadesATD = Hades.GetComponent<AbilityTriggerDetection>();

                    if (HoundATD.HoundHit == true && hellHound != null)
                        HoundATD.HSDB.enabled = true;

                    if (HadesATD.HadesHit == true && Hades != null)
                    {
                        HadesATD.HadesSDB.enabled = true;
                        HadesATD.HadesSDB.HadesAnimController.enabled = true;
                    }

                    HadesATD.HadesHit = false;
                }

                Destroy(FreezeCircle);

                ATD.minotaurHit = false;
                HoundATD.HoundHit = false;
                IsFreezeAbility = false;
            }
        }

        if (CooldownActive == true)
        {
            ThreeCoolDownTextActive.SetActive(true);
            Cooldown -= Time.deltaTime;
            string CooldownToString = ((int)Cooldown + 1).ToString();
            ThreeCoolDownText.text = CooldownToString;

            if (Cooldown <= 0.0f)
            {
                ThreeActive.SetActive(false);
                ThreeCoolDownTextActive.SetActive(false);
                CooldownActive = false;
            }
        }
    }
}
