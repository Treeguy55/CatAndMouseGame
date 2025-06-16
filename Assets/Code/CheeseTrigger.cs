using UnityEngine;

public class CheeseTrigger : MonoBehaviour
{
    public GameWinUI gameWinUI; // assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameWinUI.ShowGameWin();
        }
    }
}
