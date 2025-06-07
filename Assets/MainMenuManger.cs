using UnityEditor; // Required for UnityEditor.EditorApplication
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Drag your menu GameObject here in the inspector
    public GameObject mainMenuUI;

    public void StartGame()
    {
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameState.gameStarted = true;
       
        
       

    }

    public void QuitGame()
    {
        Debug.Log("Quit clicked");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
