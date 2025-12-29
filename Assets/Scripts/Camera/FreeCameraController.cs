using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class FreeCameraController : MonoBehaviour
{
    public static FreeCameraController Instance { get; private set; }

    public float moveSpeed = 8f;
    public float lookSensitivity = 0.2f;
    public float boostMultiplier = 3f;

    float yaw;
    float pitch;

    InputAction moveAction;
    InputAction lookAction;
    InputAction rmbAction;
    InputAction upAction;
    InputAction downAction;
    InputAction sprintAction;

    public Camera MainCamera { get; private set; }

    void Awake()
    {
        Instance = this;
        MainCamera = GetComponent<Camera>();
    }

    void OnEnable()
    {
        moveAction = new InputAction("Move", binding: "<Keyboard>/w/s/a/d");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        lookAction = new InputAction("Look", binding: "<Mouse>/delta");
        rmbAction = new InputAction("RMB", binding: "<Mouse>/rightButton");
        upAction = new InputAction("Up", binding: "<Keyboard>/space");
        downAction = new InputAction("Down", binding: "<Keyboard>/leftCtrl");
        sprintAction = new InputAction("Sprint", binding: "<Keyboard>/leftShift");

        moveAction.Enable();
        lookAction.Enable();
        rmbAction.Enable();
        upAction.Enable();
        downAction.Enable();
        sprintAction.Enable();

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        rmbAction.Disable();
        upAction.Disable();
        downAction.Disable();
        sprintAction.Disable();
    }

    void Update()
    {
        float speed = moveSpeed;
        if (sprintAction.ReadValue<float>() > 0) speed *= boostMultiplier;

        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector3 dir = transform.forward * move.y + transform.right * move.x;

        if (upAction.ReadValue<float>() > 0) dir += Vector3.up;
        if (downAction.ReadValue<float>() > 0) dir += Vector3.down;

        transform.position += dir * speed * Time.deltaTime;

        if (rmbAction.ReadValue<float>() > 0)
        {
            Vector2 look = lookAction.ReadValue<Vector2>();
            yaw += look.x * lookSensitivity;
            pitch -= look.y * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
