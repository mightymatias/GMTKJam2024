using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TransparentRaycast : MonoBehaviour, IPointerClickHandler
{
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        // Convert the screen point to local point in the rectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform, screenPoint, eventCamera, out Vector2 localPoint);

        // Convert local point to texture coordinate
        Rect rect = image.sprite.rect;
        localPoint.x += image.rectTransform.pivot.x * rect.width;
        localPoint.y += image.rectTransform.pivot.y * rect.height;

        // Calculate the texture coordinate relative to the size of the image
        Vector2 uv = new Vector2(localPoint.x / rect.width, localPoint.y / rect.height);

        // Convert uv into texture space
        Texture2D texture = image.sprite.texture;
        Vector2 texCoords = new Vector2(
            Mathf.Lerp(image.sprite.rect.x, image.sprite.rect.xMax, uv.x / image.rectTransform.rect.width),
            Mathf.Lerp(image.sprite.rect.y, image.sprite.rect.yMax, uv.y / image.rectTransform.rect.height)
        );

        // Get the pixel color
        Color color = texture.GetPixel((int)texCoords.x, (int)texCoords.y);

        // Only return true if the alpha is greater than 0
        return color.a > 0.1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsRaycastLocationValid(eventData.position, eventData.pressEventCamera))
        {
            Debug.Log($"{gameObject.name} was clicked and it's visible!");
            // Your logic to move or interact with the image goes here
        }
        else
        {
            Debug.Log($"{gameObject.name} was clicked, but it's transparent!");
        }
    }
}
