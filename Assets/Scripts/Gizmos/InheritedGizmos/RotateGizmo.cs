using UnityEngine;

public class RotateGizmo : BaseGizmo<RotateGizmo>
{

  void Awake()
  {
    gameObject.SetActive(false);
  }
}
