using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc : MonoBehaviour
{
    void Start()
    {
        // Call ShowMessage from StartMessageController when gameplay starts
        StartMessageController messageController = FindObjectOfType<StartMessageController>();
        if (messageController != null)
        {
            messageController.ShowMessage();
        }
        else
        {
            Debug.LogWarning("StartMessageController not found in the scene!");
        }
    }
}
