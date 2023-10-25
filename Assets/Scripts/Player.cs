using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Datas")]
    public PlayerData playerData;
    public WeaponData weaponData;

    public GameObject model;

    [Header("Stats")]
    public int level;
    public int experience;
    public int maxExperience;
    public int maxHealth;
    public int health;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case GameState.Respawning:
                break;
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
        }
    }

    public void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    public void Start()
    {
        
        InitializeDataToStats();
    }

    public void InitializeDataToStats()
    {
        playerData.Initialize();
        level = playerData.level;
        experience = playerData.experience;
        maxExperience = playerData.maxExperience;
        maxHealth = playerData.maxHealth;
        health = playerData.health;
    }

    public bool IsAlive()
    {
        return health > 1;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Heal(int heal)
    {
        health += heal;
    }

    public void AddExperience(int experience)
    {
        this.experience += experience;
    }

    public void LevelUp()
    {
        level++;
        experience = 0;
        maxExperience += maxExperience * 2;
    }

    public bool IsLevelUp()
    {
        return experience >= maxExperience;
    }

    private void Update()
    {
    }

    private async void RespawnTimer()
    {
        await Task.Delay(10000);
        GameManager.Instance.SetGameState(GameState.Playing);
        health = maxHealth;
    }

    public void Die()
    {
        GameManager.Instance.SetGameState(GameState.Respawning);
        model.SetActive(false);
        RespawnTimer();
    }
}
