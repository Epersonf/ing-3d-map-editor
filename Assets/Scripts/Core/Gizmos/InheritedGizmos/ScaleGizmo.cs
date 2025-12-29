using UnityEngine;

public class ScaleGizmo : BaseGizmo<ScaleGizmo>
{
    [SerializeField] ScaleHandle x;
    [SerializeField] ScaleHandle y;
    [SerializeField] ScaleHandle z;

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
