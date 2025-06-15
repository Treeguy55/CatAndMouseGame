using UnityEngine;

public class HideZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.isHidden = true;
                Debug.Log("HideZone: Player entered, hiding enabled");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.isHidden = false;
                Debug.Log("HideZone: Player exited, hiding disabled");
            }
        }
    }
}