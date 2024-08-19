using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D pencilCursor;
    public Texture2D eraserCursor;
    public Texture2D clickCursor;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private Texture2D currentCursor;

    void Start()
    {
        // Set the default cursor at the start
        currentCursor = defaultCursor;
        Cursor.SetCursor(currentCursor, hotSpot, cursorMode);
    }

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Line"))
            {
                SetCursor(eraserCursor);
            }
            else if (hit.collider.CompareTag("Canvas"))
            {
                SetCursor(pencilCursor);
            }
            else
            {
                SetCursor(defaultCursor);
            }
        }
        else
        {
            SetCursor(defaultCursor);
        }
    }

    void HandleClick()
    {
        // Change the cursor when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(clickCursor, hotSpot, cursorMode);
        }

        // Revert back to the current hover cursor when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(currentCursor, hotSpot, cursorMode);
        }
    }

    void SetCursor(Texture2D cursor)
    {
        if (currentCursor != cursor)
        {
            currentCursor = cursor;
            Cursor.SetCursor(currentCursor, hotSpot, cursorMode);
        }
    }
}
