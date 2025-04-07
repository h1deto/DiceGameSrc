using UnityEngine;

public class TärningarKvarScript : MonoBehaviour
{
    private DiceRollScript diceRollScript;

    private GameObject[] tärningKvar = new GameObject[3];


    private void Start()
    {
        GameObject diceRoll = GameObject.Find("RollButton");
        diceRollScript = diceRoll.GetComponent<DiceRollScript>();

        tärningKvar[0] = GameObject.Find("1Kvar");
        tärningKvar[1] = GameObject.Find("2Kvar");
        tärningKvar[2] = GameObject.Find("3Kvar");

    }
    public void TärningarKvar()
    {
        if (diceRollScript.antalRullningar == 3)
        {
            for (int rull = 0; rull < tärningKvar.Length; rull++)
            {
                tärningKvar[rull].SetActive(false);
            }
        }
        else if (diceRollScript.antalRullningar == 2)
        {
            tärningKvar[1].SetActive(false);
            tärningKvar[2].SetActive(false);
        }
        else if (diceRollScript.antalRullningar == 1)
        {
            tärningKvar[2].SetActive(false);
        }
        else if (diceRollScript.antalRullningar == 0)
        {
            for (int rull = 0; rull < tärningKvar.Length; rull++)
            {
                tärningKvar[rull].SetActive(true);
            }
        }
    }
}
