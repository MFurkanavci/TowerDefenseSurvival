using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [Header("Player")]
    public Player player;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerLevel;
    public TextMeshProUGUI playerExperience;
    public TextMeshProUGUI playerMaxExperience;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerMaxHealth;

    [Header("Player Stats")]
    public TextMeshProUGUI playerExperienceMultiplier;
    public TextMeshProUGUI playerHealthMultiplier;
    public TextMeshProUGUI playerMaxWeightMultiplier;
    public TextMeshProUGUI playerResourceGainMultiplier;
    public TextMeshProUGUI playerResourceGatherSpeedMultiplier;

    [Header("Player Movement")]
    public TextMeshProUGUI playerSpeed;
    public TextMeshProUGUI playerOverWeightSpeed;

    [Header("Weapon")]
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponID;
    public TextMeshProUGUI weaponDescription;
    public TextMeshProUGUI weaponDamage;
    public TextMeshProUGUI weaponAttackRange;
    public TextMeshProUGUI weaponAttackRate;
    public TextMeshProUGUI weaponMaxAmmo;
    public TextMeshProUGUI weaponAmmo;
    public TextMeshProUGUI weaponReloadTime;
    public TextMeshProUGUI weaponRecoil;
    public TextMeshProUGUI weaponCriticalChance;
    public TextMeshProUGUI weaponCriticalDamage;

    [Header("Weapon Stats")]
    public TextMeshProUGUI weaponDamageMultiplier;
    public TextMeshProUGUI weaponAttackRangeMultiplier;
    public TextMeshProUGUI weaponAttackRateMultiplier;
    public TextMeshProUGUI weaponMaxAmmoMultiplier;
    public TextMeshProUGUI weaponReloadTimeMultiplier;
    public TextMeshProUGUI weaponRecoilMultiplier;
    public TextMeshProUGUI weaponCriticalChanceMultiplier;
    public TextMeshProUGUI weaponCriticalDamageMultiplier;

    public GameObject talentsPanel;

    void UpdatePlayerUI()
    {
        playerHealth.text = $"Palyer Health:{player.health} / {player.maxHealth}";
    }

    void Update()
    {
        UpdatePlayerUI();
    }

    public void OpenTalents()
    { 
        if (talentsPanel.activeSelf)
        {
            talentsPanel.SetActive(false);
        }
        else
        {
            talentsPanel.SetActive(true);
        }
    }
}
