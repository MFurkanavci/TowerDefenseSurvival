using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int health = 100;
    public GameObject deathEffect;
    public GameObject player;
    public GameObject enemy;
    public GameObject enemyBullet;
    public GameObject enemyBulletSpawn;
    public float bulletSpeed = 30.0f;
    public float bulletLife = 3.0f;
    public float fireRate = 0.5f;

    private float nextFire = 0.0f;

    private void Update()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //Fire();
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(enemyBullet, enemyBulletSpawn.transform.position, enemyBulletSpawn.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        Destroy(bullet, bulletLife);
    }

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
            TakeDamage(25);
        }
    }
}
