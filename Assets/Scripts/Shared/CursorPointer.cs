using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D customCursor;
    private Vector2 hotspot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
