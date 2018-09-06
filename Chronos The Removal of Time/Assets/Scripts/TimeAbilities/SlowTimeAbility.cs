using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlowTimeAbility : MonoBehaviour
{
    float sensitivityY;
    public GameObject SlowAbilityPrefab;
    public Transform SlowAbilitySpawn;
    public Transform ChronosTransform;
    public GameObject SlowCircle;
    AbilityTriggerDetection MinotaurATD;
    AbilityTriggerDetection HoundATD;
    AbilityTriggerDetection HadesATD;
    bool InitiateSlow = false;
    GameObject hellMinotaur;
    GameObject hellHound;
    GameObject Hades;
    PlayerController PC;
    public GameObject OneActive;
    public GameObject OneCoolDownTextActive;
    public Text OneCoolDownText;
    float Cooldown = 8.0f;
    bool CooldownActive = false;
    float SlowDuration = 0.0f;
    public bool IsSlowAbility = false;
    public bool IsEnabled = false;
    bool PlaceAbility = false;
    Scene currentScene;
    string sceneName;

    void Start ()
    {
        sensitivityY = 3.0f;
        hellMinotaur = GameObject.Find("HellMinotaur");
        hellHound = GameObject.Find("HellHoundModel");
        MinotaurATD = hellMinotaur.GetComponent<AbilityTriggerDetection>();
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
            if (Input.GetKeyDown("1"))
            {
                SlowCircle = (GameObject)Instantiate(SlowAbilityPrefab, SlowAbilitySpawn.position, ChronosTransform.rotation);
                OneActive.SetActive(true);
                Cooldown = 8.0f;
            }

            if (Input.GetKey("1"))
            {
                if(SlowCircle != null)
                    SlowCircle.transform.parent = ChronosTransform;

                IsEnabled = false;

                float distance = Vector3.Distance(SlowCircle.transform.position, ChronosTransform.position);
                Vector3 forward = ChronosTransform.transform.TransformDirection(Vector3.forward);
                Vector3 toOther = SlowCircle.transform.position - ChronosTransform.position;

                if (distance < 30.00f && Vector3.Dot(forward, toOther) > 0)
                    SlowCircle.transform.Translate(Vector3.forward * Input.GetAxis("Mouse Y") * sensitivityY);
                else if (Input.GetAxis("Mouse Y") > 0 && distance < 30.00f && Vector3.Dot(forward, toOther) >= 0)
                    SlowCircle.transform.Translate(Vector3.forward * Input.GetAxis("Mouse Y") * sensitivityY);
                else if (Input.GetAxis("Mouse Y") < 0 && distance >= 30.00f && Vector3.Dot(forward, toOther) > 0)
                    SlowCircle.transform.Translate(Vector3.forward * Input.GetAxis("Mouse Y") * sensitivityY);
                else if (distance >= 30.00f)
                    distance = 30.00f;
                else if (Vector3.Dot(forward, toOther) < 0)
                    SlowCircle.transform.position = ChronosTransform.position;
                else
                    SlowCircle.transform.position = ChronosTransform.position;
            }

            if (Input.GetKeyUp("1"))
            {
                if(SlowCircle != null)
                    SlowCircle.transform.parent = null;

                InitiateSlow = true;
                PC.StopAnim = true;
                PC.ChronosAnimController.SetBool("Slow", true);
                PlaceAbility = true;
            }
        }
        else
        {
            if (InitiateSlow == true)
            {
                SlowDuration = 0.0f;
                IsEnabled = true;
                IsSlowAbility = true;
                CooldownActive = true;
            }
        }

        if (IsSlowAbility == true)
        {
            InitiateSlow = false;
            SlowDuration += Time.deltaTime;

            if(SlowDuration >= 1.0f)
            {
                PC.StopAnim = false;
                PC.ChronosAnimController.SetBool("Slow", false);
            }

            hellMinotaur = GameObject.Find("HellMinotaur");
            if(hellMinotaur != null)
                MinotaurATD = hellMinotaur.GetComponent<AbilityTriggerDetection>();

            if (SlowCircle != null)
            {
                if (sceneName == "Tutorial")
                {
                    if (MinotaurATD.minotaurHit == true)
                    {
                        SlowCircle.transform.localPosition = new Vector3(0, -3.44f, 0);
                        foreach (Renderer r in SlowCircle.GetComponentsInChildren<Renderer>())
                            r.enabled = true;
                    }

                    if (HoundATD.HoundHit == true)
                    {
                        SlowCircle.transform.localPosition = new Vector3(0.00019f, -0.0095f, 0.00174f);
                        foreach (Renderer r in SlowCircle.GetComponentsInChildren<Renderer>())
                            r.enabled = true;
                    }

                    if (MinotaurATD.minotaurHit == false && HoundATD.HoundHit == false)
                    {
                        foreach (Renderer r in SlowCircle.GetComponentsInChildren<Renderer>())
                            r.enabled = false;
                    }
                }

                if (sceneName == "BossBattle")
                {
                    Hades = GameObject.Find("HadesModel");
                    if(Hades != null)
                        HadesATD = Hades.GetComponent<AbilityTriggerDetection>();

                    if (MinotaurATD.minotaurHit == true)
                    {
                        SlowCircle.transform.localPosition = new Vector3(0, -3.17f, 0);
                        foreach (Renderer r in SlowCircle.GetComponentsInChildren<Renderer>())
                            r.enabled = true;
                    }

                    if (HoundATD.HoundHit == true)
                    {
                        SlowCircle.transform.localPosition = new Vector3(0.00019f, -0.0095f, 0.00174f);
                        foreach (Renderer r in SlowCircle.GetComponentsInChildren<Renderer>())
                            r.enabled = true;
                    }

                    if (HadesATD.HadesHit == true)
                    {
                        SlowCircle.transform.localPosition = new Vector3(0f, -0.535f, 0f);
                        foreach (Renderer r in SlowCircle.GetComponentsInChildren<Renderer>())
                            r.enabled = true;
                    }

                    if (MinotaurATD.minotaurHit == false && HoundATD.HoundHit == false && HadesATD.HadesHit == false)
                    {
                        foreach (Renderer r in SlowCircle.GetComponentsInChildren<Renderer>())
                            r.enabled = false;
                    }
                }
            }

            if (SlowDuration >= 3.0f)
            {
                PlaceAbility = false;

                if (MinotaurATD.minotaurHit == true)
                {
                    // revert slow effect to normal
                    MinotaurATD.BSDB.MovementSlow = 0.0f;
                    MinotaurATD.BSDB.RangedAttackSlow = 0.0f;
                    MinotaurATD.BSDB.RangedAttackDurationIncrease = 0.0f;
                    MinotaurATD.BSDB.MeleeDurationIncrease = 0.0f;
                    MinotaurATD.BSDB.MeleeContactIncrease = 0.0f;
                    MinotaurATD.BSDB.BlockDurationIncrease = 0.0f;
                    MinotaurATD.BSDB.BlockDelayIncrease = 0.0f;
                    MinotaurATD.BSDB.MinotaurAnimController.speed = 1.0f;
                }

                if (sceneName == "BossBattle")
                {
                    if (HoundATD.HoundHit == true)
                    {
                        HoundATD.HSDB.MovementSlow = 0.0f;
                        HoundATD.HSDB.MeleeDurationIncrease = 0.0f;
                        HoundATD.HSDB.MeleeContactIncrease = 0.0f;
                    }

                    if (HadesATD.HadesHit == true)
                    {
                        HadesATD.HadesSDB.MovementSlow = 0.0f;
                        HadesATD.HadesSDB.RangedAttackSlow = 0.0f;
                        HadesATD.HadesSDB.RangedAttackDurationIncrease = 0.0f;
                        HadesATD.HadesSDB.MeleeDurationIncrease = 0.0f;
                        HadesATD.HadesSDB.MeleeContactIncrease = 0.0f;
                        HadesATD.HadesSDB.BlockDurationIncrease = 0.0f;
                        HadesATD.HadesSDB.BlockDelayIncrease = 0.0f;
                        HadesATD.HadesSDB.HadesAnimController.speed = 1.0f;
                    }

                    HadesATD.HadesHit = false;
                }

                Destroy(SlowCircle);

                MinotaurATD.minotaurHit = false;
                HoundATD.HoundHit = false;
                IsSlowAbility = false;
            }
        }

        if (CooldownActive == true)
        {
            OneCoolDownTextActive.SetActive(true);
            Cooldown -= Time.deltaTime;
            string CooldownToString = ((int)Cooldown + 1).ToString();
            OneCoolDownText.text = CooldownToString;

            if (Cooldown <= 0.0f)
            {
                OneActive.SetActive(false);
                OneCoolDownTextActive.SetActive(false);
                CooldownActive = false;
            }
        }
    }
}
