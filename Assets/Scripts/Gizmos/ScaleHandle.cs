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

        float amount = Vector3.Dot(delta, axis);
        Vector3 s = target.TF.localScale;
        s += axis * amount;
        target.TF.localScale = Vector3.Max(s, Vector3.one * 0.01f);
    }
}
