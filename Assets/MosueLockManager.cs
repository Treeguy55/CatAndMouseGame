using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosueLockManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!GameState.gameStarted || PauseManager.IsPaused)
        {
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }
}
