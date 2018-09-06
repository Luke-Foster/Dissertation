using System.Collections;
using UnityEngine;

public class HoundStateDrivenBrain : BasicAIController
{
    public enum HoundAIStates { HoundMove, HoundMelee };
    // Sets up state machine variable
    public FSM<HoundAIStates> houndAIStateMachine;
    // Puts a 0.4s interval on the state machine
    protected float thinkInterval = 0.4f;
    // Initialises chopperStateActive to true 
    public bool houndAIStateActive = true;

    public Transform ChronosTransform;
    public bool InMeleeRange = false;
    public bool MeleeContact = false;
    public bool InContactWithChronos = false;
    public float MeleeDuration = 0.0f;
    public float MovementSlow = 0.0f;
    public float MeleeDurationIncrease = 0.0f;
    public float MeleeContactIncrease = 0.0f;

    protected void Awake()
    {
        // Sets up a new FSM 
        houndAIStateMachine = new FSM<HoundAIStates>();
        // This adds in all the new states into the state machine using variables declared
        houndAIStateMachine.AddState(new HoundMove<HoundAIStates>(HoundAIStates.HoundMove, this, 0f));
        houndAIStateMachine.AddState(new HoundMelee<HoundAIStates>(HoundAIStates.HoundMelee, this, 0f));
        // Sets Idle as first state to be in once plays been activated
        houndAIStateMachine.SetInitialState(HoundAIStates.HoundMove);

        // These are all the transitions of the states I will require within this program
        houndAIStateMachine.AddTransition(HoundAIStates.HoundMove, HoundAIStates.HoundMelee);
        houndAIStateMachine.AddTransition(HoundAIStates.HoundMelee, HoundAIStates.HoundMove);
    }

    public bool GuardHoundMoveToHoundMelee(State<HoundAIStates> currentState)
    {
        return (InMeleeRange == true);
    }

    public bool GuardHoundMeleeToHoundMove(State<HoundAIStates> currentState)
    {
        return (MeleeDuration >= (1.2f + MeleeDurationIncrease));
    }

    void Start ()
    {
        // Calls to Think function
        StartCoroutine(Think());
        ChronosTransform = GameObject.Find("Chronos").transform;
    }
	
	void Update ()
    {
        base.Update();
        // Refers to all act callbacks within other scripts
        if (houndAIStateActive)
        {
            houndAIStateMachine.CurrentState.Act();
        }

        ChronosTransform = GameObject.Find("Chronos").transform;
    }

    protected IEnumerator Think()
    {
        yield return new WaitForSeconds(thinkInterval);
        // Checks state machine 
        houndAIStateMachine.Check();
        // Calls to Think function
        StartCoroutine(Think());
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == ("Chronos"))
        {
            InMeleeRange = true;
            InContactWithChronos = true;
            Debug.Log("Hitting chronos");
        }
    }

    void OnTriggerExit(Collider col)
    {
        InContactWithChronos = false;
    }
}
