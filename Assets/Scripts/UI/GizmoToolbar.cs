using UnityEngine;
using UnityEngine.UIElements;

public class GizmoToolbar : MonoBehaviour
{
    UIDocument doc;

    Button modeButton;
    Button gridButton;
    Button spaceButton;

    void Awake()
    {
        doc = GetComponent<UIDocument>();

        modeButton = doc.rootVisualElement.Q<Button>("modeButton");
        gridButton = doc.rootVisualElement.Q<Button>("gridButton");
        spaceButton = doc.rootVisualElement.Q<Button>("spaceButton");

        modeButton.clicked += OnModeClicked;
        gridButton.clicked += OnGridClicked;
        spaceButton.clicked += OnSpaceClicked;

        Refresh();
    }

    void OnModeClicked()
    {
        switch (GizmoSettings.Mode)
        {
            case GizmoMode.Move:   GizmoSettings.SetMode(GizmoMode.Rotate); break;
            case GizmoMode.Rotate: GizmoSettings.SetMode(GizmoMode.Scale);  break;
            case GizmoMode.Scale:  GizmoSettings.SetMode(GizmoMode.Move);   break;
        }

        Refresh();
    }

    void OnGridClicked()
    {
        GizmoSettings.SnapEnabled = !GizmoSettings.SnapEnabled;
        Refresh();
    }

    void OnSpaceClicked()
    {
        GizmoSettings.LocalSpace = !GizmoSettings.LocalSpace;
        Refresh();
    }

    void Refresh()
    {
        modeButton.text = $"Mode: {GizmoSettings.Mode}";
        gridButton.text = GizmoSettings.SnapEnabled ? "Grid: On" : "Grid: Off";
        spaceButton.text = GizmoSettings.LocalSpace ? "Space: Local" : "Space: Global";
    }
}
