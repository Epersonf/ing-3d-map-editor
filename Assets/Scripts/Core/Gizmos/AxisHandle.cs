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
        float d = Vector3.Dot(delta, axis);
        target.TF.position = basePos + axis * d;
    }
}
