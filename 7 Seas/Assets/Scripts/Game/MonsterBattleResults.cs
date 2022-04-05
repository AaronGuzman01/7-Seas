using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBattleResults : MonoBehaviour
{
    public Text HealthLostText;
    public Text MonsterStatusText;
    public GameObject FX;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Enemy").Equals("Monster")) {

            if (PlayerPrefs.GetString("MonsterStatus") == "Dead")
            {
                MonsterStatusText.text = "THE MONSTER HAS BEEN SLAIN! YOUR TREASURE IS SAFE.";
            }
            else
            {
                MonsterStatusText.text = "THE MONSTER GOT AWAY! YOUR CREW LOSES HOPE AFTER SUCH A DEFEAT. \n GOLD LOST: " 
                    + ResultsManager.players[0].GetCurrentTreasure().ToString();

                ResultsManager.players[0].ClearTreasure();
            }

            FX.SetActive(false);
        }
    }
}
