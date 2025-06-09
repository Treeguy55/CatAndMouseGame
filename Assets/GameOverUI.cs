using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverMenu;
    public PlayerMovement playerMovement; // drag your PlayerMovement script here in inspector

    public void ShowGameOver()
    {
        EventSystem.current.sendNavigationEvents = true;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseManager.IsPaused = true;
        playerMovement.DisableMovement(); // disable player movement on game over
    }
    public void RestartGame()
    {
        // Hide menus immediately
        if (gameOverMenu != null)
            gameOverMenu.SetActive(false);

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