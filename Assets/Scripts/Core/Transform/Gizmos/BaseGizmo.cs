using UnityEngine;

public abstract class BaseGizmo<T> : LazySingleton<T> where T : MonoBehaviour
{
    protected TransformableObject target;

    public virtual void Attach(TransformableObject t)
    {
        target = t;
        gameObject.SetActive(true);
        transform.position = t.TF.position;
    }

    public void Detach()
    {
        target = null;
        gameObject.SetActive(false);
    }

    protected virtual void LateUpdate()
    {
        if (target != null)
            transform.position = target.TF.position;
    }
}
