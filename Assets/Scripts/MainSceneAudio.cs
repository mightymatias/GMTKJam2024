using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneAudio : MonoBehaviour
{

    public AudioSource MainSrc;

    public AudioClip pincer, fire;

    public void CutBread() // to make the button emit sound when pressed
    {
        MainSrc.clip = pincer;
        MainSrc.Play();
    }

    public void BlowFire()
    {
        MainSrc.clip = fire;
        MainSrc.Play();
    }






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
