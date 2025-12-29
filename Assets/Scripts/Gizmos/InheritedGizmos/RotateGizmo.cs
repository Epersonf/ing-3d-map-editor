using UnityEngine;

public class RotateGizmo : BaseGizmo<RotateGizmo>
{
    [SerializeField] RotateHandle x;
    [SerializeField] RotateHandle y;
    [SerializeField] RotateHandle z;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Attach(TransformableObject t)
    {
        base.Attach(t);
        x.Bind(t, Vector3.right);
        y.Bind(t, Vector3.up);
        z.Bind(t, Vector3.forward);
    }
}
