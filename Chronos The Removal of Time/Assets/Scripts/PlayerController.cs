using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour 
{
    float x, z, VerticalSpeed, HorizontalSpeed;
    float jumpVelocity = 10.0f;
    float fallMultiplier = 5.0f;
    float delay = 0.25f;
    public float attackTimer = 0.0f;
    float FallSpeed = 1.0f;
    public float MovementSpeedBoost = 1.0f;
    public float AttackSpeedBoost = 0.0f;
    public float DodgeCooldown = 2.0f;

    int Count = 0;
    int maxCount = 1;

    public bool StopAnim = false;
    public bool grounded = true;
    bool basicAttacking = false;
    public bool rangedAttack = false;
    bool ToggleAttack = false;
    bool dodge = false;
    public bool DodgeCooldownActive = false;
    bool MeleeContact = false;
    bool MeleeContactOnce = false;
    bool MeleedMinotaur = false;
    bool MeleedHound = false;
    bool MeleedHades = false;
    public bool RootActive = false;
    bool[] dodgeDirection = new bool[4];

    public GameObject ProjectilePrefab;
    public Transform ProjectileSpawn;
    GameObject Projectile;
    Rigidbody rb;
    GameObject minotaur;
    GameObject HellHound;
    GameObject Hades;
    GameObject chronosModel;
    HealthManager HM;
    BasicStateDrivenBrain BSDB;
    HadesStateDrivenBrain HadesSDB;
    CapsuleCollider capsule;
    BoxCollider boxCollider;
    Vector2 input;
    Vector3 desiredMove;
    public GameObject CrossedSwords, BowAndArrow;
    Scene currentScene;
    string sceneName;
    public Animator ChronosAnimController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start () 
	{
		x = 0.0f;
		z = 0.0f;

        Count = maxCount;
        VerticalSpeed = 10.0f;
        HorizontalSpeed = 8.0f;

        minotaur = GameObject.Find("Minotaur");
        HM = minotaur.GetComponent<HealthManager>();
        BSDB = minotaur.GetComponent<BasicStateDrivenBrain>();

        chronosModel = GameObject.Find("ChronosModel");
        boxCollider = chronosModel.GetComponent<BoxCollider>();

        capsule = GetComponent<CapsuleCollider>();

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        ChronosAnimController = GetComponentInChildren<Animator>();

        for (int i = 0; i < dodgeDirection.Length; i++)
        {
            dodgeDirection[i] = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        grounded = true;
        Count = 1;
    }

    void OnCollisionStay(Collision col)
    {
        grounded = true;
    } 

    void OnCollisionExit(Collision col)
    {
        grounded = false;
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name == ("HellMinotaur") && attackTimer >= (0.65f - AttackSpeedBoost) && MeleeContact == false && MeleeContactOnce == false && ToggleAttack == false)
        {
            Debug.Log("Melee Connected  to Minotaur");
            MeleedMinotaur = true;
            MeleeContact = true;
            MeleeContactOnce = true;
        }

        if (col.gameObject.name == ("HellHoundModel") && attackTimer >= (0.65f - AttackSpeedBoost) && MeleeContact == false && MeleeContactOnce == false && ToggleAttack == false)
        {
            Debug.Log("Melee Connected  to Hell Hound");
            MeleedHound = true;
            MeleeContact = true;
            MeleeContactOnce = true;
        }

        if (col.gameObject.name == ("HadesModel") && attackTimer >= (0.65f - AttackSpeedBoost) && MeleeContact == false && MeleeContactOnce == false && ToggleAttack == false)
        {
            Debug.Log("Melee Connected  to Hades");
            MeleedHades = true;
            MeleeContact = true;
            MeleeContactOnce = true;
        }
    }

    void MeleeDamage()
    {
        if(MeleedMinotaur == true)
        {
            minotaur = GameObject.Find("Minotaur");
            HM = minotaur.GetComponent<HealthManager>();
            BSDB = minotaur.GetComponent<BasicStateDrivenBrain>();

            if (BSDB.Blocking != true)
                HM.MinotaurHealth -= 10;
            else
                Debug.Log("Minotaur Successfully Blocked the Melee");

            MeleeContact = false;
            MeleedMinotaur = false;
        }
        
        if(MeleedHound == true)
        {
            HellHound = GameObject.Find("HellHound");
            HM = HellHound.GetComponent<HealthManager>();
            HM.HoundHealth -= 10;
            MeleeContact = false;
            MeleedHound = false;
        }

        if(MeleedHades == true)
        {
            Hades = GameObject.Find("Hades");
            HM = Hades.GetComponent<HealthManager>();
            HadesSDB = Hades.GetComponent<HadesStateDrivenBrain>();

            if (HadesSDB.Blocking != true)
                HM.HadesHealth -= 10;
            else
                Debug.Log("Hades Successfully Blocked the Melee");

            MeleeContact = false;
            MeleedHades = false;
        }
    }

    void Walk()
    {
        //Chronos wasd movement
        z = Input.GetAxis("Vertical") * Time.deltaTime * VerticalSpeed * MovementSpeedBoost / FallSpeed;
        x = Input.GetAxis("Horizontal") * Time.deltaTime * HorizontalSpeed * MovementSpeedBoost / FallSpeed;

        if (Input.GetAxis("Vertical") < 0)
            z = z / 2.0f;



        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") == 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("Forward", true);
        else
            ChronosAnimController.SetBool("Forward", false);

        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") == 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("Backward", true);
        else
            ChronosAnimController.SetBool("Backward", false);

        if (Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") == 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("Left", true);
        else
            ChronosAnimController.SetBool("Left", false);

        if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") == 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("Right", true);
        else
            ChronosAnimController.SetBool("Right", false);

        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("ForwardLeft", true);
        else
            ChronosAnimController.SetBool("ForwardLeft", false);

        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("ForwardRight", true);
        else
            ChronosAnimController.SetBool("ForwardRight", false);

        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("BackwardLeft", true);
        else
            ChronosAnimController.SetBool("BackwardLeft", false);

        if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0 && grounded == true && basicAttacking == false && rangedAttack == false && StopAnim == false)
            ChronosAnimController.SetBool("BackwardRight", true);
        else
            ChronosAnimController.SetBool("BackwardRight", false);

        transform.Translate(x, 0, z);
    }

    void Sprint()
    {
        //Chronos sprint
        if (Input.GetKey(KeyCode.LeftShift) && grounded == true && Input.GetAxis("Vertical") > 0 && basicAttacking == false)
        {
            ChronosAnimController.speed = 1.5f;
            VerticalSpeed = 15.0f;
        }
        else
        {
            ChronosAnimController.speed = 1.0f;
            VerticalSpeed = 10.0f;
        }
    }

    void Dodge()
    {
        delay -= Time.deltaTime;

        //Determines Chronos's direction of dodge
        if (Input.GetKey("w") && (Input.GetKey("a") != true) && (Input.GetKey("d") != true))
            dodgeDirection[0] = true;
        else if (Input.GetKey("a"))
            dodgeDirection[1] = true;
        else if (Input.GetKey("d"))
            dodgeDirection[2] = true;
        else if (Input.GetKey("s"))
            dodgeDirection[3] = true;
        else
        {
            Count = 1;
            dodge = false;
            DodgeCooldownActive = true;
        }

        //Moves Chronos in direction of dodge
        if (dodgeDirection[0] == true && dodgeDirection[1] == false && dodgeDirection[2] == false)
            transform.Translate(Vector3.forward * Time.deltaTime * 40.0f);
        else if (dodgeDirection[1] == true)
            transform.Translate(Vector3.left * Time.deltaTime * 40.0f);
        else if (dodgeDirection[2] == true)
            transform.Translate(Vector3.right * Time.deltaTime * 40.0f);
        else if (dodgeDirection[3] == true)
            transform.Translate(Vector3.back * Time.deltaTime * 40.0f);

        if (delay < 0)
        {
            for (int i = 0; i < dodgeDirection.Length; i++)
            {
                dodgeDirection[i] = false;
            }
            Count = 1;
            dodge = false;
            DodgeCooldownActive = true;
        }
    }
    
    void GetInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        input = new Vector2(h, v);
    }

    void StepCheck()
    {
        if (input.x == 0 && input.y == 0)
        {
            return;
        }

        Vector3 direction = desiredMove.normalized;
        float length = capsule.radius + 0.01f;

        Ray ray = new Ray(transform.position + new Vector3(0, 0.25f, 0), direction);
        Debug.DrawRay(transform.position + new Vector3(0, 0.25f, 0), direction * length);

        if (Physics.Raycast(ray, length))
        {
            Ray ray2 = new Ray(transform.position + new Vector3(0, 0.25f, 0), direction);
            Debug.DrawRay(transform.position + new Vector3(0, 0.25f, 0), direction * length);
            if (!Physics.Raycast(ray2, length))
            {
                Vector3 rayStart = transform.position + new Vector3(0, 0.25f, 0) + direction * length;
                Ray ray3 = new Ray(rayStart, Vector3.down);
                Debug.DrawRay(rayStart, Vector3.down * 0.25f);
                RaycastHit hit;
                if (Physics.Raycast(ray3, out hit, 0.25f))
                {
                    Vector3 stepHeight = hit.point;
                    transform.position = new Vector3(transform.position.x, stepHeight.y, transform.position.z);
                }
            }
        }
    }

    void SpawnPrefab()
    {
        Projectile = (GameObject)Instantiate(ProjectilePrefab, ProjectileSpawn.position, ProjectileSpawn.rotation);
        Destroy(Projectile, (0.6f - AttackSpeedBoost));
    }

    void FixedUpdate()
    {
        //Chronos jump
        if (Input.GetKeyDown(KeyCode.Space) && Count == maxCount && Input.GetKey("1") == false && Input.GetKey("3") == false && RootActive == false)
        {
            grounded = false;
            ChronosAnimController.SetBool("Jump", true);
            rb.velocity = Vector3.up * jumpVelocity;
            Count = 0;
        }

        //Increases Chronos's fall speed
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void Update()
    {
        StepCheck();
        GetInput();
        desiredMove = transform.forward * input.y * 10.0f + transform.right * input.x * 8.0f;

        //Calls the Walk, Sprint and Dodge functions based on whether conditions are met
        if (dodge == false)
        {
            if(sceneName == "Tutorial")
                Walk();
            else
            {
                if(RootActive == false)
                    Walk();
            }

            if (basicAttacking == true)
            {
                attackTimer += Time.deltaTime;
                ChronosAnimController.SetBool("Melee", true);

                if (attackTimer >= (0.55f - AttackSpeedBoost))
                    boxCollider.enabled = true;

                if (attackTimer >= (1.0f - AttackSpeedBoost))
                {
                    basicAttacking = false;
                    ChronosAnimController.SetBool("Melee", false);
                    MeleeContactOnce = false;
                    attackTimer = 0.0f;
                    boxCollider.enabled = false;
                }
            }
            else if(rangedAttack == true)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer > 0.6)
                    ChronosAnimController.SetBool("RangedAttack", false);

                if (attackTimer < (0.9f - AttackSpeedBoost))
                {
                    
                    if (Projectile != null)
                    {
                        Projectile.transform.Translate(Vector3.forward * Time.deltaTime * 40.0f);
                    }
                }
                else
                {
                    rangedAttack = false;
                    attackTimer = 0.0f;
                }
            }
            else
            {
                if (sceneName == "Tutorial")
                    Sprint();
                else
                {
                    if (RootActive == false)
                        Sprint();
                }
            }
        }
        else
        {
            if (sceneName == "Tutorial")
                Dodge();
            else
            {
                if (RootActive == false)
                    Dodge();
            }
        }

        //Player left clicks to basic attack
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift) == false && dodge == false && Input.GetKey("1") == false && Input.GetKey("3") == false)
        {
            if(ToggleAttack == false)
            {
                if (attackTimer <= 0)
                {
                    basicAttacking = true;
                    Debug.Log("Melee Attack");
                }
            }
            else if(ToggleAttack == true)
            {
                if (attackTimer <= 0)
                {
                    if (rangedAttack == false)
                    {
                        Invoke("SpawnPrefab", 0.3f);
                        ChronosAnimController.SetBool("RangedAttack", true);
                    }
                    rangedAttack = true;
                }
            }
        }

        //When Q is pressed it will toggle Chronos between 
        if(Input.GetKeyDown("q") == true)
        {
            if(ToggleAttack == false)
            {
                ToggleAttack = true;
                CrossedSwords.SetActive(false);
                BowAndArrow.SetActive(true);
                Debug.Log("Switched to Ranged");
            }
            else
            {
                ToggleAttack = false;
                CrossedSwords.SetActive(true);
                BowAndArrow.SetActive(false);
                Debug.Log("Switched to Melee");
            }
        }

        //Player right clicks to dodge
        if (Input.GetMouseButtonDown(1) && grounded == true && Count == 1 && Input.GetKey("1") == false && Input.GetKey("3") == false && DodgeCooldownActive == false)
        {
            dodge = true;
            Count = 0;
            delay = 0.25f;
            attackTimer = 0.0f;
            DodgeCooldown = 2.0f;
            x = 0;
            z = 0;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("StartMenu");

        if(sceneName == "BossBattle")
        {
            Hades = GameObject.Find("Hades");
            if(Hades == null)
                SceneManager.LoadScene("StartMenu");
        }


        if (MeleeContact == true)
            MeleeDamage();

        //Changing fall speed to make player move slower while in the air
        if (grounded == false)
        {
            FallSpeed = 1.25f;
        }
        else
        {
            FallSpeed = 1.0f;
            ChronosAnimController.SetBool("Jump", false);
        }

        if(DodgeCooldownActive == true)
        {
            DodgeCooldown -= Time.deltaTime;

            if (DodgeCooldown <= 0.0f)
                DodgeCooldownActive = false;
        }
    }
}
