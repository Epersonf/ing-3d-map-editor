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

        float amount = delta.magnitude * Mathf.Sign(Vector3.Dot(delta, axis));
        target.TF.Rotate(axis, amount * 5f, Space.World);
    }
}
