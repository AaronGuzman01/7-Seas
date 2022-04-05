using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipvTreasureResults : MonoBehaviour
{
    public Text shipStatus;
    public Text goldEarned;
    // Start is called before the first frame update
    void Start()
    {
        int gold = PlayerPrefs.GetInt("Gold");

        if (PlayerPrefs.GetString("Enemy").Equals("Treasure")) {

            if (PlayerPrefs.GetInt("ShipHits") == 0)
            {
                shipStatus.text = "YOU DESTROYED THE TREASURE SHIP!";
                shipStatus.color = Color.green;
                goldEarned.text = "GOLD EARNED: " + gold.ToString();
                goldEarned.color = Color.yellow;

                ResultsManager.players[0].AddToTreasure(gold);
            }
            else
            {
                shipStatus.text = "YOU FAILED TO DESTROY THE TREASURE SHIP";
                shipStatus.color = Color.red;
                goldEarned.text = "NO GOLD EARNED";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
