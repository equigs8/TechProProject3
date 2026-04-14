using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton for easy access from enemies

    public enum GameState { BuildingPhase, InWave, GameOver, Tutorial };

    public GameState gameState;
    public int currentWave;
    public int enemiesAlive; // Tracks remaining enemies

    public EnemySpawner enemySpawner;
    public BuildingManager buildingManager;
    public ButtonManager buttonManager;
    public ResourceManager resourceManager;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void OnEnable()
    {
        SubscribeToEvents();
    }

    void Start()
    {
        currentWave = 0;
        gameState = GameState.BuildingPhase;
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.BuildingPhase:
                resourceManager.StopOilProduction();
                break;
            case GameState.InWave:
                resourceManager.StartOilProduction();
                // Check if all enemies are defeated to return to BuildingPhase
                if (enemiesAlive <= 0)
                {
                    EndWave();
                }
                break;
            case GameState.GameOver:
                break;
            case GameState.Tutorial:
                break;
        }
    }

    void SubscribeToEvents()
    {
        buttonManager.readyButton.AddListener(ReadyButtonClicked);
    }

    void ReadyButtonClicked()
    {
        if (gameState != GameState.BuildingPhase) return;

        buttonManager.TurnOffButton("Ready Button");
        gameState = GameState.InWave;
        currentWave++; // Increment wave number

        // Start spawning with scaled enemy count
        enemySpawner.StartWave(currentWave);
    }

    public void EnemyDestroyed()
    {
        enemiesAlive--; // Called by enemies when they die
    }

    void EndWave()
    {
        Debug.Log("Wave Cleared! Returning to Building Phase.");
        gameState = GameState.BuildingPhase;
        buttonManager.TurnOnButton("Ready Button"); // Re-enable the button for the next wave
    }
}