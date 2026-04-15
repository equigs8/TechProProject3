using UnityEngine;

public class HowtoPlayManager : MonoBehaviour
{

    public GameObject[] tips;

    

    // Update is called once per frame
    public void UpdateTip(int index)
    {
        if (index < 0 || index >= tips.Length)
        {
            return;
        }
        for (int i = 0; i < tips.Length; i++)
        {
            tips[i].SetActive(i == index);
        }
    }
}
