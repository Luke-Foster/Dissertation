using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurRewind : MonoBehaviour
{
    bool isRewinding = false;
    float RecordTime = 5f;
    List<MinotaurPointInTime> minotaurPointsInTime;
    BasicStateDrivenBrain BSDB;
    public float ButtonDownTimer = 4.5f;
    public float Cooldown = 10.0f;
    int PressCount = 0;
    public bool RewindActive = false;
    bool CooldownActive = false;
    Animator MinotaurAnimController;

    void Start ()
    {
        minotaurPointsInTime = new List<MinotaurPointInTime>();
        BSDB = GetComponent<BasicStateDrivenBrain>();
        MinotaurAnimController = GetComponentInChildren<Animator>();
    }
	
	void Update ()
    {
        if(Input.GetKeyDown("4") && RewindActive == false && CooldownActive == false)
        {
            StartRewind();
            Debug.Log("Minotaur Rewinding");
            PressCount = 1;
            Cooldown = 10.0f;
            RewindActive = true;
        }
        if (Input.GetKeyUp("4"))
        {
            StopRewind();
            Debug.Log("Minotaur Not Rewinding");
            ButtonDownTimer = 4.5f;
            PressCount = 0;
        }

        if (CooldownActive == true)
        {
            Cooldown -= Time.deltaTime;

            if (Cooldown <= 0.0f)
                CooldownActive = false;
        }
    }

    void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    void Rewind()
    {
        ButtonDownTimer -= Time.deltaTime;

        if (minotaurPointsInTime.Count > 0 && Input.GetKey("4") && PressCount == 1 && ButtonDownTimer > 0.0f && CooldownActive == false)
        {
            MinotaurPointInTime pointInTime = minotaurPointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            minotaurPointsInTime.RemoveAt(0);
            BSDB.enabled = false;
            MinotaurAnimController.SetBool("BrainActiveAnim", false);
            MinotaurAnimController.SetBool("MeleeAnim", false);
            MinotaurAnimController.SetBool("RangedAnim", false);
            MinotaurAnimController.SetBool("BlockAnim", false);
        }
        else
        {
            StopRewind();
            RewindActive = false;
        }
    }

    void Record()
    {
        if(minotaurPointsInTime.Count > Mathf.Round(RecordTime / Time.fixedDeltaTime))
        {
            minotaurPointsInTime.RemoveAt(minotaurPointsInTime.Count - 1);
        }

        minotaurPointsInTime.Insert(0, new MinotaurPointInTime(transform.position, transform.rotation));
    }

    void StartRewind()
    {
        isRewinding = true;
    }

    void StopRewind()
    {
        isRewinding = false;
        CooldownActive = true;
    }
}
