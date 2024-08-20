using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageMover
    : MonoBehaviour,
        IPointerClickHandler,
        IPointerEnterHandler,
        IPointerExitHandler
{
    public Image img;
    public Canvas canvas;
    public LineGenerator lineGenerator; // Reference to the LineGenerator
    public AudioClip clickSound; // Sound to play on click

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private AudioSource audioSource; // Reference to the AudioSource component
    public float hoverOffsetY = 25f;
    public float clickOffsetY = 75f;
    public float moveDuration = 0.2f;

    private bool isHovered = false;
    private bool isClicked = false;

    private void Start()
    {
        img.alphaHitTestMinimumThreshold = 0.5f;
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource if it's missing
        }
    }

    private void Update()
    {
        // Handle the situation where the user is drawing on the canvas.
        if (lineGenerator != null && lineGenerator.IsDrawingLine())
        {
            // If the user is actively drawing a line, do not move the image back.
            return;
        }

        // If the left mouse button is clicked and the object is currently clicked
        if (Input.GetMouseButtonDown(0) && isClicked)
        {
            // Check if the pointer is neither over the game object nor its children, and not over the canvas
            if (!IsPointerOverGameObjectAndChildren() && !IsPointerOverCanvas())
            {
                // Only move back if the click was outside the target object and the canvas
                MoveBackToOriginalPosition();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked) // Only move on hover if not clicked
        {
            StopAllCoroutines();
            Vector2 hoverPosition = originalPosition + new Vector2(0, hoverOffsetY);
            StartCoroutine(MoveToPosition(hoverPosition));
            isHovered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Check if the pointer is still over the image or any of its child elements
        if (
            !RectTransformUtility.RectangleContainsScreenPoint(
                rectTransform,
                Input.mousePosition,
                eventData.pressEventCamera
            )
        )
        {
            StopAllCoroutines();
            StartCoroutine(MoveToPosition(originalPosition));
            isHovered = false;
            isClicked = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (lineGenerator != null && lineGenerator.IsDrawingLine())
        {
            // Ignore the click if a line is being drawn
            return;
        }

        if (isHovered)
        {
            StopAllCoroutines();
            Vector2 clickPosition = originalPosition + new Vector2(0, clickOffsetY);
            StartCoroutine(MoveToPosition(clickPosition));
            isClicked = true; // Flag to prevent hover movement after click

            // Play the click sound
            if (clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                startPosition,
                targetPosition,
                elapsedTime / moveDuration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    private bool IsPointerOverGameObjectAndChildren()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            Input.mousePosition,
            null
        );
    }

    private bool IsPointerOverCanvas()
    {
        // Check if the pointer is over the canvas (used for line drawing)
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Create a list to hold all the raycast results
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        // Check if any of the raycast results are related to the canvas or its children
        foreach (var result in raycastResults)
        {
            if (result.gameObject.transform.IsChildOf(canvas.transform))
            {
                return true;
            }
        }

        return false;
    }

    private void MoveBackToOriginalPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(originalPosition));
        isHovered = false;
        isClicked = false;
    }
}
