using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindTimeAbility : MonoBehaviour
{
    bool isRewinding = false;
    public float recordTime = 5f;
    public float ButtonDownTimer = 4.5f;
    public float Cooldown = 10.0f;
    int PressCount = 0;
    bool RewindActive = false;
    bool CooldownActive = false;
    public static List<PointInTime> pointsInTime;
    PlayerController playerController;
    HealthManager HM;
    SpeedAbility speedAbility;
    SlowTimeAbility STA;
    FreezeTimeAbility FTA;
    Rigidbody rb;
    public GameObject FourActive;
    public GameObject FourCoolDownTextActive;
    public Text FourCoolDownText;
    Animator ChronosAnimController;

    void Start ()
    {
        pointsInTime = new List<PointInTime>();
        playerController = GetComponent<PlayerController>();
        speedAbility = GetComponent<SpeedAbility>();
        STA = GetComponent<SlowTimeAbility>();
        FTA = GetComponent<FreezeTimeAbility>();
        rb = GetComponent<Rigidbody>();
        HM = GetComponent<HealthManager>();
        ChronosAnimController = GetComponentInChildren<Animator>();
    }

    void Update ()
    {
        if (Input.GetKeyDown("4") && RewindActive == false && CooldownActive == false)
        {
            StartRewind();
            Debug.Log("Rewinding");
            FourActive.SetActive(true);
            PressCount = 1;
            Cooldown = 10.0f;
            RewindActive = true;
        }
    
        if (Input.GetKeyUp("4"))
        {
            StopRewind();
            ButtonDownTimer = 4.5f;
            PressCount = 0;
            playerController.enabled = true;
            speedAbility.enabled = true;
            STA.enabled = true;
            FTA.enabled = true;
            Debug.Log("Not Rewinding");
        }

        if(CooldownActive == true)
        {
            FourCoolDownTextActive.SetActive(true);
            Cooldown -= Time.deltaTime;
            string CooldownToString = ((int)Cooldown + 1).ToString();
            FourCoolDownText.text = CooldownToString;

            if (Cooldown <= 0.0f)
            {
                FourCoolDownTextActive.SetActive(false);
                FourActive.SetActive(false);
                CooldownActive = false;
            }
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

        if (pointsInTime.Count > 0 && Input.GetKey("4") && PressCount == 1 && ButtonDownTimer > 0.0f && CooldownActive == false)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            HM.ChronosHealth = pointInTime.chronosHealth;
            pointsInTime.RemoveAt(0);
            ChronosAnimController.SetBool("Forward", false);
            ChronosAnimController.SetBool("ForwardLeft", false);
            ChronosAnimController.SetBool("ForwardRight", false);
            ChronosAnimController.SetBool("Left", false);
            ChronosAnimController.SetBool("Right", false);
            ChronosAnimController.SetBool("Backward", false);
            ChronosAnimController.SetBool("BackwardLeft", false);
            ChronosAnimController.SetBool("BackwardRight", false);
            ChronosAnimController.SetBool("Melee", false);
            ChronosAnimController.SetBool("RangedAttack", false);
            ChronosAnimController.SetBool("Jump", false);
            playerController.enabled = false;
            speedAbility.enabled = false;
            STA.enabled = false;
            FTA.enabled = false;
        }
        else
        {
            StopRewind();
            playerController.enabled = true;
            speedAbility.enabled = true;
            STA.enabled = true;
            FTA.enabled = true;
            RewindActive = false;
        }
    }

    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, HM.ChronosHealth));
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        CooldownActive = true;
    }
}