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
      GizmoModeController.SetCurrent(this);
    else
      GizmoModeController.Clear();
  }

  public Transform TF => transform;
}
