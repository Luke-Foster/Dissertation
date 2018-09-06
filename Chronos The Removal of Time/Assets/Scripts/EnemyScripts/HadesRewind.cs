using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesRewind : MonoBehaviour
{
    bool isRewinding = false;
    float RecordTime = 5f;
    List<HadesPointInTime> hadesPointsInTime;
    HadesStateDrivenBrain HadesSDB;
    public float ButtonDownTimer = 4.5f;
    public float Cooldown = 10.0f;
    int PressCount = 0;
    public bool RewindActive = false;
    bool CooldownActive = false;
    Animator HadesAnimController;

    void Start()
    {
        hadesPointsInTime = new List<HadesPointInTime>();
        HadesSDB = GetComponent<HadesStateDrivenBrain>();
        HadesAnimController = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("4") && RewindActive == false && CooldownActive == false)
        {
            StartRewind();
            Debug.Log("Hades Rewinding");
            PressCount = 1;
            Cooldown = 10.0f;
            RewindActive = true;
        }
        if (Input.GetKeyUp("4"))
        {
            StopRewind();
            Debug.Log("Hades Not Rewinding");
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

        if (hadesPointsInTime.Count > 0 && Input.GetKey("4") && PressCount == 1 && ButtonDownTimer > 0.0f && CooldownActive == false)
        {
            HadesPointInTime pointInTime = hadesPointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            hadesPointsInTime.RemoveAt(0);
            HadesAnimController.SetBool("BrainActiveAnim", false);
            HadesAnimController.SetBool("MeleeAnim", false);
            HadesAnimController.SetBool("RangedAnim", false);
            HadesAnimController.SetBool("BlockAnim", false);
            HadesAnimController.SetBool("AbilityAnim", false);
            HadesSDB.enabled = false;
        }
        else
        {
            StopRewind();
            RewindActive = false;
        }
    }

    void Record()
    {
        if (hadesPointsInTime.Count > Mathf.Round(RecordTime / Time.fixedDeltaTime))
        {
            hadesPointsInTime.RemoveAt(hadesPointsInTime.Count - 1);
        }

        hadesPointsInTime.Insert(0, new HadesPointInTime(transform.position, transform.rotation));
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
