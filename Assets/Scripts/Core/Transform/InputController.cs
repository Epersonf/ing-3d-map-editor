using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputController : MonoBehaviour
{
    static InputController instance;

    [SerializeField] LayerMask hitMask;
    [SerializeField] float dragSensitivity = 5f;

    TransformableObject current;
    AxisHandle dragging;
    RotateHandle rotating;
    ScaleHandle scaling;
    Plane dragPlane;
    Vector3 dragStartPos;
    Vector3 objStartPos;
    Quaternion startRot;
    Vector3 startScale;

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
        // ---- BLOQUEIA CLIQUE EM UI ----
        var doc = FindFirstObjectByType<UIDocument>();
        if (doc != null)
        {
            var panel = doc.rootVisualElement.panel;
            var pos = pointer.ReadValue<Vector2>();

            if (panel != null && panel.Pick(pos) != null)
            {
                // cancela interação 3D
                return;
            }
        }
        // --------------------------------

        if (click.WasPressedThisFrame())
        {
            Ray r = FreeCameraController.Instance.MainCamera.ScreenPointToRay(pointer.ReadValue<Vector2>());
            if (Physics.Raycast(r, out var hit, 200f, hitMask))
            {
                if (hit.collider.TryGetComponent(out AxisHandle axis))
                {
                    dragging = axis;

                    Ray rr = FreeCameraController.Instance.MainCamera.ScreenPointToRay(pointer.ReadValue<Vector2>());
                    dragPlane = new Plane(FreeCameraController.Instance.MainCamera.transform.forward, axis.Target.TF.position);

                    dragPlane.Raycast(rr, out float enter);
                    dragStartPos = rr.GetPoint(enter);

                    objStartPos = axis.Target.TF.position;
                    return;
                }

                if (hit.collider.TryGetComponent(out RotateHandle rot))
                {
                    rotating = rot;

                    Ray rr = FreeCameraController.Instance.MainCamera.ScreenPointToRay(pointer.ReadValue<Vector2>());
                    dragPlane = new Plane(FreeCameraController.Instance.MainCamera.transform.forward, rot.Target.TF.position);

                    dragPlane.Raycast(rr, out float enterR);
                    dragStartPos = rr.GetPoint(enterR);
                    startRot = rot.Target.TF.rotation;
                    return;
                }

                if (hit.collider.TryGetComponent(out ScaleHandle scl))
                {
                    scaling = scl;

                    Ray rr = FreeCameraController.Instance.MainCamera.ScreenPointToRay(pointer.ReadValue<Vector2>());
                    dragPlane = new Plane(FreeCameraController.Instance.MainCamera.transform.forward, scl.Target.TF.position);

                    dragPlane.Raycast(rr, out float enterS);
                    dragStartPos = rr.GetPoint(enterS);
                    startScale = scl.Target.TF.localScale;
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

        if (click.IsPressed())
        {
            Ray rr = FreeCameraController.Instance.MainCamera.ScreenPointToRay(pointer.ReadValue<Vector2>());

            if (dragPlane.Raycast(rr, out float enter))
            {
                Vector3 planePoint = rr.GetPoint(enter);
                Vector3 delta = planePoint - dragStartPos;

                if (dragging != null) dragging.Apply(delta * dragSensitivity, objStartPos);
                if (rotating != null) rotating.Apply(delta * dragSensitivity, startRot);
                if (scaling != null) scaling.Apply(delta * dragSensitivity, startScale);
            }
        }

        if (click.WasReleasedThisFrame())
        {
            dragging = null;
            rotating = null;
            scaling = null;
        }
    }
}