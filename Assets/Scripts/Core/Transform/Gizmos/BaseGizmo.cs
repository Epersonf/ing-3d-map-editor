using UnityEngine;

public abstract class BaseGizmo<T> : LazySingleton<T> where T : MonoBehaviour
{
    protected TransformableObject target;

    public virtual void Attach(TransformableObject t)
    {
        target = t;
        gameObject.SetActive(true);
        transform.position = t.TF.position;

        if (GizmoSettings.LocalSpace)
            transform.rotation = t.TF.rotation;
        else
            transform.rotation = Quaternion.identity;
    }

    public void Detach()
    {
        target = null;
        gameObject.SetActive(false);
    }

    protected virtual void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.TF.position;

        if (GizmoSettings.LocalSpace)
            transform.rotation = target.TF.rotation;
        else
            transform.rotation = Quaternion.identity;
    }
}
