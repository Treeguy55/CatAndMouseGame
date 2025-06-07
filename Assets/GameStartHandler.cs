using UnityEngine;

public class GameStartHandler : MonoBehaviour
{
    private SimpleTimer timer;

    void Start()
    {
        timer = FindObjectOfType<SimpleTimer>();
        if (timer != null)
        {
            timer.StartTimer();
        }
    }
}
