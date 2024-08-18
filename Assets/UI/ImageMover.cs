using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// A component that moves a UI Image when the mouse pointer interacts with it.
/// Implements the IPointerEnterHandler, IPointerExitHandler, and IPointerClickHandler interfaces
/// to detect and respond to mouse events.
/// </summary>
public class ImageMover : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// The Image component that will be moved.
    /// </summary>
    public Image img;

    /// <summary>
    /// The Canvas that should move with the Image. Optional.
    /// </summary>
    public Canvas canvas;

    /// <summary>
    /// The RectTransform of the image, used to control its position.
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// The RectTransform of the Canvas if it needs to be moved.
    /// </summary>
    private RectTransform canvasRectTransform;

    /// <summary>
    /// The original position of the image before any interaction.
    /// </summary>
    private Vector2 originalPosition;

    /// <summary>
    /// The amount the image should move up when hovered over.
    /// </summary>
    public float hoverOffsetY = 25f;

    /// <summary>
    /// The amount the image should move up when clicked after hovering.
    /// </summary>
    public float clickOffsetY = 75f;

    /// <summary>
    /// The duration of the movement animation in seconds.
    /// Default is 0.2 seconds.
    /// </summary>
    public float moveDuration = 0.2f;

    private bool isHovered = false;

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

        // If a Canvas is assigned, cache its RectTransform as well.
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// Handles the pointer enter event when the mouse hovers over the image.
    /// Moves the image up slightly.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Move the image up slightly when hovered over.
        StopAllCoroutines();
        Vector2 hoverPosition = originalPosition + new Vector2(0, hoverOffsetY);
        StartCoroutine(MoveToPosition(hoverPosition));
        isHovered = true;
    }

    /// <summary>
    /// Handles the pointer exit event when the mouse stops hovering over the image.
    /// Returns the image to its original position.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Move the image back to its original position when the pointer exits.
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(originalPosition));
        isHovered = false;
    }

    /// <summary>
    /// Handles the click event on the image.
    /// Moves the image further up when clicked, if it has already been hovered over.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovered)
        {
            // Move the image further up when clicked.
            StopAllCoroutines();
            Vector2 clickPosition = originalPosition + new Vector2(0, clickOffsetY);
            StartCoroutine(MoveToPosition(clickPosition));
        }
    }

    /// <summary>
    /// Coroutine that smoothly moves the image (and optionally the canvas) to a target position over a set duration.
    /// </summary>
    /// <param name="targetPosition">The position to move the image to.</param>
    /// <returns>An IEnumerator that manages the animation over time.</returns>
    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        // Capture the start position of the image.
        Vector2 startPosition = rectTransform.anchoredPosition;

        // Capture the start position of the canvas if assigned.
        Vector2 canvasStartPosition = Vector2.zero;
        if (canvasRectTransform != null)
        {
            canvasStartPosition = canvasRectTransform.anchoredPosition;
        }

        // Track the time elapsed during the movement.
        float elapsedTime = 0;

        // Continue the animation until the duration is met.
        while (elapsedTime < moveDuration)
        {
            // Lerp between the start and target positions based on the elapsed time.
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);

            // Move the canvas along with the image if the canvas is assigned.
            if (canvasRectTransform != null)
            {
                Vector2 canvasTargetPosition = canvasStartPosition + (targetPosition - startPosition);
                canvasRectTransform.anchoredPosition = Vector2.Lerp(canvasStartPosition, canvasTargetPosition, elapsedTime / moveDuration);
            }

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            // Wait for the next frame before continuing.
            yield return null;
        }

        // Ensure the image is exactly at the target position after the animation completes.
        rectTransform.anchoredPosition = targetPosition;

        // Ensure the canvas is exactly at the correct position after the animation completes.
        if (canvasRectTransform != null)
        {
            canvasRectTransform.anchoredPosition = canvasStartPosition + (targetPosition - startPosition);
        }
    }
}
