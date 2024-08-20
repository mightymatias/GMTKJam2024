using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private CanvasGroup canvasGroup;

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

        // Get or add a CanvasGroup component
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Set the initial CanvasGroup properties
        canvasGroup.alpha = 0f; // Fully transparent initially
        canvasGroup.interactable = false; // No interaction
        canvasGroup.blocksRaycasts = false; // Allow clicks through the panel
    }

    public void StartFadeIn()
    {
        // Start the fade-in sequence
        StartCoroutine(FadeInSequence());
    }

    private IEnumerator FadeInSequence()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Enable interaction and raycast blocking
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Fade in the black screen and first text
        yield return FadeIn(blackScreen, screenColor);
        yield return FadeIn(messageText, textColor);

        // After the first text is fully visible, fade in the additional text
        yield return FadeIn(additionalText, additionalTextColor);

        // Wait for player input to return to main menu
        StartCoroutine(WaitForInput());
    }

    private IEnumerator FadeIn(Graphic uiElement, Color initialColor)
    {
        float timer = 0f;
        Color targetColor = initialColor;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            // Apply fade effect
            targetColor.a = alpha;
            uiElement.color = targetColor;

            // Update the CanvasGroup alpha if we're fading the blackScreen
            if (uiElement == blackScreen)
            {
                canvasGroup.alpha = alpha;
            }

            yield return null;
        }

        // Ensure full opacity after fade
        targetColor.a = 1f;
        uiElement.color = targetColor;

        // Ensure CanvasGroup alpha is fully set if we faded the blackScreen
        if (uiElement == blackScreen)
        {
            canvasGroup.alpha = 1f;
        }
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

    public void HidePanel()
    {
        // Disable interaction and raycast blocking
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Make the panel fully transparent
        canvasGroup.alpha = 0f;
    }
}
