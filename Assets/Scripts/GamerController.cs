using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    [Range(0.1f, 5.0f)]
    public float timeScale = 5.0f;

    void Update()
    {
        // Adjusts the physics and animation playback speed
        Time.timeScale = timeScale;
    }
}
