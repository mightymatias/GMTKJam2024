using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// A component that moves a UI Image when the mouse pointer interacts with it.
/// Implements the IPointerClickHandler, IPointerEnterHandler, and IPointerExitHandler interfaces
/// to detect and respond to mouse events.
/// </summary>
public class ImageMover : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// The Image component that will be moved.
    /// </summary>
    public Image img;

    /// <summary>
    /// The RectTransform of the image, used to control its position.
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// The original position of the image before any interaction.
    /// </summary>
    private Vector2 originalPosition;

    /// <summary>
    /// The amount by which the image will be moved when interacted with.
    /// Default is a 50-pixel move upward.
    /// </summary>
    public Vector2 moveOffset = new Vector2(0, 50); 

    /// <summary>
    /// The duration of the movement animation in seconds.
    /// Default is 0.2 seconds.
    /// </summary>
    public float moveDuration = 0.2f;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the rectTransform and originalPosition, and sets the alpha hit test threshold.
    /// </summary>
    private void Start()
    {
        // Set the alpha hit test threshold to ignore transparent parts of the image for interaction.
        img.alphaHitTestMinimumThreshold = 0.5f;

        // Cache the RectTransform component and store the original position.
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    /// <summary>
    /// Handles the click event on the image.
    /// Moves the image by the specified offset.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Stop any ongoing movement animations and start a new one to move the image.
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(rectTransform.anchoredPosition + moveOffset));
    }

    /// <summary>
    /// Handles the pointer enter event when the mouse hovers over the image.
    /// Moves the image by the specified offset.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Stop any ongoing movement animations and start a new one to move the image.
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(rectTransform.anchoredPosition + moveOffset));
    }

    /// <summary>
    /// Handles the pointer exit event when the mouse stops hovering over the image.
    /// Returns the image to its original position.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Stop any ongoing movement animations and start a new one to move the image back to its original position.
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(originalPosition));
    }

    /// <summary>
    /// Coroutine that smoothly moves the image to a target position over a set duration.
    /// </summary>
    /// <param name="targetPosition">The position to move the image to.</param>
    /// <returns>An IEnumerator that manages the animation over time.</returns>
    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        // Capture the start position of the image.
        Vector2 startPosition = rectTransform.anchoredPosition;

        // Track the time elapsed during the movement.
        float elapsedTime = 0;

        // Continue the animation until the duration is met.
        while (elapsedTime < moveDuration)
        {
            // Lerp between the start and target positions based on the elapsed time.
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            // Wait for the next frame before continuing.
            yield return null;
        }

        // Ensure the image is exactly at the target position after the animation completes.
        rectTransform.anchoredPosition = targetPosition;
    }
}
