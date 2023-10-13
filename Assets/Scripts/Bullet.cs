using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void SOData(TurretData data)
    {
        damage = data.damage;
    }

}
