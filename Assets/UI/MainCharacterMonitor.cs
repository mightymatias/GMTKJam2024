using UnityEngine;

public class MainCharacterMonitor : MonoBehaviour
{
    public GameObject mainCharacter;
    public FadeInScreen fadeInScript;
    public AudioManager audioManager;
    public AudioClip newAudioClip; // New audio clip to transition to

    void Update()
    {
        if (mainCharacter == null)
        {
            // Trigger the fade-in effect
            fadeInScript.StartFadeIn();

            // Fade out current music and switch to new clip
            audioManager.FadeOutAndSwitch(newAudioClip);

            Destroy(this); // Optional: Destroy this script since it has fulfilled its purpose
        }
    }
}
