using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Buttons for the game over menu
    public void RestartFromCheckpoint()
    {
        // Fade out to either a black or white screen depending on which it is
        // Re-load the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.GameOver(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.GameOver(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
