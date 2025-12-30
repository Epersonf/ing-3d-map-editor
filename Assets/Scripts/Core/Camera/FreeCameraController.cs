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
    
    // Nova variável para armazenar estado da rotação
    bool isRightMouseDown = false;

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

        // Registrar callback para detectar quando botão direito é pressionado/solto
        rmbAction.started += OnRmbStarted;
        rmbAction.canceled += OnRmbCanceled;
    }

    void OnDisable()
    {
        // Remover callbacks
        rmbAction.started -= OnRmbStarted;
        rmbAction.canceled -= OnRmbCanceled;

        moveAction.Disable();
        lookAction.Disable();
        rmbAction.Disable();
        upAction.Disable();
        downAction.Disable();
        sprintAction.Disable();
    }

    void Update()
    {
        // Só processa movimento/rotação se botão direito estiver pressionado
        if (!isRightMouseDown)
            return;

        // MOVIMENTO (WASD, espaço, Ctrl)
        float speed = moveSpeed;
        if (sprintAction.ReadValue<float>() > 0)
            speed *= boostMultiplier;

        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector3 dir = transform.forward * move.y + transform.right * move.x;

        if (upAction.ReadValue<float>() > 0)
            dir += Vector3.up;
        if (downAction.ReadValue<float>() > 0)
            dir += Vector3.down;

        transform.position += dir * speed * Time.deltaTime;

        // ROTAÇÃO (arrastar mouse)
        Vector2 look = lookAction.ReadValue<Vector2>();
        if (look.sqrMagnitude > 0.01f) // Evitar micro-movimentos
        {
            yaw += look.x * lookSensitivity;
            pitch -= look.y * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    private void OnRmbStarted(InputAction.CallbackContext ctx)
    {
        isRightMouseDown = true;
    }

    private void OnRmbCanceled(InputAction.CallbackContext ctx)
    {
        isRightMouseDown = false;
    }
}
