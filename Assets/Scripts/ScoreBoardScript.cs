using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardScript : MonoBehaviour
{
    private T�rningarKvarScript t�rningarKvarScript;
    private DiceRollScript diceRollScript;
    private TextMeshProUGUI totalPo�ngText;
    private TextMeshProUGUI toppenPo�ng;
    private TextMeshProUGUI toppenPo�ngMedBonus;

    [SerializeField] private GameObject bonusCheck;
    [SerializeField] private GameObject bonusNot;

    static int[] totalaPo�ng = new int[13];
    static int[] multipliers = new int[] { 1, 2, 3, 4, 5, 6 };

    static bool f�rBonus = false;

    public bool anv�nd = false;
    public bool kanSkrivas = false;
    public bool kanStrykas = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject diceRollObject = GameObject.Find("RollButton");
        diceRollScript = diceRollObject.GetComponent<DiceRollScript>();

        GameObject totalPo�ng = GameObject.Find("TotalPo�ng");
        totalPo�ngText = totalPo�ng.GetComponent<TextMeshProUGUI>();

        GameObject topPo�ng = GameObject.Find("TotalPo�ngTop");
        toppenPo�ng = topPo�ng.GetComponent<TextMeshProUGUI>();

        GameObject topPo�ngMBonus = GameObject.Find("TotalPo�ngTopMedBonus");
        toppenPo�ngMedBonus = topPo�ngMBonus.GetComponent<TextMeshProUGUI>();

        GameObject t�rningarKvar = GameObject.Find("1Kvar");
        t�rningarKvarScript = t�rningarKvar.GetComponent<T�rningarKvarScript>();

    }
    public void StrykKnapp(int strykIndex)
    {
        if (kanStrykas && !anv�nd)
        {

            diceRollScript.scoreBoardScriptKnappar[strykIndex].anv�nd = true;
            diceRollScript.scoreBoardScriptKnappar[strykIndex].kanStrykas = false;

            GameObject strykKnapp = diceRollScript.scoreBoardScriptKnappar[strykIndex].gameObject;
            TextMeshProUGUI strykKnappText = strykKnapp.GetComponentInChildren<TextMeshProUGUI>();
            Button buttonComponent = strykKnapp.GetComponent<Button>();

            buttonComponent.interactable = false;

            strykKnappText.text = "X";
            strykKnappText.fontSize = 36;


            diceRollScript.harSkrivitDennaHanden = true;


            diceRollScript.KollaT�rningar();
            diceRollScript.antalRullningar = 0;
            t�rningarKvarScript.T�rningarKvar();
        }
    }

    public void ScoreKnappGenerell(int index)
    {
        if (kanSkrivas && !anv�nd && !kanStrykas)
        {

            // h�mtar texten i knappen
            GameObject ettTillSexKnapp = GameObject.FindGameObjectWithTag("ScoreButton" + (index + 1));
            TextMeshProUGUI ettTillSexKnappText = ettTillSexKnapp.GetComponentInChildren<TextMeshProUGUI>();


            // po�ngdelen
            totalaPo�ng[index] = diceRollScript.antalAvVarjeT�rning[index] * multipliers[index];
            ettTillSexKnappText.text = totalaPo�ng[index].ToString();


            // g�r att denna knappen inte b�rjar lysa igen
            anv�nd = true;


            // g�r att knappen inte g�r att klicka p� igen
            Button buttonComponent = ettTillSexKnapp.GetComponent<Button>();
            buttonComponent.interactable = false;


            //
            R�knarTotalPo�ng();


            // g�r att man inte kan klicka p� en annan knapp, och uppdaterar f�rgen efter man har klickat p� en knapp
            diceRollScript.harSkrivitDennaHanden = true;
            diceRollScript.KollaT�rningar();

            R�knaBonus();


            // �terst�ller antal rolls
            diceRollScript.antalRullningar = 0;


            // k�r denna metoden f�r att �terst�lla texturerna f�r antal rolls
            t�rningarKvarScript.T�rningarKvar();
        }
    }
    public void ScoreKnappUndreDelen(int index)
    {
        int tempIndex = index + 6;
        if (kanSkrivas && !anv�nd && !kanStrykas)
        {
            int[] tempV�rden = new int[7];

            int indexTriss = Array.FindIndex(diceRollScript.antalAvVarjeT�rning, value => value >= 3);
            if (indexTriss > -1)
            {
                tempV�rden[0] = diceRollScript.antalAvVarjeT�rning[indexTriss] * multipliers[indexTriss];
            }

            int indexFyrtal = Array.FindIndex(diceRollScript.antalAvVarjeT�rning, value => value >= 4);
            if (indexFyrtal > -1)
            {
                tempV�rden[1] = diceRollScript.antalAvVarjeT�rning[indexFyrtal] * multipliers[indexFyrtal];
            }

            tempV�rden[2] = 25;
            tempV�rden[3] = 30;
            tempV�rden[4] = 40;
            tempV�rden[5] = 50;
            tempV�rden[6] = diceRollScript.totalValue;


            GameObject underKnappar = GameObject.FindGameObjectWithTag("ScoreButton" + (tempIndex + 1));
            TextMeshProUGUI underKnapparText = underKnappar.GetComponentInChildren<TextMeshProUGUI>();

            underKnapparText.text = tempV�rden[index].ToString();
            totalaPo�ng[tempIndex] = tempV�rden[index];


            anv�nd = true;


            Button buttonComponent = underKnappar.GetComponent<Button>();
            buttonComponent.interactable = false;


            R�knarTotalPo�ng();


            diceRollScript.harSkrivitDennaHanden = true;
            diceRollScript.KollaT�rningar();


            diceRollScript.antalRullningar = 0;


            t�rningarKvarScript.T�rningarKvar();
        }
    }

    public void R�knaBonus()
    {
        int f�rDuBonus = 0;
        int rK = 0;

        for (int i = 0; i < 6; i++)
        {
            f�rDuBonus += totalaPo�ng[i];

            if (diceRollScript.scoreBoardScriptKnappar[i].anv�nd)
            {
                rK++;
            }
        }

        toppenPo�ng.text = f�rDuBonus.ToString();

        if (f�rDuBonus >= 63)
        {
            bonusCheck.SetActive(true);
            f�rBonus = true;
        }

        if (rK == 6)
        {
            if (f�rBonus)
            {
                toppenPo�ngMedBonus.text = (f�rDuBonus + 35).ToString();
            }
            else
            {
                toppenPo�ngMedBonus.text = f�rDuBonus.ToString();

                bonusNot.SetActive(true);
            }
        }
    }

    public void R�knarTotalPo�ng()
    {
        int total = 0;
        foreach (int tal in totalaPo�ng)
        {
            total += tal;
        }
        if (f�rBonus)
        {
            total += 35;
        }

        totalPo�ngText.text = total.ToString();
    }
}
