using UnityEngine;
using UnityEngine.UI;

public class TransparentRaycastImage : Image
{
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        // Convert the screen point to local point in the rectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPoint, eventCamera, out Vector2 localPoint);

        // Convert local point to texture coordinate
        Rect rect = GetPixelAdjustedRect();
        localPoint.x += rectTransform.pivot.x * rect.width;
        localPoint.y += rectTransform.pivot.y * rect.height;

        // Calculate the texture coordinate relative to the size of the image
        Vector2 uv = new Vector2(localPoint.x / rect.width, localPoint.y / rect.height);

        // Get the texture coordinate from the sprite
        uv = sprite.rect.position + uv * sprite.rect.size;

        // Get the pixel from the texture
        Texture2D texture = sprite.texture;
        Color color = texture.GetPixel((int)uv.x, (int)uv.y);

        // Return true if the alpha value is greater than a threshold, e.g., 0.1
        return color.a > 0.1f;
    }
}
