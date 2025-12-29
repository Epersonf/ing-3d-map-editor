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
      GizmoSettings.SetCurrent(this);
    else
      GizmoSettings.Clear();
  }

  public Transform TF => transform;
}
