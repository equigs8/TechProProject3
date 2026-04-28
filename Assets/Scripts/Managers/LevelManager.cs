using UnityEngine;
using UnityEngine.SceneManagement; // Required for Scene switching

public class LevelManager : MonoBehaviour
{
    // Singleton so other scripts (like PauseManager or LossManager) can easily call it
    public static LevelManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Loads a scene by its exact name (e.g., "Level_01")
    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f; // Always unpause before loading
        SceneManager.LoadScene(sceneName);
    }

    // Loads a scene by its number in the Build Settings
    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    // Reloads the current active scene (Perfect for the "Retry" button)
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Automatically loads the next scene in your Build Settings order
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Ensure there is actually a next level to load
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("You beat the last level! Returning to Main Menu.");
            LoadMainMenu(); 
        }
    }

    // Shortcut for returning to the main menu (Assumes Main Menu is Build Index 0)
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }

    // Quits the application
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}