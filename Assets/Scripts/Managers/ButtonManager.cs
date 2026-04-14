using UnityEngine;
using UnityEngine.Events;

public class ButtonManager : MonoBehaviour
{
    public UnityEvent readyButton = new UnityEvent();

    public void ClickReadyButton() => readyButton.Invoke();


    public void TurnOnButton(string buttonName)
    {
        GameObject button = GameObject.Find(buttonName);
        button.SetActive(true);
    }
    public void TurnOffButton(string buttonName)
    {
        GameObject button = GameObject.Find(buttonName);
        button.SetActive(false);
    }

}