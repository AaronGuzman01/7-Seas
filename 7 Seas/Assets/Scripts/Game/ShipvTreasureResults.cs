using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipvTreasureResults : MonoBehaviour
{
    public Text GoldEarned;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Enemy").Equals("Treasure")) {

            GoldEarned.text = "GOLD EARNED: " + PlayerPrefs.GetInt("Treasure Score");

            ResultsManager.players[0].AddTreasure(PlayerPrefs.GetInt("Treasure Score"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
