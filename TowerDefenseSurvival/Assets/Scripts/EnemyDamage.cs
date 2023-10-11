using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int health = 100;

    private float attackRange;
    private float attackAngle = 45.0f;
    private float attackRate;
    private float nextAttack;

    private Player target;

    public EnemyData enemyData;
    public GameObject deathEffect;

    public bool isPlayerInRange;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            TakeDamage(other.gameObject.GetComponent<Bullet>().damage);
            other.gameObject.GetComponent<Bullet>().DestroyBullet();
        }
    }

    public void DealDamage(Player player, int damage)
    {
        player.TakeDamage(damage);
    }

    public void Attack(Player player)
    {
        if (Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            DealDamage(player, enemyData.damage);
        }
    }

    public void SODataToStats()
    {
        health = enemyData.health;
        attackRate = enemyData.attackRate;
        attackRange = enemyData.attackRange;
    }

    public void Start()
    {
        SODataToStats();
    }

    public void Update()
    {
        CheckForRange();
        if (isPlayerInRange)
        {
            Attack(target);
        }
    }

    void CheckForRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            isPlayerInRange = false;
            target = null;
            return;
        }

        target = player.GetComponent<Player>();
        Vector3 direction = target.gameObject.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (direction.magnitude <= attackRange && angle <= attackAngle)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
            target = null;
            player = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, attackAngle, 0) * transform.forward * attackRange);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -attackAngle, 0) * transform.forward * attackRange);
    }

}
