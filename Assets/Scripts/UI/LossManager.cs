using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LossManager : MonoBehaviour
{

    public string[] GameTips = {"a","b","You Suck!"};
     public GameObject LossScreen;
     public TextMeshProUGUI GameTipsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameLost(){
        Time.timeScale = 0; //stop game from progressing
        LossScreen.gameObject.SetActive(true);


    }

    public void Retry(){
        Time.timeScale = 1; //Start game time
        LossScreen.gameObject.SetActive(false);
        //return to previous wave??

    }

    public void MainMenu(){
        //Debug.Log("MainMenu!");
        //Debug.Log(GameTips[Random.Range(0,GameTips.Length)]);
        //Debug.Log(GameTips[0]);
        GameTipsText.text = GameTips[Random.Range(0,GameTips.Length)];
        
    }
}
