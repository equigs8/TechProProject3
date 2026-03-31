using UnityEngine;


public class PauseManager : MonoBehaviour
{
    public bool isPaused;
    public GameObject PauseMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            isPaused = !isPaused;
            if (isPaused){
                PauseGame();
            } else
            {
                ResumeGame();

            }

        }
            

    
    }


    public void PauseGame(){
        Time.timeScale = 0;
        PauseMenu.gameObject.SetActive(true);


    }

    public void ResumeGame(){
        Time.timeScale = 1;
        PauseMenu.gameObject.SetActive(false);


    }

    public void Settings(){
        Debug.Log("Settings!");
    }

    public void MainMenu(){
        Debug.Log("MainMenu!");
    }

    public void Quit(){
        Debug.Log("Quit!");
    }




}
