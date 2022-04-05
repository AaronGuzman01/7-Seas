using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    public Text scoreText;
    public Text goldText;
    int highestScore;
    private static int Points;

    public void Start()
    {
        int highestScore = PlayerPrefs.GetInt("highScore", 0);
        Points = 0;
        
        GameObject tempScore = GameObject.Find("Canvas");
        scoreText = GameObject.Find("Score").GetComponent<Text>();
    }
    void Update()
    {
        if (PlayerPrefs.GetString("Enemy").Equals("Player"))
        {
            scoreText.text = "DAMAGE DEALT: " + Points;
        }
        else if(PlayerPrefs.GetString("Enemy").Equals("Treasure"))
        {
            scoreText.text = "Hits Required: " + PlayerPrefs.GetInt("ShipHits").ToString();

            goldText.gameObject.SetActive(true);
            goldText.text = "Gold: " + PlayerPrefs.GetInt("Gold").ToString();
        }
        else
        {
            scoreText.text = ": " + Points;
        }
        highestScore = PlayerPrefs.GetInt("highScore");

        if (highestScore < Points)
        {
            PlayerPrefs.SetInt("highScore", Points);
            highestScore = PlayerPrefs.GetInt("highScore");
        }
        if (!(Points == 0))
        {
            PlayerPrefs.SetInt("score", Points);
        }
    }
    public void AddPoints(int amount)
    {
        string difficulty = PlayerPrefs.GetString("Difficulty");
        int multiplier = 1;
        
        /*
        switch (difficulty)
        {
            case "POWDER MONKEY":
                //no change in multiplier
                break;
            case "BOATSWAIN":
                multiplier = 2;
                break;
            case "QUARTERMASTER":
                multiplier = 3;
                break;
            case "CAPTAIN":
                multiplier = 5;
                break;
        }*/

        amount = amount * multiplier;

        if (PlayerPrefs.GetString("Enemy").Equals("Treasure"))
        {
            Points += (amount * 100);
            scoreText.text = "Gold Earned: " + Points;
            PlayerPrefs.SetInt("Treasure Score", Points);

            Debug.Log(PlayerPrefs.GetInt("Treasure Score"));
        }
        else
        {
            Points = Points + amount;
            scoreText.text = ": " + Points;
            PlayerPrefs.SetInt("score", Points);
        }
        PlayerPrefs.Save();
    }

    public static void ResetScore()
    {
        Points = 0;
    }
}
