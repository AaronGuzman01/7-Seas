using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipVShipResults : MonoBehaviour
{
    public Text WinnerText;
    public Text Player1ScoreText;
    public Text Player2ScoreText;
    public Text WinnerResults;
    private int P1Score;
    private int P2Score;
    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.GetString("Enemy").Equals("Player"))
        {

            P1Score = PlayerPrefs.GetInt("Player1Score");
            P2Score = PlayerPrefs.GetInt("Player2Score");
            int results = CalculateWinner() * 100;
            if (P1Score > P2Score)
            {
                WinnerText.text = "Player " + ResultsManager.players[0].GetPlayerNum().ToString() + " Wins!".ToUpper();
                WinnerResults.text = "Player " + ResultsManager.players[0].GetPlayerNum().ToString() + " gets: ".ToUpper() + results + " gold!".ToUpper();

                ResultsManager.players[0].AddTreasure(results);
                ResultsManager.ships[0].SetActive(true);
            }
            else if (P1Score < P2Score)
            {
                WinnerText.text = "Player " + ResultsManager.players[1].GetPlayerNum().ToString() + " Wins!".ToUpper();
                WinnerResults.text = "Player " + ResultsManager.players[1].GetPlayerNum().ToString() + " gets: ".ToUpper() + results + " gold!".ToUpper();

                ResultsManager.players[1].AddTreasure(results);
                ResultsManager.ships[1].SetActive(true);
            }
            else
            {
                WinnerText.text = "Yar...tis a draw!".ToUpper();
                WinnerResults.text = "No booty for either of you powder monkeys!".ToUpper();
            }

            Player1ScoreText.text = "Player " + ResultsManager.players[0].GetPlayerNum().ToString() + " Score: " + P1Score;
            Player2ScoreText.text = "Player " + ResultsManager.players[1].GetPlayerNum().ToString() + " Score: " + P2Score;
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int CalculateWinner()
    {
        return (Math.Abs(P1Score - P2Score));
    }
}