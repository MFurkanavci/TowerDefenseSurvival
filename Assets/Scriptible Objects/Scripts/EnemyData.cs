using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int health;
    public int damage;
    public float speed;
    public float attackRange;
    public float attackRate;
    public int experienceDrop;
    public int soulDrop;

    public GameObject enemyPrefab;
}