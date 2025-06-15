using UnityEngine;
using UnityEngine.InputSystem; // <-- add this for new Input System

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool IsPaused = false;

    public SimpleTimer simpleTimer;

    void Update()
    {
        // Check keyboard Escape key
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        // Check controller Start button (common pause button)
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        if (simpleTimer != null)
            simpleTimer.StartTimer();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        if (simpleTimer != null)
            simpleTimer.PauseTimer();
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}