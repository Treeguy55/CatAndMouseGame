using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameWinUI : MonoBehaviour
{
    public GameObject gameWinMenu;
    public PlayerMovement playerMovement; // assign in Inspector

    public void ShowGameWin()
    {
        EventSystem.current.sendNavigationEvents = true;
        gameWinMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseManager.IsPaused = true;
        playerMovement.DisableMovement(); // stop movement on win
    }

    public void RestartGame()
    {
        if (gameWinMenu != null)
            gameWinMenu.SetActive(false);

        MainMenuManager mainMenu = FindObjectOfType<MainMenuManager>();
        if (mainMenu != null && mainMenu.mainMenuUI != null)
            mainMenu.mainMenuUI.SetActive(false);

        if (playerMovement != null)
            playerMovement.EnableMovement();

        PauseManager.IsPaused = false;
        Time.timeScale = 1f;

        GameState.gameStarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
