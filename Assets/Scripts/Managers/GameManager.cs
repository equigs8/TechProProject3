using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState { BuildingPhase, InWave, GameOver, Tutorial };

    public GameState gameState;
    public int currentWave;

    public EnemySpawner enemySpawner;
    public BuildingManager buildingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWave = 0;
        gameState = GameState.BuildingPhase;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.BuildingPhase:
                break;
            case GameState.InWave:
                break;
            case GameState.GameOver:
                break;
            case GameState.Tutorial:
                break;
        }
    }
}
