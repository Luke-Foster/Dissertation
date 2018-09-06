using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundRewind : MonoBehaviour
{
    bool isRewinding = false;
    float RecordTime = 5f;
    List<HoundPointInTime> houndPointsInTime;
    HoundStateDrivenBrain HSDB;
    public float ButtonDownTimer = 4.5f;
    public float Cooldown = 10.0f;
    int PressCount = 0;
    public bool RewindActive = false;
    bool CooldownActive = false;

    void Start()
    {
        houndPointsInTime = new List<HoundPointInTime>();
        HSDB = GetComponent<HoundStateDrivenBrain>();
    }

    void Update()
    {
        if (Input.GetKeyDown("4") && RewindActive == false && CooldownActive == false)
        {
            StartRewind();
            Debug.Log("Hound Rewinding");
            PressCount = 1;
            Cooldown = 10.0f;
            RewindActive = true;
        }
        if (Input.GetKeyUp("4"))
        {
            StopRewind();
            Debug.Log("Hound Not Rewinding");
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

        if (houndPointsInTime.Count > 0 && Input.GetKey("4") && PressCount == 1 && ButtonDownTimer > 0.0f && CooldownActive == false)
        {
            HoundPointInTime pointInTime = houndPointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            houndPointsInTime.RemoveAt(0);
            HSDB.enabled = false;
        }
        else
        {
            StopRewind();
            RewindActive = false;
        }
    }

    void Record()
    {
        if (houndPointsInTime.Count > Mathf.Round(RecordTime / Time.fixedDeltaTime))
        {
            houndPointsInTime.RemoveAt(houndPointsInTime.Count - 1);
        }

        houndPointsInTime.Insert(0, new HoundPointInTime(transform.position, transform.rotation));
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
