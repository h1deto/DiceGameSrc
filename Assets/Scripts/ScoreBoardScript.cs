using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardScript : MonoBehaviour
{
    private TärningarKvarScript tärningarKvarScript;
    private DiceRollScript diceRollScript;
    private StrykKnappScript strykKnappScript;

    private TextMeshProUGUI totalPoängText;
    private TextMeshProUGUI toppenPoäng;
    private TextMeshProUGUI toppenPoängMedBonus;

    [SerializeField] TextMeshProUGUI slutPoäng;

    [SerializeField] private GameObject bonusCheck;
    [SerializeField] private GameObject bonusNot;

    [SerializeField] private GameObject endgameScreen;

    AudioSource strykLjud;

    static int[] totalaPoäng = new int[13];
    static int[] multipliers = new int[] { 1, 2, 3, 4, 5, 6 };

    static bool fårBonus = false;

    public bool använd = false;
    public bool kanSkrivas = false;
    public bool kanStrykas = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject stryk = GameObject.Find("ScratchSound");
        strykLjud = stryk.GetComponent<AudioSource>();

        GameObject diceRollObject = GameObject.Find("RollButton");
        diceRollScript = diceRollObject.GetComponent<DiceRollScript>();

        GameObject strykKnapp = GameObject.Find("Stryk");
        strykKnappScript = strykKnapp.GetComponent<StrykKnappScript>();

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
        if (kanStrykas && !använd && !diceRollScript.harSkrivitDennaHanden)
        {

            diceRollScript.scoreBoardScriptKnappar[strykIndex].använd = true;


            GameObject strykKnapp = diceRollScript.scoreBoardScriptKnappar[strykIndex].gameObject;
            TextMeshProUGUI strykKnappText = strykKnapp.GetComponentInChildren<TextMeshProUGUI>();
            Button buttonComponent = strykKnapp.GetComponent<Button>();

            buttonComponent.interactable = false;

            totalaPoäng[strykIndex] = 0;

            strykKnappText.text = "X";
            strykKnappText.fontSize = 36;

            for (int i = 0; i < diceRollScript.scoreBoardScriptKnappar.Length; i++)
            {
                diceRollScript.scoreBoardScriptKnappar[i].kanStrykas = false;
            }


            RäknarTotalPoäng();

            diceRollScript.harSkrivitDennaHanden = true;
            diceRollScript.KollaTärningar();


            diceRollScript.antalRullningar = 0;
            tärningarKvarScript.TärningarKvar();

            strykKnappScript.omKnappenTryckt = false;

            ÅterställTärningar();
            strykLjud.Play(0);
        }
    }

    public void ScoreKnappGenerell(int index)
    {
        if (kanSkrivas && !använd && !kanStrykas && !diceRollScript.harSkrivitDennaHanden)
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


            
            RäknarTotalPoäng();


            // gör att man inte kan klicka på en annan knapp, och uppdaterar färgen efter man har klickat på en knapp
            diceRollScript.harSkrivitDennaHanden = true;

            diceRollScript.KollaTärningar();

            RäknaBonus();


            // återställer antal rolls
            diceRollScript.antalRullningar = 0;


            // kör denna metoden för att återställa texturerna för antal rolls
            tärningarKvarScript.TärningarKvar();

            ÅterställTärningar();
            strykLjud.Play(0);
        }
    }
    public void ScoreKnappUndreDelen(int index)
    {
        int tempIndex = index + 6;
        if (kanSkrivas && !använd && !kanStrykas && !diceRollScript.harSkrivitDennaHanden)
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

            ÅterställTärningar();
            strykLjud.Play(0);
        }
    }
    private void ÅterställTärningar()
    {
        for (int i = 0; i < diceRollScript.tärningarFinns.Length; i++)
        {
            if (diceRollScript.tärningarFinns[i] != null)
            {
                Destroy(diceRollScript.tärningarFinns[i]);
            }

            diceRollScript.tärningMarkerad[i] = false;
        }

        //diceRollScript.totalValue = 0;
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
        bool ärSpeletKlart = true;

        int total = 0;
        foreach (int tal in totalaPoäng)
        {
            total += tal;
        }
        if (fårBonus)
        {
            total += 35;
        }
        for (int i = 0; i < totalaPoäng.Length; i++)
        {
            if (diceRollScript.scoreBoardScriptKnappar[i].använd == false)
            {
                ärSpeletKlart = false;
            }
        }
        if (ärSpeletKlart)
        {
            Debug.Log("Spelet är klart, du fick: " + total + " poäng.");
            // ska lägga till en ruta som visar poäng, och sedan att den skriver poängen till en highscore fil

            endgameScreen.SetActive(true);

            if (total > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", total);
            }

            slutPoäng.text = $"Spelet är slut\n Du fick {total} poäng.";
        }

        totalPoängText.text = total.ToString();
    }
}
