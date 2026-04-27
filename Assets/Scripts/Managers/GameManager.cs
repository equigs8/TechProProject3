using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public enum GameState { BuildingPhase, InWave, GameOver, Tutorial };

    public GameState gameState;
    public int currentWave;
    public int enemiesAlive; 

    public Target[] targets;
    public EnemySpawner enemySpawner;
    public BuildingManager buildingManager;
    public ButtonManager buttonManager;
    public ResourceManager resourceManager;
    public UIManager uiManager;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        currentWave = 0;
        SubscribeToEvents();
        
        // Initialize the first state
        ChangeState(GameState.BuildingPhase);
    }

    void Update()
    {
        if (gameState == GameState.InWave)
        {
            // Check if ALL targets are destroyed
            bool anyTargetAlive = false;
            foreach (Target t in targets)
            {
                if (t != null && t.health > 0)
                {
                    anyTargetAlive = true;
                    break; 
                }
            }

            if (!anyTargetAlive)
            {
                ChangeState(GameState.GameOver);
            }
        }
    }

    // A new method to handle transitioning between states cleanly
    public void ChangeState(GameState newState)
    {
        gameState = newState;

        switch (gameState)
        {
            case GameState.BuildingPhase:
                resourceManager.StopOilProduction();
                uiManager.BuildingPhaseUI(true);
                buttonManager.TurnOnButton("Ready Button"); // Ensure button is on
                break;

            case GameState.InWave:
                resourceManager.StartOilProduction();
                uiManager.BuildingPhaseUI(false);
                break;

            case GameState.GameOver:
                uiManager.GameOverUI(true);
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
        
        currentWave++; 
        
        // Calculate enemies for this wave (e.g., 5 base + 2 per wave)
        enemiesAlive = 5 + (currentWave * 2); 

        // Start the wave and pass the count to the spawner
        ChangeState(GameState.InWave);
        enemySpawner.StartWave(currentWave, enemiesAlive);
    }

    // Called by enemies when they die via: GameManager.instance.EnemyDestroyed();
    public void EnemyDestroyed()
    {
        enemiesAlive--; 

        // We only need to check if the wave is over when an enemy actually dies!
        if (gameState == GameState.InWave && enemiesAlive <= 0)
        {
            EndWave();
        }
    }

    void EndWave()
    {
        Debug.Log("Wave Cleared! Returning to Building Phase.");
        ChangeState(GameState.BuildingPhase);
    }
}