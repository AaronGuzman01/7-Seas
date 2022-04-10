using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCollision : MonoBehaviour
{
    GameObject dice;
    HashSet<string> diceHit = new HashSet<string>();

    private void Update()
    {
        if (diceHit.Count >= 5)
        {
            diceHit.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        dice = other.gameObject.transform.parent.gameObject;

        if (dice.name.Contains("Dice") && !diceHit.Contains(dice.name)) {
            DiceManager.diceStart = true;

            diceHit.Add(dice.name);

            Destroy(other.gameObject);
        }
    }
}
