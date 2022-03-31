using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private string[] players = new string[8];
    private int[] playerNums = null;
    private static int currentPlayer = 1;
    private int numPlayers = 0;
    public static int staticNumPlayers;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("mast1").GetComponent<Button>().image.enabled = true;
        Screen.orientation = ScreenOrientation.Landscape;
        var F = new FileStream(Application.persistentDataPath + "/Players.txt", FileMode.Open, FileAccess.Read);
        StreamReader rdr = new StreamReader(F);

        for (int i = 0; rdr.Peek() > -1; i++)
        {
            players[i] = rdr.ReadLine().Trim();
            if (players[i].Equals("t"))
                numPlayers++;
        }
        staticNumPlayers = numPlayers;
        rdr.Close();
        F.Close();
        playerNums = new int[numPlayers];
        
        for(int i = 0, j = 0; i<players.Length; i++)
        {
            if (players[i].Equals("t"))
            {
                playerNums[j] = i + 1;
                j++;
            }
        }
        if (numPlayers > 0)
        {
            string coins = System.IO.File.ReadAllText(Application.persistentDataPath + "/Difficulty.txt");
            GameObject.Find("coins").GetComponent<Text>().text = coins;
            GameObject.Find("shipNum").GetComponent<Text>().text = playerNums[0].ToString();
            currentPlayer = 1;
        }
        else
        {
            GameObject.Find("Left").GetComponent<Button>().interactable = false;
            GameObject.Find("Right").GetComponent<Button>().interactable = false;
        }
    }
   
   
    public void right()
    {
        if (currentPlayer < numPlayers)
        {
            currentPlayer++;
            GameObject.Find("shipNum").GetComponent<Text>().text = playerNums[currentPlayer - 1].ToString();
            
        }
    }

    public void left()
    {
        if (currentPlayer > 1)
        {
            currentPlayer--;
            GameObject.Find("shipNum").GetComponent<Text>().text = playerNums[currentPlayer - 1].ToString();
            
        }
    }
}
