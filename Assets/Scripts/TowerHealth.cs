using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerHealth : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI infoText;

    private void Update()
    {
        if (health <= 0)
        {
            infoText.text = "You Lose!";
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
    healthText.text = $"Tower Health: {health}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(25);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.transform.root.CompareTag("EnemyAggro"))
            {
                return;
            }
            other.gameObject.GetComponent<EnemyDamage>().Die();
            TakeDamage(10);
        }
    }
}
