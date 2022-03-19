﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtons : MonoBehaviour {
    public Sprite swabieOnSprite;
    public Sprite swabieOffSprite;
    public Sprite seamanOnSprite;
    public Sprite seamanOffSprite;
    public Sprite captainOnSprite;
    public Sprite captainOffSprite;

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetString("Cup") == "Seaman" && !SetupMenu.resetSetup)
        {
            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOnSprite;
            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOffSprite;
            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOffSprite;

            DiceCupMain.swabie = false;
            DiceCupMain.seaman = true;
            DiceCupMain.captain = false;
        }
        else if (PlayerPrefs.GetString("Cup") == "Captain" && !SetupMenu.resetSetup)
        {
            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOnSprite;
            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOffSprite;
            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOffSprite;

            DiceCupMain.swabie = false;
            DiceCupMain.seaman = false;
            DiceCupMain.captain = true;
        }
        else
        {
            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOnSprite;
            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOffSprite;
            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOffSprite;

            PlayerPrefs.SetString("Cup", "Swabie");

            DiceCupMain.swabie = true;
            DiceCupMain.seaman = false;
            DiceCupMain.captain = false;
        }

        /*
        //load seaman by default
        PlayerPrefs.SetString("Difficulty", "Seaman");
        DiceCupMain.swabie = false;
        DiceCupMain.seaman = true;
        DiceCupMain.captain = false;
        GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOffSprite;
        GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOnSprite;
        GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOffSprite;
        */


        //LoadDiceLevel();

    }

    //set difficulty levels
    public void LoadDiceLevel()
    {
        if (this.name == "Swabie")
        {
            PlayerPrefs.SetString("Cup", "Swabie");
            DiceCupMain.swabie = true;
            DiceCupMain.seaman = false;
            DiceCupMain.captain = false;
            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOnSprite;
            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOffSprite;
            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOffSprite;
        }
        else if (this.name == "Seaman")
        {
            PlayerPrefs.SetString("Cup", "Seaman");
            DiceCupMain.swabie = false;
            DiceCupMain.seaman = true;
            DiceCupMain.captain = false;
            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOffSprite;
            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOnSprite;
            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOffSprite;
        }
        else if (this.name == "Captain")
        {
            PlayerPrefs.SetString("Cup", "Captain");
            DiceCupMain.swabie = false;
            DiceCupMain.seaman = false;
            DiceCupMain.captain = true;
            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOffSprite;
            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOffSprite;
            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOnSprite;
        }
    }
}
