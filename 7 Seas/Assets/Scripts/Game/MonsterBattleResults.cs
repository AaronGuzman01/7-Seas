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

            HealthLostText.text = HealthLostText.text.Replace("@", PlayerPrefs.GetInt("DamageDoneMonster").ToString());

            if (PlayerPrefs.GetString("MonsterStatus") == "Dead")
            {
                int GoldEarned = Random.Range(700, 1400);
                MonsterStatusText.text = "THE MONSTER HAS BEEN SLAIN!";
            }
            else
            {
                MonsterStatusText.text = "THE MONSTER GOT AWAY! YOUR CREW LOSES HOPE AFTER SUCH A DEFEAT.";
            }

            HealthLostText.text.Replace("@", PlayerPrefs.GetInt("DamageDoneMonster").ToString());

            FX.SetActive(false);
        }
    }
}
