using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{

    AudioSource audsrc;

    // Start is called before the first frame update
    void Start()
    {
        audsrc = GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play_sound()
    {
        audsrc.Play();
    }

}
