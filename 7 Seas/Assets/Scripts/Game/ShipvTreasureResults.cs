using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipvTreasureResults : MonoBehaviour
{
    public Text shipStatus;
    public Text goldEarned;
    public Text reward;
    // Start is called before the first frame update
    void Start()
    {
        int gold = PlayerPrefs.GetInt("Gold");

        if (PlayerPrefs.GetString("Enemy").Equals("Treasure")) {

            if (PlayerPrefs.GetInt("ShipHits") == 0)
            {
                shipStatus.text = "YOU DESTROYED THE TREASURE SHIP!";
                shipStatus.color = Color.green;
                goldEarned.color = Color.yellow;

                if (PlayerPrefs.GetInt("Doubled") == 1)
                {
                    reward.gameObject.SetActive(true);
                    gold = gold * 2;
                }

                goldEarned.text = "GOLD EARNED: " + gold.ToString();
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
