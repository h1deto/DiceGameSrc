using TMPro;
using UnityEngine;

public class HighscoreScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;
    void Start()
    {
        Highscore();
    }

    void Highscore()
    {
        highscoreText.text = "Högsta uppnådda poäng är:\n"+ PlayerPrefs.GetInt("HighScore",0);
    }
}
