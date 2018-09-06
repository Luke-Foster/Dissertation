using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesStateDrivenBrain : BasicAIController
{
    // Declares the constant state names 
    public enum HadesAIStates { HadesMove, HadesMelee, HadesRangedAttack, HadesBlock, HadesDodge, HadesAbility };
    // Sets up state machine variable
    public FSM<HadesAIStates> hadesAIStateMachine;
    // Puts a 0.4s interval on the state machine
    protected float thinkInterval = 0.4f;
    // Initialises chopperStateActive to true 
    public bool hadesAIStateActive = true;

    public Transform ChronosTransform;
    public bool InMeleeRange = false;
    public bool RangedAttackActive = false;
    public bool Block = false;
    public bool Blocking = false;
    public bool MeleeContact = false;
    public bool InContactWithChronos = false;
    public bool Dodge = false;
    public bool AbilityHittingChronos = false;
    public bool AbilityActive = false;
    public bool AbilityDamage = false;

    public float MeleeDuration = 0.0f;
    public float RangedAttackDuration = 0.0f;
    public float randomAbilityTimer = 0.0f;
    public float randomTimer = 0.0f;
    public float randomRangedAttackValue;
    public float BlockDuration = 0.0f;
    public float MovementSlow = 0.0f;
    public float RangedAttackSlow = 0.0f;
    public float RangedAttackDurationIncrease = 0.0f;
    public float MeleeDurationIncrease = 0.0f;
    public float MeleeContactIncrease = 0.0f;
    public float BlockDurationIncrease = 0.0f;
    public float BlockDelayIncrease = 0.0f;
    public float AbilityDelay = 0.0f;

    public int ClickCount = 0;
    public int randomBlock;
    public int randomDodge;
    public int randomAbility;
    public GameObject ProjectilePrefab;
    public Transform ProjectileSpawn;
    public GameObject Projectile;
    public RaycastHit hit = new RaycastHit();
    public GameObject RootCircle;
    public GameObject RootAbilityPrefab;
    public Animator HadesAnimController;

    protected void Awake()
    {
        // Sets up a new FSM 
        hadesAIStateMachine = new FSM<HadesAIStates>();
        // This adds in all the new states into the state machine using variables declared
        hadesAIStateMachine.AddState(new HadesMove<HadesAIStates>(HadesAIStates.HadesMove, this, 0f));
        hadesAIStateMachine.AddState(new HadesMelee<HadesAIStates>(HadesAIStates.HadesMelee, this, 0f));
        hadesAIStateMachine.AddState(new HadesRangedAttack<HadesAIStates>(HadesAIStates.HadesRangedAttack, this, 0f));
        hadesAIStateMachine.AddState(new HadesBlock<HadesAIStates>(HadesAIStates.HadesBlock, this, 0f));
        hadesAIStateMachine.AddState(new HadesDodge<HadesAIStates>(HadesAIStates.HadesDodge, this, 0f));
        hadesAIStateMachine.AddState(new HadesAbility<HadesAIStates>(HadesAIStates.HadesAbility, this, 0f));
        // Sets Idle as first state to be in once plays been activated
        hadesAIStateMachine.SetInitialState(HadesAIStates.HadesMove);

        // These are all the transitions of the states I will require within this program
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesMove, HadesAIStates.HadesMelee);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesMove, HadesAIStates.HadesRangedAttack);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesMove, HadesAIStates.HadesBlock);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesMove, HadesAIStates.HadesDodge);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesMove, HadesAIStates.HadesAbility);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesMelee, HadesAIStates.HadesMove);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesRangedAttack, HadesAIStates.HadesMove);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesBlock, HadesAIStates.HadesMove);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesDodge, HadesAIStates.HadesMove);
        hadesAIStateMachine.AddTransition(HadesAIStates.HadesAbility, HadesAIStates.HadesMove);
    }

    // These Guards determine the conditions states need to meet to be initialised 
    public bool GuardHadesMoveToHadesMelee(State<HadesAIStates> currentState)
    {
        return (InMeleeRange == true);
    }

    public bool GuardHadesMoveToHadesRangedAttack(State<HadesAIStates> currentState)
    {
        return (RangedAttackActive == true);
    }

    public bool GuardHadesMoveToHadesBlock(State<HadesAIStates> currentState)
    {
        return (Block == true);
    }

    public bool GuardHadesMoveToHadesDodge(State<HadesAIStates> currentState)
    {
        return (Dodge == true);
    }

    public bool GuardHadesMoveToHadesAbility(State<HadesAIStates> currentState)
    {
        return (AbilityActive == true);
    }

    public bool GuardHadesMeleeToHadesMove(State<HadesAIStates> currentState)
    {
        return (MeleeDuration >= (2.3f + MeleeDurationIncrease));
    }

    public bool GuardHadesRangedAttackToHadesMove(State<HadesAIStates> currentState)
    {
        return (RangedAttackDuration >= 2.2f);
    }

    public bool GuardHadesBlockToHadesMove(State<HadesAIStates> currentState)
    {
        return (BlockDuration >= (0.8f + BlockDurationIncrease));
    }

    public bool GuardHadesDodgeToHadesMove(State<HadesAIStates> currentState)
    {
        return (Dodge == false);
    }

    public bool GuardHadesAbilityToHadesMove(State<HadesAIStates> currentState)
    {
        return (AbilityDelay >= 2.0f);
    }

    public void Start()
    {
        // Calls to Think function
        StartCoroutine(Think());
        randomRangedAttackValue = Random.Range(3.0f, 8.0f);
        randomBlock = Random.Range(1, 5);
        randomDodge = Random.Range(1, 5);
        randomAbility = Random.Range(10, 16);
        ChronosTransform = GameObject.Find("Chronos").transform;
        HadesAnimController = GetComponentInChildren<Animator>();
        HadesAnimController.SetBool("BrainActiveAnim", true);
    }

    public void Update()
    {
        base.Update();
        // Refers to all act callbacks within other scripts
        if (hadesAIStateActive)
        {
            hadesAIStateMachine.CurrentState.Act();
        }

        ChronosTransform = GameObject.Find("Chronos").transform;

        randomTimer += Time.deltaTime;

        if (randomTimer >= randomRangedAttackValue)
            RangedAttackActive = true;

        randomAbilityTimer += Time.deltaTime;

        if (randomAbilityTimer >= randomAbility && AbilityDelay < 1.8f)
            AbilityActive = true;

        if (Input.GetMouseButtonDown(0) && ClickCount == 0)
        {
            randomBlock = Random.Range(1, 5);
            randomDodge = Random.Range(1, 5);
        }

        if (Input.GetMouseButtonDown(0) && ClickCount == 0 && randomBlock == 2)
        {
            ClickCount = 1;
            Block = true;
        }

        if (Input.GetMouseButtonDown(0) && ClickCount == 0 && randomDodge == 2)
        {
            ClickCount = 1;
            Dodge = true;
        }

        if (gameObject == null && Projectile != null)
            Destroy(Projectile);

        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            var distanceToGround = hit.distance;
        }

        if (hit.distance >= 6.1f && hit.transform.tag == "Ground")
            transform.Translate(-Vector3.up * (3 - MovementSlow) * Time.deltaTime);
        else if (hit.distance <= 5.9f && hit.transform.tag == "Ground")
            transform.Translate(Vector3.up * (3 - MovementSlow) * Time.deltaTime);
    }

    protected IEnumerator Think()
    {
        yield return new WaitForSeconds(thinkInterval);
        // Checks state machine 
        hadesAIStateMachine.Check();
        // Calls to Think function
        StartCoroutine(Think());
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == ("Chronos"))
        {
            InMeleeRange = true;
            InContactWithChronos = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        InContactWithChronos = false;
    }
}
