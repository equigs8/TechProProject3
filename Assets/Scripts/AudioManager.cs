using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource background;
    public AudioSource player;

    [Header("Audio Clips")]
    public AudioClip goodClick;

    public AudioClip badClick;
    
    public Audio Clip music;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void PlayMusic(){
        background.clip = music;
        background.Play();

    }

    public void GoodClick(){
        player.clip = goodClick;
        player.Play();
    }

     public void BadClick(){
        player.clip = badClick;
        player.Play();
    }

    
}
