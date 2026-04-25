using UnityEngine;

public class HowToPlayManager : MonoBehaviour
{
    public GameObject[] menus = new GameObject[4];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Updatemenu(int index)
    {
        if (index < 0 || index > menus.Length)
            return;

        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(i == index);
        }
    }
}
