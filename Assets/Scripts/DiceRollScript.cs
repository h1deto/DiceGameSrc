using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollScript : MonoBehaviour
{
    [SerializeField] private TärningarKvarScript tärningarKvarScript;
    [SerializeField] public TextMeshProUGUI totalValueFromDiceRolls;
    public StrykKnappScript strykKnappScript;


    public GameObject[] prefabs;
    private GameObject[] tärningarFinns = new GameObject[5];


    GameObject[] ettTillSexKnapp = new GameObject[6];
    GameObject[] undreKnappar = new GameObject[7];


    public ScoreBoardScript[] scoreBoardScriptKnappar;


    private bool[] tärningMarkerad = new bool[5];
    public bool harSkrivitDennaHanden = true;


    public int[] tärningVärde = new int[] {-1,-2,-3,-4,-5,-6};
    public int[] antalAvVarjeTärning = new int[6];


    // public bool[] kanSkrivas = new bool[13];
    // public bool[] poängAnvänd = new bool[13];
    // dessa arrayer var för knapparna, men det är omskrivet till en array med objects


    public int antalRullningar = 0;
    public int maxRullningar = 3;
    public int totalValue = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        totalValueFromDiceRolls.text = totalValue.ToString();
    }

    public void TärningsRullande()
    {
        if (antalRullningar < maxRullningar)
        {
            totalValue = 0;
            for (int i = 0; i < tärningMarkerad.Length; i++)
            {
                if (tärningMarkerad[i] == false)
                {
                    tärningVärde[i] = D6Roll() - 1;

                    if (tärningarFinns[i] != null)
                    {
                        Destroy(tärningarFinns[i]);
                    }

                    GameObject tärningSpawn = GameObject.FindGameObjectWithTag($"Spawn{i + 1}");
                    Vector2 tärningPosition = tärningSpawn.transform.position;
                    tärningarFinns[i] = Instantiate(prefabs[tärningVärde[i]], tärningPosition, tärningSpawn.transform.rotation);

                    totalValue += tärningVärde[i] + 1;
                }
            }

            strykKnappScript.omKnappenTryckt = false;

            antalRullningar++;
            KollaTärningar();
            tärningarKvarScript.TärningarKvar();

        }
    }

    public int D6Roll()
    {
        // test för att göra en egen rng algo
        //
        System.Random seed = new System.Random();
        int seedVärde = seed.Next(int.MinValue, int.MaxValue);

        System.Random rng = new System.Random(seedVärde);
        int d6Roll = rng.Next(1, 7);
        Debug.Log("DEBUG: " + d6Roll);
        return d6Roll;

    }

    public void KollaTärningar()
    {
        for (int i = 0; i < antalAvVarjeTärning.Length; i++)
        {
            antalAvVarjeTärning[i] = 0;
        }

        foreach (int value in tärningVärde)
        {
            if (value >= 0)
            {
                antalAvVarjeTärning[value]++;
            }
        }

        Color fullAlpha;
        ColorUtility.TryParseHtmlString("#2BFF00FF", out fullAlpha);

        Color ingenAlpha;
        ColorUtility.TryParseHtmlString("#2BFF0000", out ingenAlpha);


        for (int i = 0; i < 6; i++)
        {
            ettTillSexKnapp[i] = GameObject.FindGameObjectWithTag("ScoreButton" + (i + 1));
            Image ettTillSexKnappImage = ettTillSexKnapp[i].GetComponent<Image>();

            if (antalAvVarjeTärning[i] > 0 && !scoreBoardScriptKnappar[i].använd && !harSkrivitDennaHanden)
            {
                scoreBoardScriptKnappar[i].kanSkrivas = true;

                ettTillSexKnappImage.color = fullAlpha;

            }
            else
            {
                scoreBoardScriptKnappar[i].kanSkrivas = false;

                ettTillSexKnappImage.color = ingenAlpha;
            }
        }


        bool[] underVärdenBool = new bool[]
        {
            antalAvVarjeTärning.Any(count => count >= 3), // triss & över
            antalAvVarjeTärning.Any(count => count >= 4), // fyrtal & över
            antalAvVarjeTärning.Contains(3) && antalAvVarjeTärning.Contains(2), // kåk
            antalAvVarjeTärning[0] == 1 && antalAvVarjeTärning[1] == 1 && antalAvVarjeTärning[2] == 1 && antalAvVarjeTärning[3] == 1 && antalAvVarjeTärning[4] == 1, // litenStege
            antalAvVarjeTärning[1] == 1 && antalAvVarjeTärning[2] == 1 && antalAvVarjeTärning[3] == 1 && antalAvVarjeTärning[4] == 1 && antalAvVarjeTärning[5] == 1, // storStege
            antalAvVarjeTärning.Any(count => count == 5), // yatzy
            totalValue > 0 // chans
        };

        for (int i = 0; i < underVärdenBool.Length; i++)
        {
            undreKnappar[i] = GameObject.FindGameObjectWithTag("ScoreButton" + (i + 7));
            Image undreKnapparImage = undreKnappar[i].GetComponent<Image>();

            if (underVärdenBool[i] && !scoreBoardScriptKnappar[i + 6].använd && !harSkrivitDennaHanden)
            {
                scoreBoardScriptKnappar[i + 6].kanSkrivas = true;

                undreKnapparImage.color = fullAlpha;
            }
            else
            {
                scoreBoardScriptKnappar[i + 6].kanSkrivas = false;

                undreKnapparImage.color = ingenAlpha;
            }
        }

        harSkrivitDennaHanden = false;
    }
}
