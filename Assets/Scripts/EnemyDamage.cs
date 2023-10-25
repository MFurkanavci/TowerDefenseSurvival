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

    private EnemyNAV enemyNAV;

    public bool isPlayerInRange;

    [SerializeField]
    private GameObject experience;

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
        GameObject effect = ObjectPooler.Instance.SpawnFromPool(deathEffect, transform.position, Quaternion.identity);
        effect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        effect.transform.rotation = Quaternion.Euler(-90, 0, 0);
        effect.transform.SetParent(transform.parent);
        effect.GetComponent<ParticleSystem>().Play();
        DropExperience();
        health = enemyData.health;

        ObjectPooler.Instance.ReturnObject(gameObject, gameObject);
    }
    public void DropExperience()
    {
        GameObject experience = ObjectPooler.Instance.SpawnFromPool(this.experience, transform.position, Quaternion.identity);
        experience.GetComponent<Experience>().experience = enemyData.experienceDrop;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bullet>(out Bullet bullet))
        {
            TakeDamage(bullet.damage);
            ObjectPooler.Instance.ReturnObject(other.gameObject, other.gameObject);
        }
        if (other.TryGetComponent<Player>(out Player player))
        {
            DealDamage(player, enemyData.damage);
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

            if (!player.IsAlive())
            {
                player.Die();
            }
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
        enemyNAV = GetComponent<EnemyNAV>();
        nextAttack = attackRate;
        attackRange = GetRandomStoppingDistance();
    }

    public void Update()
    {
        CheckForRange();
        if (isPlayerInRange && enemyNAV.agent.remainingDistance <= attackRange && target.IsAlive())
        {
            FaceTarget();
            enemyNAV.agent.velocity = Vector3.zero;
            Attack(target);
        }
    }

    public float GetRandomStoppingDistance()
    {
        return Random.Range(1.0f, attackRange);
    }

    public void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, enemyNAV.agent.angularSpeed * Time.deltaTime);
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
