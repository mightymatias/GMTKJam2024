using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsMainMenu : MonoBehaviour
{

    public AudioSource AudioSrc;

    public AudioClip background1, sfx1, sfx2;

    public void Button1() // to make the button emit sound when pressed
    {
        AudioSrc.clip = sfx1;
        AudioSrc.Play();
    }

    public void ButtonHover()
    {
        AudioSrc.clip = sfx2;
        AudioSrc.Play();
    }


    // Start is called before the first frame update
    void Start() //to make the background music start playing right when the game starts
    {
        AudioSrc.clip = background1;
        AudioSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
