using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TransformableObject : MonoBehaviour
{
  Collider col;

  public Dictionary<string, string> Tags { get; }
      = new Dictionary<string, string>();

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

    // Novo método para remover tags
    public bool RemoveTag(string key)
    {
        return Tags.Remove(key);
    }
}
