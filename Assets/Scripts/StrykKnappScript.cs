using UnityEngine;
using UnityEngine.UI;

public class StrykKnappScript : MonoBehaviour
{
    public DiceRollScript diceRollScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StrykKnapp()
    {
        ColorUtility.TryParseHtmlString("#FF1E00FF", out Color fullAlphaRöd);

        for (int i = 0; i < diceRollScript.scoreBoardScriptKnappar.Length; i++)
        {
            if (diceRollScript.scoreBoardScriptKnappar[i].använd == false)
            {
                diceRollScript.scoreBoardScriptKnappar[i].kanStrykas = true;
                GameObject strykKnapp = GameObject.FindGameObjectWithTag("ScoreButton" + (i + 1));
                Image strykKnappImage = strykKnapp.GetComponent<Image>();

                strykKnappImage.color = fullAlphaRöd;

            }
        }
    }
}
