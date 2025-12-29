using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    static InputController instance;

    [SerializeField] LayerMask hitMask;
    [SerializeField] float dragSensitivity = 5f;

    TransformableObject current;
    AxisHandle dragging;
    Vector3 dragStartPos;
    Vector3 objStartPos;

    InputAction click;
    InputAction pointer;

    void Awake()
    {
        instance = this;

        click = new InputAction("Click", binding: "<Mouse>/leftButton");
        pointer = new InputAction("Pointer", binding: "<Pointer>/position");

        click.Enable();
        pointer.Enable();
    }

    void Update()
    {
        if (click.WasPressedThisFrame())
        {
            Ray r = FreeCameraController.Instance.MainCamera.ScreenPointToRay(pointer.ReadValue<Vector2>());
            if (Physics.Raycast(r, out var hit, 200f, hitMask))
            {
                if (hit.collider.TryGetComponent(out AxisHandle handle))
                {
                    dragging = handle;
                    dragStartPos = hit.point;
                    objStartPos = handle.transform.parent.position;
                    return;
                }

                if (hit.collider.TryGetComponent(out TransformableObject obj))
                {
                    if (current != null) current.SetSelected(false);
                    current = obj;
                    current.SetSelected(true);
                    return;
                }
            }

            if (current != null) current.SetSelected(false);
            current = null;
        }

        if (click.IsPressed() && dragging != null)
        {
            Ray r = FreeCameraController.Instance.MainCamera.ScreenPointToRay(pointer.ReadValue<Vector2>());
            if (Physics.Raycast(r, out var hit, 200f))
            {
                Vector3 delta = hit.point - dragStartPos;
                dragging.Apply(delta * dragSensitivity, objStartPos);
            }
        }

        if (click.WasReleasedThisFrame())
            dragging = null;
    }
}