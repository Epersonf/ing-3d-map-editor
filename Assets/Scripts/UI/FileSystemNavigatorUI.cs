using UnityEngine;
using UnityEngine.EventSystems;

public class FileSystemNavigatorUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    RectTransform panel;
    Vector2 offset;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panel, e.position, e.pressEventCamera, out offset);
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panel.parent as RectTransform, e.position, e.pressEventCamera, out pos))
        {
            panel.localPosition = pos - offset;
        }
    }
}