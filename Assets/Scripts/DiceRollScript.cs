using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollScript : MonoBehaviour
{
    [SerializeField] private T�rningarKvarScript t�rningarKvarScript;
    [SerializeField] public TextMeshProUGUI totalValueFromDiceRolls;
    public StrykKnappScript strykKnappScript;


    public GameObject[] prefabs;
    private GameObject[] t�rningarFinns = new GameObject[5];


    GameObject[] ettTillSexKnapp = new GameObject[6];
    GameObject[] undreKnappar = new GameObject[7];


    public ScoreBoardScript[] scoreBoardScriptKnappar;


    private bool[] t�rningMarkerad = new bool[5];
    public bool harSkrivitDennaHanden = true;


    public int[] t�rningV�rde = new int[] {-1,-2,-3,-4,-5,-6};
    public int[] antalAvVarjeT�rning = new int[6];


    // public bool[] kanSkrivas = new bool[13];
    // public bool[] po�ngAnv�nd = new bool[13];
    // dessa arrayer var f�r knapparna, men det �r omskrivet till en array med objects


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

    public void T�rningsRullande()
    {
        if (antalRullningar < maxRullningar)
        {
            totalValue = 0;
            for (int i = 0; i < t�rningMarkerad.Length; i++)
            {
                if (t�rningMarkerad[i] == false)
                {
                    t�rningV�rde[i] = D6Roll() - 1;

                    if (t�rningarFinns[i] != null)
                    {
                        Destroy(t�rningarFinns[i]);
                    }

                    GameObject t�rningSpawn = GameObject.FindGameObjectWithTag($"Spawn{i + 1}");
                    Vector2 t�rningPosition = t�rningSpawn.transform.position;
                    t�rningarFinns[i] = Instantiate(prefabs[t�rningV�rde[i]], t�rningPosition, t�rningSpawn.transform.rotation);

                    totalValue += t�rningV�rde[i] + 1;
                }
            }

            strykKnappScript.omKnappenTryckt = false;

            antalRullningar++;
            KollaT�rningar();
            t�rningarKvarScript.T�rningarKvar();

        }
    }

    public int D6Roll()
    {
        // test f�r att g�ra en egen rng algo
        //
        System.Random seed = new System.Random();
        int seedV�rde = seed.Next(int.MinValue, int.MaxValue);

        System.Random rng = new System.Random(seedV�rde);
        int d6Roll = rng.Next(1, 7);
        Debug.Log("DEBUG: " + d6Roll);
        return d6Roll;

    }

    public void KollaT�rningar()
    {
        for (int i = 0; i < antalAvVarjeT�rning.Length; i++)
        {
            antalAvVarjeT�rning[i] = 0;
        }

        foreach (int value in t�rningV�rde)
        {
            if (value >= 0)
            {
                antalAvVarjeT�rning[value]++;
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

            if (antalAvVarjeT�rning[i] > 0 && !scoreBoardScriptKnappar[i].anv�nd && !harSkrivitDennaHanden)
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


        bool[] underV�rdenBool = new bool[]
        {
            antalAvVarjeT�rning.Any(count => count >= 3), // triss & �ver
            antalAvVarjeT�rning.Any(count => count >= 4), // fyrtal & �ver
            antalAvVarjeT�rning.Contains(3) && antalAvVarjeT�rning.Contains(2), // k�k
            antalAvVarjeT�rning[0] == 1 && antalAvVarjeT�rning[1] == 1 && antalAvVarjeT�rning[2] == 1 && antalAvVarjeT�rning[3] == 1 && antalAvVarjeT�rning[4] == 1, // litenStege
            antalAvVarjeT�rning[1] == 1 && antalAvVarjeT�rning[2] == 1 && antalAvVarjeT�rning[3] == 1 && antalAvVarjeT�rning[4] == 1 && antalAvVarjeT�rning[5] == 1, // storStege
            antalAvVarjeT�rning.Any(count => count == 5), // yatzy
            totalValue > 0 // chans
        };

        for (int i = 0; i < underV�rdenBool.Length; i++)
        {
            undreKnappar[i] = GameObject.FindGameObjectWithTag("ScoreButton" + (i + 7));
            Image undreKnapparImage = undreKnappar[i].GetComponent<Image>();

            if (underV�rdenBool[i] && !scoreBoardScriptKnappar[i + 6].anv�nd && !harSkrivitDennaHanden)
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
