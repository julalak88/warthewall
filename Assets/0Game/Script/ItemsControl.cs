using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsControl : MonoBehaviour
{
    public Player currentPlayer;
    public Button powerup_btn;
    public Button dbAttack_btn;
    public Button heal_btn;

    public GameObject powerGroup;
    public Image powerMeterImage;


    void Start()
    {
        ResetPowerUI();
    }
    public void GetHealItems()
    {
        heal_btn.interactable = false;
        currentPlayer.SetHeal();
    }

    public void PowerThrow()
    {
        powerup_btn.interactable = false;
        currentPlayer.isPowerThrow = true;
        currentPlayer.isDoubleAttack = false;
    }

    public void DoubleAttack()
    {
        dbAttack_btn.interactable = false;
        currentPlayer.isDoubleAttack = true;
        currentPlayer.isPowerThrow = false;
    }

    public void ResetPowerUI()
    {
        powerMeterImage.fillAmount = 0f;
        powerGroup.SetActive(false);
    }
    public void UpdatePowerUI(float powerMeter, float maxPower)
    {

        powerMeterImage.fillAmount = powerMeter / maxPower;

    }
}
