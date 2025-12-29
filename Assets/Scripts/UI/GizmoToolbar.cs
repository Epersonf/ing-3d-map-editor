using UnityEngine;
using UnityEngine.UIElements;

public class GizmoToolbar : MonoBehaviour
{
    UIDocument doc;

    Button modeButton;
    Button gridButton;
    Button spaceButton;

    bool gridEnabled = false;
    bool localSpace = true;

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
        switch (GizmoModeController.Mode)
        {
            case GizmoMode.Move:
                GizmoModeController.SetMode(GizmoMode.Rotate);
                break;

            case GizmoMode.Rotate:
                GizmoModeController.SetMode(GizmoMode.Scale);
                break;

            case GizmoMode.Scale:
                GizmoModeController.SetMode(GizmoMode.Move);
                break;
        }

        Refresh();
    }

    void OnGridClicked()
    {
        gridEnabled = !gridEnabled;
        SnapSettings.Enabled = gridEnabled;
        Refresh();
    }

    void OnSpaceClicked()
    {
        localSpace = !localSpace;
        GizmoSpace.Local = localSpace;
        Refresh();
    }

    void Refresh()
    {
        modeButton.text = $"Mode: {GizmoModeController.Mode}";
        gridButton.text = gridEnabled ? "Grid: On" : "Grid: Off";
        spaceButton.text = localSpace ? "Space: Local" : "Space: Global";
    }
}
