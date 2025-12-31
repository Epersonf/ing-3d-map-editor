using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerCatcher : MonoBehaviour
{
    void Awake()
    {
        AddCatchers();
    }

    void AddCatchers()
    {
        var rects = Object.FindObjectsByType<RectTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var rect in rects)
        {
            if (rect.TryGetComponent<UIRaycastCatcher>(out _))
                continue;
            rect.gameObject.AddComponent<UIRaycastCatcher>();
        }
    }
}
