using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RotateHandle : MonoBehaviour
{
    TransformableObject target;
    Vector3 axis;

    public TransformableObject Target => target;

    public void Bind(TransformableObject t, Vector3 a)
    {
        target = t;
        axis = a.normalized;
    }

    public void Apply(Vector3 delta, Quaternion baseRot)
    {
        if (!target) return;

        Vector3 worldAxis =
            GizmoSettings.LocalSpace
            ? target.TF.TransformDirection(axis)
            : axis;

        float signed = Vector3.Dot(delta, worldAxis);

        float degrees = signed * 300f; // adjustable gain

        if (GizmoSettings.SnapEnabled)
            degrees = Mathf.Round(degrees / 15f) * 15f;

        target.TF.rotation =
            Quaternion.AngleAxis(degrees, worldAxis) * baseRot;
    }
}
