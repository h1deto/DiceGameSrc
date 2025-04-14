using UnityEngine;
using UnityEngine.UI;

public class StrykKnappScript : MonoBehaviour
{
    public DiceRollScript diceRollScript;

    public bool omKnappenTryckt = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StrykKnappen()
    {
        if (true)
        {
            if (omKnappenTryckt == false)
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
                    omKnappenTryckt = true;
                }
            }
            else
            {
                ColorUtility.TryParseHtmlString("#2BFF0000", out Color ingenAlphaGrön);
                for (int i = 0; i < diceRollScript.scoreBoardScriptKnappar.Length; i++)
                {
                    if (diceRollScript.scoreBoardScriptKnappar[i].använd == false)
                    {
                        diceRollScript.scoreBoardScriptKnappar[i].kanStrykas = false;
                        GameObject strykKnapp = GameObject.FindGameObjectWithTag("ScoreButton" + (i + 1));
                        Image strykKnappImage = strykKnapp.GetComponent<Image>();

                        strykKnappImage.color = ingenAlphaGrön;
                    }
                    if (diceRollScript.antalRullningar > 0)
                    {
                        diceRollScript.KollaTärningar();
                    }
                    omKnappenTryckt = false;
                }
            }
        }
    }
}
