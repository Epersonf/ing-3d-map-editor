using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GizmoToolbar : MonoBehaviour
{
    [SerializeField] Button modeButton;
    [SerializeField] Button gridButton;
    [SerializeField] Button spaceButton;

    [SerializeField] TMP_Text modeLabel;
    [SerializeField] TMP_Text gridLabel;
    [SerializeField] TMP_Text spaceLabel;

    void Awake()
    {
        modeButton.onClick.AddListener(OnModeClicked);
        gridButton.onClick.AddListener(OnGridClicked);
        spaceButton.onClick.AddListener(OnSpaceClicked);
        Refresh();
    }

    void OnModeClicked()
    {
        switch (GizmoSettings.Mode)
        {
            case GizmoMode.Move: GizmoSettings.SetMode(GizmoMode.Rotate); break;
            case GizmoMode.Rotate: GizmoSettings.SetMode(GizmoMode.Scale); break;
            case GizmoMode.Scale: GizmoSettings.SetMode(GizmoMode.Move); break;
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
        modeLabel.text = $"Mode: {GizmoSettings.Mode}";
        gridLabel.text = GizmoSettings.SnapEnabled ? "Grid: On" : "Grid: Off";
        spaceLabel.text = GizmoSettings.LocalSpace ? "Space: Local" : "Space: Global";
    }
}
