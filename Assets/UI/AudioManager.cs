using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public float fadeDuration = 2f; // Duration of the fade in/out

    public void FadeOutAndSwitch(AudioClip newClip)
    {
        StartCoroutine(FadeOutInTransition(newClip));
    }

    private IEnumerator FadeOutInTransition(AudioClip newClip)
    {
        // Fade out the current audio using unscaledDeltaTime
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;

        // Switch to the new clip
        audioSource.clip = newClip;
        audioSource.loop = false; // Ensure the new clip does not loop
        audioSource.Play();

        // Fade in the new audio using unscaledDeltaTime
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}
