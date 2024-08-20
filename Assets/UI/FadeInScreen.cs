using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // For loading the main menu scene
using UnityEngine.UI;

public class FadeInScreen : MonoBehaviour
{
    public Image blackScreen;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI additionalText;
    public float fadeDuration = 2f; // Duration of the fade-in

    private Color screenColor;
    private Color textColor;
    private Color additionalTextColor;

    void Start()
    {
        // Initialize colors
        screenColor = blackScreen.color;
        textColor = messageText.color;
        additionalTextColor = additionalText.color;

        // Set initial alpha to 0
        screenColor.a = 0f;
        textColor.a = 0f;
        additionalTextColor.a = 0f;

        // Apply the initial colors
        blackScreen.color = screenColor;
        messageText.color = textColor;
        additionalText.color = additionalTextColor;
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeInSequence());
    }

    private IEnumerator FadeInSequence()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Fade in the black screen and first text
        yield return StartCoroutine(FadeIn(blackScreen, screenColor));
        yield return StartCoroutine(FadeIn(messageText, textColor));

        // After the first text is fully visible, fade in the additional text
        yield return StartCoroutine(FadeIn(additionalText, additionalTextColor));

        // Wait for player input to return to main menu
        StartCoroutine(WaitForInput());
    }

    private IEnumerator FadeIn(Graphic uiElement, Color targetColor)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = timer / fadeDuration;

            // Apply fade effect
            targetColor.a = Mathf.Clamp01(alpha);
            uiElement.color = targetColor;

            yield return null;
        }

        // Ensure full opacity after fade
        targetColor.a = 1f;
        uiElement.color = targetColor;
    }

    private IEnumerator WaitForInput()
    {
        while (true)
        {
            // Check for mouse click
            if (Input.GetMouseButtonDown(0))
            {
                // Resume the game before changing the scene
                Time.timeScale = 1f;

                // Load the main menu scene (replace "MainMenu" with your scene name)
                SceneManager.LoadScene("MainMenu");
                yield break;
            }
            yield return null;
        }
    }
}
