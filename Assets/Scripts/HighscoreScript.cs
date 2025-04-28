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
        highscoreText.text = "H�gsta uppn�dda po�ng �r:\n"+ PlayerPrefs.GetInt("HighScore",0);
    }
}
