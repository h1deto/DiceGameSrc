using UnityEngine;

public class KlickaPåTärningarScript : MonoBehaviour
{
    private int diceIndex;
    private DiceRollScript diceRollScript;

    public void Initialize(int index, DiceRollScript script)
    {
        diceIndex = index;
        diceRollScript = script;
    }

    private void OnMouseDown()
    {
        if (diceRollScript != null)
        {
            if (diceRollScript.ÄrTärningMarkerad(diceIndex))
            {
                // Reverse the changes: unmark the dice and move it back down
                diceRollScript.KlickadTärning(diceIndex); // Toggle the marker back to false
                transform.position -= new Vector3(0, 0.5f, 0); // Move the dice back down
            }
            else
            {
                // Apply the changes: mark the dice and move it up
                diceRollScript.KlickadTärning(diceIndex); // Toggle the marker to true
                transform.position += new Vector3(0, 0.5f, 0); // Move the dice up
            }
        }
    }
}
