using UnityEngine;
using System.Collections;

public class BasicStateDrivenBrain : BasicAIController
{
	// Declares the constant state names 
	public enum BasicAIStates { BasicMove, BasicMelee, BasicRangedAttack, BasicBlock };
	// Sets up state machine variable
	public FSM<BasicAIStates> basicAIStateMachine;
	// Puts a 0.4s interval on the state machine
	protected float thinkInterval = 0.4f;
	// Initialises chopperStateActive to true 
	public bool basicAIStateActive = true;

    public Transform ChronosTransform;
    public bool InMeleeRange = false;
    public bool RangedAttackActive = false;
    public bool Block = false;
    public bool Blocking = false;
    public bool MeleeContact = false;
    public bool InContactWithChronos = false;

    public float MeleeDuration = 0.0f;
    public float RangedAttackDuration = 0.0f;
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

    public int ClickCount = 0;
    public int randomBlock;
    public GameObject ProjectilePrefab;
    public Transform ProjectileSpawn;
    public GameObject Projectile;
    public Animator MinotaurAnimController;
    
    protected void Awake()
	{
        // Sets up a new FSM 
        basicAIStateMachine = new FSM<BasicAIStates>();
        // This adds in all the new states into the state machine using variables declared
        basicAIStateMachine.AddState(new BasicMove<BasicAIStates>(BasicAIStates.BasicMove, this, 0f));
        basicAIStateMachine.AddState(new BasicMelee<BasicAIStates>(BasicAIStates.BasicMelee, this, 0f));
        basicAIStateMachine.AddState(new BasicRangedAttack<BasicAIStates>(BasicAIStates.BasicRangedAttack, this, 0f));
        basicAIStateMachine.AddState(new BasicBlock<BasicAIStates>(BasicAIStates.BasicBlock, this, 0f));
        // Sets Idle as first state to be in once plays been activated
        basicAIStateMachine.SetInitialState (BasicAIStates.BasicMove);

        // These are all the transitions of the states I will require within this program
        basicAIStateMachine.AddTransition (BasicAIStates.BasicMove, BasicAIStates.BasicMelee);
        basicAIStateMachine.AddTransition (BasicAIStates.BasicMove, BasicAIStates.BasicRangedAttack);
        basicAIStateMachine.AddTransition (BasicAIStates.BasicMove, BasicAIStates.BasicBlock);
        basicAIStateMachine.AddTransition(BasicAIStates.BasicMelee, BasicAIStates.BasicMove);
        basicAIStateMachine.AddTransition(BasicAIStates.BasicRangedAttack, BasicAIStates.BasicMove);
        basicAIStateMachine.AddTransition(BasicAIStates.BasicBlock, BasicAIStates.BasicMove);
    }

	// These Guards determine the conditions states need to meet to be initialised 
	public bool GuardBasicMoveToBasicMelee(State<BasicAIStates> currentState)
	{
		return (InMeleeRange == true);
	}

    public bool GuardBasicMoveToBasicRangedAttack(State<BasicAIStates> currentState)
    {
        return (RangedAttackActive == true);
    }

    public bool GuardBasicMoveToBasicBlock(State<BasicAIStates> currentState)
    {
        return (Block == true);
    }

    public bool GuardBasicMeleeToBasicMove(State<BasicAIStates> currentState)
    {
        return (MeleeDuration >= (2.05f + MeleeDurationIncrease));
    }

    public bool GuardBasicRangedAttackToBasicMove(State<BasicAIStates> currentState)
    {
        return (RangedAttackDuration >= 3.1f);
    }

    public bool GuardBasicBlockToBasicMove(State<BasicAIStates> currentState)
    {
        return (BlockDuration >= (1.6f + BlockDurationIncrease));
    }

    public void Start()
	{
		// Calls to Think function
		StartCoroutine(Think());
        randomRangedAttackValue = Random.Range(3.0f, 8.0f);
        randomBlock = Random.Range(1, 4);
        ChronosTransform = GameObject.Find("Chronos").transform;
        MinotaurAnimController = GetComponentInChildren<Animator>();
        MinotaurAnimController.SetBool("BrainActiveAnim", true);
    }

	public void Update()
	{
		base.Update ();
		// Refers to all act callbacks within other scripts
		if (basicAIStateActive) 
		{
			basicAIStateMachine.CurrentState.Act ();
		}

        ChronosTransform = GameObject.Find("Chronos").transform;

        randomTimer += Time.deltaTime;

        if (randomTimer >= randomRangedAttackValue)
            RangedAttackActive = true;

        if (Input.GetMouseButtonDown(0) && ClickCount == 0)
            randomBlock = Random.Range(1, 4);

        if (Input.GetMouseButtonDown(0) && ClickCount == 0 && randomBlock == 2)
        {
            ClickCount = 1;
            Block = true;
        }

        if (gameObject == null && Projectile != null)
            Destroy(Projectile);
    }

	protected IEnumerator Think()
	{
		yield return new WaitForSeconds(thinkInterval);
		// Checks state machine 
		basicAIStateMachine.Check();
		// Calls to Think function
		StartCoroutine(Think());
	}

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name == ("Chronos"))
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
