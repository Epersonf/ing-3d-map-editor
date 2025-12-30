using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIPointerCatcher : MonoBehaviour
{
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null || uiDocument.rootVisualElement == null)
        {
            Debug.LogError("UIPointerCatcher: UIDocument or rootVisualElement is null.");
            return;
        }

        // Registrar para eventos de ponteiro no rootVisualElement
        uiDocument.rootVisualElement.RegisterCallback<PointerDownEvent>(OnPointerDown, TrickleDown.TrickleDown);
        // Registrar também para ClickEvent como fallback
        uiDocument.rootVisualElement.RegisterCallback<ClickEvent>(OnClick, TrickleDown.TrickleDown);
    }

    void OnPointerDown(PointerDownEvent evt)
    {
        UIBlocker.ClickedUI = true;
    }

    void OnClick(ClickEvent evt)
    {
        UIBlocker.ClickedUI = true;
    }
}
