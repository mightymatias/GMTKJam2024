using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageMover : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image img;
    public Canvas canvas;

    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;
    private Vector2 originalPosition;
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

        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
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
        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, eventData.pressEventCamera))
        {
            // If not, return to the original position
            StopAllCoroutines();
            StartCoroutine(MoveToPosition(originalPosition));
            isHovered = false;
            isClicked = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovered)
        {
            StopAllCoroutines();
            Vector2 clickPosition = originalPosition + new Vector2(0, clickOffsetY);
            StartCoroutine(MoveToPosition(clickPosition));
            isClicked = true; // Flag to prevent hover movement after click
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 canvasStartPosition = Vector2.zero;

        if (canvasRectTransform != null)
        {
            canvasStartPosition = canvasRectTransform.anchoredPosition;
        }

        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);

            if (canvasRectTransform != null)
            {
                Vector2 canvasTargetPosition = canvasStartPosition + (targetPosition - startPosition);
                canvasRectTransform.anchoredPosition = Vector2.Lerp(canvasStartPosition, canvasTargetPosition, elapsedTime / moveDuration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;

        if (canvasRectTransform != null)
        {
            canvasRectTransform.anchoredPosition = canvasStartPosition + (targetPosition - startPosition);
        }
    }
}
