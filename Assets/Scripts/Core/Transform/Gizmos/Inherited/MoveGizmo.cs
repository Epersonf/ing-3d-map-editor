using UnityEngine;

public class MoveGizmo : BaseGizmo<MoveGizmo>
{
    [SerializeField] AxisHandle x;
    [SerializeField] AxisHandle y;
    [SerializeField] AxisHandle z;

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
