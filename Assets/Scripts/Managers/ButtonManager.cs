using UnityEngine;
using UnityEngine.Events;

public class ButtonManager : MonoBehaviour
{
    public UnityEvent readyButton = new UnityEvent();
    public UnityEvent restartButton = new UnityEvent();

    public void ClickReadyButton() => readyButton.Invoke();
    public void RestartButton() => restartButton.Invoke();


    [Header("Buttons")]
    public GameObject readyButtonObject;

    public void TurnOnButton(string buttonName)
    {
        GameObject button = GameObject.Find(buttonName);
        if (button == null)
        {
            button = readyButtonObject;
        }
        button.SetActive(true);
    }
    public void TurnOffButton(string buttonName)
    {
        GameObject button = GameObject.Find(buttonName);
        button.SetActive(false);
    }

    

}