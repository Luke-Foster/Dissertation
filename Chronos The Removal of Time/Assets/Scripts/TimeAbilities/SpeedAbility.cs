using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedAbility : MonoBehaviour
{
    GameObject Chronos;
    PlayerController playerController;
    public float abilityTimer = 0.0f;
    public float Cooldown = 6.0f;
    private bool speedAbilityActive = false;
    public GameObject TwoActive;
    public GameObject TwoCoolDownTextActive;
    public Text TwoCoolDownText;
    Animator ChronosAnimController;

    void Start ()
    {
        Chronos = GameObject.Find("Chronos");
        playerController = Chronos.GetComponent<PlayerController>();
        ChronosAnimController = GetComponentInChildren<Animator>();
    }
	
	void Update ()
    {
        if (speedAbilityActive == false)
        {
            if (Input.GetKeyDown("2"))
            {
                playerController.MovementSpeedBoost = 1.5f;
                playerController.AttackSpeedBoost = 0.3f;
                ChronosAnimController.speed = 1.5f;
                abilityTimer = 0.0f;
                Cooldown = 6.0f;
                TwoActive.SetActive(true);
                speedAbilityActive = true;
                Debug.Log("Speed Ability has been Activated");
            }
        }
        else
        {
            abilityTimer += Time.deltaTime;
        }

        if(abilityTimer >= 6.0f)
        {
            playerController.MovementSpeedBoost = 1.0f;
            playerController.AttackSpeedBoost = 0.0f;
            ChronosAnimController.speed = 1.0f;

            TwoCoolDownTextActive.SetActive(true);
            Cooldown -= Time.deltaTime;
            string CooldownToString = ((int)Cooldown + 1).ToString();
            TwoCoolDownText.text = CooldownToString;

            if(Cooldown <= 0.0f)
            {
                speedAbilityActive = false;
                TwoActive.SetActive(false);
                TwoCoolDownTextActive.SetActive(false);
                abilityTimer = 0.0f;
            }
        }

	}
}
