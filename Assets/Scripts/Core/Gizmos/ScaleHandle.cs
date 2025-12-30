using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScaleHandle : MonoBehaviour
{
    TransformableObject target;
    Vector3 axis;

    public TransformableObject Target => target;

    public void Bind(TransformableObject t, Vector3 a)
    {
        target = t;
        axis = a.normalized;
    }

    public void Apply(Vector3 delta)
    {
        if (!target) return;

        Vector3 worldAxis =
            GizmoSettings.LocalSpace
            ? target.TF.TransformDirection(axis)
            : axis;

        float amount = Vector3.Dot(delta, worldAxis);

        Vector3 s = target.TF.localScale;
        s += axis * amount;

        if (GizmoSettings.SnapEnabled)
        {
            s.x = Mathf.Round(s.x / GizmoSettings.SnapUnit) * GizmoSettings.SnapUnit;
            s.y = Mathf.Round(s.y / GizmoSettings.SnapUnit) * GizmoSettings.SnapUnit;
            s.z = Mathf.Round(s.z / GizmoSettings.SnapUnit) * GizmoSettings.SnapUnit;
        }

        target.TF.localScale = Vector3.Max(s, Vector3.one * 0.01f);
    }
}
