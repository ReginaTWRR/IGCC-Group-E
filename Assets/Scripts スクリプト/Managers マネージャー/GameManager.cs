using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    bool isGamePaused = false;
    public bool IsGamePaused => isGamePaused;

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }
}
