using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D clickCursor; 
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // Set the default cursor at the start
        Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
    }

    void Update()
    {
        // Check if the left mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(clickCursor, hotSpot, cursorMode);
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
        }
    }
}
