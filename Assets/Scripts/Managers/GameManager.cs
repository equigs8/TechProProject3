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

    public TextMeshProUGUI[] legHealthTexts;

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
        ChangeMaxOil(resourceManager.GetMaxOil());
    }

    void Update()
    {   
        UpdateLegHealthUI()
        ChangeMaxOil(resourceManager.GetMaxOil());
        if(gameState == GameState.BuildingPhase)
        {
            resourceManager.StopOilProduction();
            uiManager.UpdateOil(resourceManager.GetCurrentOil());
        }
        if (gameState == GameState.InWave)
        {
            resourceManager.StartOilProduction();
            uiManager.UpdateOil(resourceManager.GetCurrentOil());
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

    void UpdateLegHealthUI()
    {
        // Prevent errors if the arrays aren't set up yet
        if (targets == null || legHealthTexts == null) return;

        for (int i = 0; i < targets.Length; i++)
        {
            // Make sure we have a corresponding text element assigned for this index
            if (i < legHealthTexts.Length && legHealthTexts[i] != null)
            {
                if (targets[i] != null && targets[i].health > 0)
                {
                    
                    legHealthTexts[i].text = "HP: " + targets[i].health.ToString("0"); 
                }
                else
                {
                    
                    legHealthTexts[i].text = "Destroyed";
                    legHealthTexts[i].color = Color.red; 
                }
            }
        }
    }
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
        buttonManager.restartButton.AddListener(RestartGame);
    }

    void RestartGame()
    {
        LevelManager.instance.RestartLevel();
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

    public void ChangeMaxOil(int amount)
    {
        uiManager.UpdateOilMax(resourceManager.GetMaxOil());
    }
}