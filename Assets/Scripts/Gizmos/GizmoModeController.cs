using UnityEngine;

public enum GizmoMode { Move, Rotate, Scale }

public class GizmoModeController : MonoBehaviour
{
    public static GizmoMode Mode { get; private set; } = GizmoMode.Move;
    static TransformableObject current;

    public static void SetMode(GizmoMode m)
    {
        Mode = m;
        Refresh();
    }

    public static void SetCurrent(TransformableObject t)
    {
        current = t;
        Refresh();
    }

    public static void Clear()
    {
        current = null;
        Refresh();
    }

    static void Refresh()
    {
        var move = MoveGizmo.Instance;
        var rot = RotateGizmo.Instance;
        var scl = ScaleGizmo.Instance;

        if (move != null) move.gameObject.SetActive(current != null && Mode == GizmoMode.Move);
        if (rot != null) rot.gameObject.SetActive(current != null && Mode == GizmoMode.Rotate);
        if (scl != null) scl.gameObject.SetActive(current != null && Mode == GizmoMode.Scale);

        if (current != null)
        {
            if (Mode == GizmoMode.Move && move != null) move.Attach(current);
            if (Mode == GizmoMode.Rotate && rot != null) rot.Attach(current);
            if (Mode == GizmoMode.Scale && scl != null) scl.Attach(current);
        }
    }
}
