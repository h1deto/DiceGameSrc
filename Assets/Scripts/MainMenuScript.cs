using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HighScore()
    {
        SceneManager.LoadScene("Highscore");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("stänger av spelet");
    }
}
