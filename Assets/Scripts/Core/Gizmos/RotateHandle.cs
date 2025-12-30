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

    public void Apply(Vector3 delta)
    {
        if (!target) return;

        Vector3 worldAxis =
            GizmoSettings.LocalSpace
            ? target.TF.TransformDirection(axis)
            : axis;

        float amount = delta.magnitude * Mathf.Sign(Vector3.Dot(delta, worldAxis)) * 5f;

        if (GizmoSettings.SnapEnabled)
            amount = Mathf.Round(amount / 15f) * 15f; // 15 degrees

        target.TF.Rotate(worldAxis, amount, Space.World);
    }
}
