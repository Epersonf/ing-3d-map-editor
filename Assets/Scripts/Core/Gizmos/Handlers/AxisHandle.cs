using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AxisHandle : MonoBehaviour
{
    TransformableObject target;
    Vector3 axis;

    public TransformableObject Target => target;

    public void Bind(TransformableObject t, Vector3 a)
    {
        target = t;
        axis = a.normalized;
    }

    public void Apply(Vector3 delta, Vector3 basePos)
    {
        if (target == null) return;

        Vector3 worldAxis =
            GizmoSettings.LocalSpace
            ? target.TF.TransformDirection(axis)
            : axis;

        float d = Vector3.Dot(delta, worldAxis);
        Vector3 pos = basePos + worldAxis * d;

        if (GizmoSettings.SnapEnabled)
        {
            pos.x = Mathf.Round(pos.x / GizmoSettings.SnapUnit) * GizmoSettings.SnapUnit;
            pos.y = Mathf.Round(pos.y / GizmoSettings.SnapUnit) * GizmoSettings.SnapUnit;
            pos.z = Mathf.Round(pos.z / GizmoSettings.SnapUnit) * GizmoSettings.SnapUnit;
        }

        target.TF.position = pos;
    }
}
