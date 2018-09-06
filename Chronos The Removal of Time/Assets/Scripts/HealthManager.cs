using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public int MinotaurHealth = 100;
    public int MinotaurMAXhealth = 100;
    public int ChronosHealth = 100;
    public int ChronosMAXHealth = 100;
    float ChronosHealthRefresh = 2.0f;
    public int HoundHealth = 100;
    public int HoundMAXHealth = 100;
    public int HadesHealth = 100;
    public int HadesMaxHealth = 100;
    float HoundHealthRefresh = 2.0f;
    public bool HitOnce = false;
    float LavaDamageCountdown = 0.8f;
    float delay = 0.5f;
    GameObject minotaur;
    GameObject Base;
    GameObject hellHound;
    GameObject hades;
    BasicStateDrivenBrain BSDB;
    HoundStateDrivenBrain HSDB;
    HellMinotaurActive HMA;
    Transform ChronosSpawnTransform;
    Transform ChronosTransform;
    Scene currentScene;
    string sceneName;

    void Start()
    {
        MinotaurMAXhealth = 100;
        ChronosMAXHealth = 100;
        HoundMAXHealth = 100;

        minotaur = GameObject.Find("Minotaur");
        BSDB = minotaur.GetComponent<BasicStateDrivenBrain>();

        hellHound = GameObject.Find("HellHound");

        if (sceneName == "BossBattle")
        {
            HSDB = hellHound.GetComponent<HoundStateDrivenBrain>();
            hades = GameObject.Find("Hades");
        }

        Base = GameObject.Find("Base");
        HMA = Base.GetComponent<HellMinotaurActive>();

        ChronosTransform = GameObject.Find("Chronos").transform;
        ChronosSpawnTransform = GameObject.Find("ChronosSpawn").transform;
    }

    void OnCollisionStay(Collision collision)
    {
        if(this.gameObject.name == ("Chronos") && collision.gameObject.tag == "Lava")
        {
            LavaDamageCountdown -= Time.deltaTime;
            Debug.Log("Chronos is touching lava");

            if (LavaDamageCountdown <= 0.0f)
            {
                ChronosHealth -= 20;
                LavaDamageCountdown = 0.8f;
            }
        }

        if (this.gameObject.name == ("HellHound") && collision.gameObject.tag == "Lava")
        {
            LavaDamageCountdown -= Time.deltaTime;
            Debug.Log("Chronos is touching lava");

            if (LavaDamageCountdown <= 0.0f)
            {
                HoundHealth -= 20;
                LavaDamageCountdown = 0.8f;
            }
        }

        if (this.gameObject.name == ("Minotaur") && collision.gameObject.tag == "Lava")
        {
            LavaDamageCountdown -= Time.deltaTime;
            Debug.Log("Chronos is touching lava");

            if (LavaDamageCountdown <= 0.0f)
            {
                MinotaurHealth -= 20;
                LavaDamageCountdown = 0.8f;
            }
        }

        if (this.gameObject.name == ("Hades") && collision.gameObject.tag == "Lava")
        {
            LavaDamageCountdown -= Time.deltaTime;
            Debug.Log("Hades is touching lava");

            if (LavaDamageCountdown <= 0.0f)
            {
                HadesHealth -= 20;
                LavaDamageCountdown = 0.8f;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == ("ChronosSphere(Clone)") && this.gameObject.name == ("HellHound") && HitOnce == false)
        {
            HoundHealth -= 10;
            Debug.Log("Projectile Hit Hound");
            delay = 0.5f;
            HitOnce = true;
        }

        if (col.gameObject.name == ("ChronosSphere(Clone)") && this.gameObject.name == ("Minotaur") && HitOnce == false)
        {
            minotaur = GameObject.Find("Minotaur");
            BSDB = minotaur.GetComponent<BasicStateDrivenBrain>();

            if (BSDB.Blocking == false)
                MinotaurHealth -= 10;
            else
                Debug.Log("Minotaur Blocked the Projectile");

            Debug.Log("Projectile Hit Minotaur");
            delay = 0.5f;
            HitOnce = true;
        }

        if (col.gameObject.name == ("ChronosSphere(Clone)") && this.gameObject.name == ("Hades") && HitOnce == false)
        {
            hades = GameObject.Find("Hades");

            if (hades.GetComponent<HadesStateDrivenBrain>().Blocking == false)
                HadesHealth -= 10;
            else
                Debug.Log("Hades Blocked the Projectile");

            Debug.Log("Projectile Hit Hades");
            delay = 0.5f;
            HitOnce = true;
        }

        if ((col.gameObject.name == ("MinotaurSphere(Clone)") || col.gameObject.name == ("HadesSphere(Clone)")) && this.gameObject.name == ("Chronos") && HitOnce == false)
        {
            Debug.Log("Projectile Hit Chronos");
            ChronosHealth -= 10;
            delay = 0.2f;
            HitOnce = true;
        }
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (BSDB.InContactWithChronos == true && BSDB.MeleeContact == true && HitOnce == false)
        {
            Debug.Log("Minotaur Dealt 10 Damage");
            ChronosHealth -= 10;
            delay = 0.2f;
            HitOnce = true;
        }

        if(sceneName == "BossBattle")
        {
            if(hellHound != null)
                HSDB = hellHound.GetComponent<HoundStateDrivenBrain>();

            if (HSDB.InContactWithChronos == true && HSDB.MeleeContact == true && HitOnce == false)
            {
                Debug.Log("Hound Dealt 10 Damage");
                ChronosHealth -= 10;
                delay = 0.2f;
                HitOnce = true;
            }

            hades = GameObject.Find("Hades");

            if (hades != null)
            {
                if (hades.GetComponent<HadesStateDrivenBrain>().InContactWithChronos == true && hades.GetComponent<HadesStateDrivenBrain>().MeleeContact == true && HitOnce == false)
                {
                    Debug.Log("Hades Dealt 10 Damage");
                    ChronosHealth -= 10;
                    delay = 0.2f;
                    HitOnce = true;
                }
            }
        }

        if (this.gameObject.name == ("Chronos") && HMA.ChronosRegenerates == true && ChronosHealth != ChronosMAXHealth && sceneName == "Tutorial")
        {
            ChronosHealthRefresh -= Time.deltaTime;

            if (ChronosHealthRefresh <= 0.0f)
            {
                ChronosHealth += 10;
                Debug.Log("Chronos healed 10 health");
                ChronosHealthRefresh = 2.0f;
            } 
        }

        if (this.gameObject.name == ("HellHound") && HoundHealth != HoundMAXHealth && sceneName == "Tutorial")
        {
            HoundHealthRefresh -= Time.deltaTime;

            if (HoundHealthRefresh <= 0.0f)
            {
                HoundHealth = HoundMAXHealth;
                HoundHealthRefresh = 2.0f;
            }
        }

        if (MinotaurHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Should be Destroyed");
        }

        if (HoundHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Should be Destroyed");
        }

        if (HadesHealth <= 0)
        {
            if (hades.GetComponent<HadesStateDrivenBrain>().RootCircle != null)
                Destroy(hades.GetComponent<HadesStateDrivenBrain>().RootCircle);
            Destroy(gameObject);
            Debug.Log("Should be Destroyed");
        }

        if (ChronosHealth <= 0)
        {
            ChronosTransform.position = ChronosSpawnTransform.position;
            ChronosTransform.rotation = ChronosSpawnTransform.rotation;
            ChronosHealth = ChronosMAXHealth;
        }

        if (HitOnce == true)
        {
            delay -= Time.deltaTime;

            if (delay <= 0.0f)
                HitOnce = false;
        }
    }
}
