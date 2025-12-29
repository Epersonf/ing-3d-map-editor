using UnityEngine;

public enum GizmoMode { Move, Rotate, Scale }

public static class GizmoSettings
{
    public static GizmoMode Mode = GizmoMode.Move;

    public static bool SnapEnabled = false;
    public static float SnapUnit = 1f;

    public static bool LocalSpace = true;

    public static TransformableObject Current;

    public static void SetCurrent(TransformableObject t)
    {
        Current = t;
        RefreshGizmos();
    }

    public static void Clear()
    {
        Current = null;
        RefreshGizmos();
    }

    public static void SetMode(GizmoMode m)
    {
        Mode = m;
        RefreshGizmos();
    }

    static void RefreshGizmos()
    {
        var move = MoveGizmo.Instance;
        var rot  = RotateGizmo.Instance;
        var scl  = ScaleGizmo.Instance;

        bool hasTarget = Current != null;

        if (move) move.gameObject.SetActive(hasTarget && Mode == GizmoMode.Move);
        if (rot)  rot.gameObject.SetActive(hasTarget && Mode == GizmoMode.Rotate);
        if (scl)  scl.gameObject.SetActive(hasTarget && Mode == GizmoMode.Scale);

        if (!hasTarget) return;

        if (Mode == GizmoMode.Move && move) move.Attach(Current);
        if (Mode == GizmoMode.Rotate && rot) rot.Attach(Current);
        if (Mode == GizmoMode.Scale && scl) scl.Attach(Current);
    }
}
