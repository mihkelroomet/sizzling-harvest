using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Texture2D CursorPointer;
    public Vector2 CursorPointerHotspot;
    public Texture2D CursorDefault;
    public Vector2 CursorDefaultHotspot;
    
    public void OnMouseEnter()
    {
        Cursor.SetCursor(CursorPointer, CursorPointerHotspot, CursorMode.ForceSoftware);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(CursorDefault, CursorDefaultHotspot, CursorMode.ForceSoftware);
    }
}
