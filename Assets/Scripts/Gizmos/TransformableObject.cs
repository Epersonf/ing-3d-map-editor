using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TransformableObject : MonoBehaviour
{
  Collider col;

  void Awake()
  {
    col = GetComponent<Collider>();
  }

  public void SetSelected(bool selected)
  {
    col.enabled = !selected;

    if (selected)
    {
      MoveGizmo.Instance.Attach(this);
      RotateGizmo.Instance.Attach(this);
      ScaleGizmo.Instance.Attach(this);
    }
    else
    {
      MoveGizmo.Instance.Detach();
      RotateGizmo.Instance.Detach();
      ScaleGizmo.Instance.Detach();
    }
  }

  public Transform TF => transform;
}
