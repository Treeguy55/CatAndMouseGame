using UnityEngine;

public class CheeseTrigger : MonoBehaviour
{
    public GameOverUI gameOverUI; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameOverUI.ShowGameOver();
        }
    }
}
