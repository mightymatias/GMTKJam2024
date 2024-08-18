using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource audioSource; // Drag the BackgroundMusic GameObject here

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
