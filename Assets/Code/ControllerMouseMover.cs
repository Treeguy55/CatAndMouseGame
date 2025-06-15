using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMouseMover : MonoBehaviour
{
    public GameObject mainMenuUI;
    public float cursorSpeed = 1000f;

    private Vector2 cursorPos;
    private bool menuWasActive;

    void Start()
    {
        cursorPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Mouse.current.WarpCursorPosition(cursorPos);
    }

    void Update()
    {
        if (mainMenuUI == null) return;

        bool menuActive = mainMenuUI.activeSelf;

        // Transition: Menu just became active
        if (menuActive && !menuWasActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorPos = Mouse.current.position.ReadValue();
            Mouse.current.WarpCursorPosition(cursorPos);
        }

        // Transition: Menu just closed
        if (!menuActive && menuWasActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Move mouse with left stick while in menu
        if (menuActive && Gamepad.current != null)
        {
            Vector2 input = Gamepad.current.leftStick.ReadValue();
            if (input.sqrMagnitude > 0.01f)
            {
                cursorPos += input * cursorSpeed * Time.unscaledDeltaTime;
                cursorPos.x = Mathf.Clamp(cursorPos.x, 0, Screen.width);
                cursorPos.y = Mathf.Clamp(cursorPos.y, 0, Screen.height);
                Mouse.current.WarpCursorPosition(cursorPos);
            }
        }

        menuWasActive = menuActive;
    }
}