using UnityEngine;
using System;

public enum GameState
{
    MainMenu,
    Playing,
    Respawning,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState gameState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetGameState(GameState.Playing);
    }

    public void SetGameState(GameState state)
    {
        gameState = state;

        switch (gameState)
        {
            case GameState.MainMenu:
                MainMenu();
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

        OnGameStateChanged?.Invoke(gameState);
    }

    private void MainMenu()
    {

    }

    private void Playing()
    {

    }

    private void Respawning()
    {

    }

    private void Paused()
    {

    }

    private void GameOver()
    {

    }
}