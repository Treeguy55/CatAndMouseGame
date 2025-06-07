using UnityEngine;
using System.Collections;
using TMPro;

public class StartMessageController : MonoBehaviour
{
    public GameObject messageUI;
    public float displayTime = 5f;

    void Start()
    {
        messageUI.SetActive(false);
    }

    public void ShowMessage()
    {
        StartCoroutine(ShowAndHide());
    }

    private IEnumerator ShowAndHide()
    {
        messageUI.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        messageUI.SetActive(false);
    }
}
