using UnityEngine;
using UnityEngine.UIElements;

public class FileSystemNavigatorUI : MonoBehaviour
{
    private UIDocument uiDocument;
    
    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        
        // Position the window
        var root = uiDocument.rootVisualElement;
        root.style.position = Position.Absolute;
        root.style.top = 50;
        root.style.left = 10;
        root.style.width = 350;
        root.style.height = 500;
    }
    
    // Optional: Add drag functionality to make window movable
    void Start()
    {
        var root = uiDocument.rootVisualElement;
        var header = root.Q<VisualElement>("header");
        
        if (header != null)
        {
            header.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 0) // Left mouse button
                {
                    root.CapturePointer(evt.pointerId);
                    evt.StopPropagation();
                }
            });
            
            header.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (root.HasPointerCapture(evt.pointerId))
                {
                    var delta = evt.deltaPosition;
                    var newX = root.style.left.value.value + delta.x;
                    var newY = root.style.top.value.value + delta.y;
                    
                    root.style.left = newX;
                    root.style.top = newY;
                }
            });
            
            header.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (root.HasPointerCapture(evt.pointerId))
                {
                    root.ReleasePointer(evt.pointerId);
                }
            });
        }
    }
}
