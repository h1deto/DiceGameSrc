using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardScript : MonoBehaviour
{
    private TärningarKvarScript tärningarKvarScript;
    private DiceRollScript diceRollScript;
    private TextMeshProUGUI totalPoängText;
    private TextMeshProUGUI toppenPoäng;
    private TextMeshProUGUI toppenPoängMedBonus;

    [SerializeField] private GameObject bonusCheck;
    [SerializeField] private GameObject bonusNot;

    static int[] totalaPoäng = new int[13];
    static int[] multipliers = new int[] { 1, 2, 3, 4, 5, 6 };

    static bool fårBonus = false;

    public bool använd = false;
    public bool kanSkrivas = false;
    public bool kanStrykas = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject diceRollObject = GameObject.Find("RollButton");
        diceRollScript = diceRollObject.GetComponent<DiceRollScript>();

        GameObject totalPoäng = GameObject.Find("TotalPoäng");
        totalPoängText = totalPoäng.GetComponent<TextMeshProUGUI>();

        GameObject topPoäng = GameObject.Find("TotalPoängTop");
        toppenPoäng = topPoäng.GetComponent<TextMeshProUGUI>();

        GameObject topPoängMBonus = GameObject.Find("TotalPoängTopMedBonus");
        toppenPoängMedBonus = topPoängMBonus.GetComponent<TextMeshProUGUI>();

        GameObject tärningarKvar = GameObject.Find("1Kvar");
        tärningarKvarScript = tärningarKvar.GetComponent<TärningarKvarScript>();

    }
    public void StrykKnapp(int strykIndex)
    {
        if (kanStrykas && !använd)
        {

            diceRollScript.scoreBoardScriptKnappar[strykIndex].använd = true;
            diceRollScript.scoreBoardScriptKnappar[strykIndex].kanStrykas = false;

            GameObject strykKnapp = diceRollScript.scoreBoardScriptKnappar[strykIndex].gameObject;
            TextMeshProUGUI strykKnappText = strykKnapp.GetComponentInChildren<TextMeshProUGUI>();
            Button buttonComponent = strykKnapp.GetComponent<Button>();

            buttonComponent.interactable = false;

            strykKnappText.text = "X";
            strykKnappText.fontSize = 36;


            diceRollScript.harSkrivitDennaHanden = true;


            diceRollScript.KollaTärningar();
            diceRollScript.antalRullningar = 0;
            tärningarKvarScript.TärningarKvar();
        }
    }

    public void ScoreKnappGenerell(int index)
    {
        if (kanSkrivas && !använd && !kanStrykas)
        {

            // hämtar texten i knappen
            GameObject ettTillSexKnapp = GameObject.FindGameObjectWithTag("ScoreButton" + (index + 1));
            TextMeshProUGUI ettTillSexKnappText = ettTillSexKnapp.GetComponentInChildren<TextMeshProUGUI>();


            // poängdelen
            totalaPoäng[index] = diceRollScript.antalAvVarjeTärning[index] * multipliers[index];
            ettTillSexKnappText.text = totalaPoäng[index].ToString();


            // gör att denna knappen inte börjar lysa igen
            använd = true;


            // gör att knappen inte går att klicka på igen
            Button buttonComponent = ettTillSexKnapp.GetComponent<Button>();
            buttonComponent.interactable = false;


            //
            RäknarTotalPoäng();


            // gör att man inte kan klicka på en annan knapp, och uppdaterar färgen efter man har klickat på en knapp
            diceRollScript.harSkrivitDennaHanden = true;
            diceRollScript.KollaTärningar();

            RäknaBonus();


            // återställer antal rolls
            diceRollScript.antalRullningar = 0;


            // kör denna metoden för att återställa texturerna för antal rolls
            tärningarKvarScript.TärningarKvar();
        }
    }
    public void ScoreKnappUndreDelen(int index)
    {
        int tempIndex = index + 6;
        if (kanSkrivas && !använd && !kanStrykas)
        {
            int[] tempVärden = new int[7];

            int indexTriss = Array.FindIndex(diceRollScript.antalAvVarjeTärning, value => value >= 3);
            if (indexTriss > -1)
            {
                tempVärden[0] = diceRollScript.antalAvVarjeTärning[indexTriss] * multipliers[indexTriss];
            }

            int indexFyrtal = Array.FindIndex(diceRollScript.antalAvVarjeTärning, value => value >= 4);
            if (indexFyrtal > -1)
            {
                tempVärden[1] = diceRollScript.antalAvVarjeTärning[indexFyrtal] * multipliers[indexFyrtal];
            }

            tempVärden[2] = 25;
            tempVärden[3] = 30;
            tempVärden[4] = 40;
            tempVärden[5] = 50;
            tempVärden[6] = diceRollScript.totalValue;


            GameObject underKnappar = GameObject.FindGameObjectWithTag("ScoreButton" + (tempIndex + 1));
            TextMeshProUGUI underKnapparText = underKnappar.GetComponentInChildren<TextMeshProUGUI>();

            underKnapparText.text = tempVärden[index].ToString();
            totalaPoäng[tempIndex] = tempVärden[index];


            använd = true;


            Button buttonComponent = underKnappar.GetComponent<Button>();
            buttonComponent.interactable = false;


            RäknarTotalPoäng();


            diceRollScript.harSkrivitDennaHanden = true;
            diceRollScript.KollaTärningar();


            diceRollScript.antalRullningar = 0;


            tärningarKvarScript.TärningarKvar();
        }
    }

    public void RäknaBonus()
    {
        int fårDuBonus = 0;
        int rK = 0;

        for (int i = 0; i < 6; i++)
        {
            fårDuBonus += totalaPoäng[i];

            if (diceRollScript.scoreBoardScriptKnappar[i].använd)
            {
                rK++;
            }
        }

        toppenPoäng.text = fårDuBonus.ToString();

        if (fårDuBonus >= 63)
        {
            bonusCheck.SetActive(true);
            fårBonus = true;
        }

        if (rK == 6)
        {
            if (fårBonus)
            {
                toppenPoängMedBonus.text = (fårDuBonus + 35).ToString();
            }
            else
            {
                toppenPoängMedBonus.text = fårDuBonus.ToString();

                bonusNot.SetActive(true);
            }
        }
    }

    public void RäknarTotalPoäng()
    {
        int total = 0;
        foreach (int tal in totalaPoäng)
        {
            total += tal;
        }
        if (fårBonus)
        {
            total += 35;
        }

        totalPoängText.text = total.ToString();
    }
}
