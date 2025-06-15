using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float lookSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalRotation;
    private bool canMove = false;

    private PlayerControls controls;

    [HideInInspector]
    public bool isHidden = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!canMove || Time.timeScale == 0f) return;

        // Movement
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // Look
        Vector2 scaledLook = lookInput;
        if (Mouse.current != null && Mouse.current.delta.IsActuated())
            scaledLook *= 0.3f; // Adjust mouse sensitivity separately

        transform.Rotate(Vector3.up * scaledLook.x * lookSensitivity);
        verticalRotation -= scaledLook.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    public void EnableMovement()
    {
        canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisableMovement()
    {
        canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            isHidden = true;
            Debug.Log("Player is now hidden");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideZone"))
        {
            isHidden = false;
            Debug.Log("Player is no longer hidden");
        }
    }
}