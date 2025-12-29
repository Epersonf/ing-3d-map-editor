using UnityEngine;

public class ScaleGizmo : BaseGizmo<ScaleGizmo>
{

    void Awake()
    {
        gameObject.SetActive(false);
    }
}
