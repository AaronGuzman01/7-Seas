﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DiceManager : MonoBehaviour {
    public int num = 0;
    public int ghostDiceTotal = 0;
    public GameObject shipDiceSprite;
    public GameObject resourceDiceSprite;
    public GameObject basicDiceSprite;
    public GameObject movementDiceSprite;
    public GameObject windDiceSprite;
    public GameObject rewardDiceSprite;
    public GameObject hazardDiceSprite;
    public GameObject colorDiceSprite;

    public InventorySystem inventorySystem;
    public GameLoop gameLoop;

    public List<Sprite> shipDiceArray;
    public List<Sprite> resourceDiceArray;
    public List<Sprite> basicDiceArray;
    public List<Sprite> movementDiceArray;
    public List<Sprite> windDiceArray;
    public List<Sprite> rewardDiceArray;
    public List<Sprite> hazardDiceArray;
    public List<Sprite> colorDiceArray;
    public List<Sprite> parrotTileArray;

    public GameObject diceImage;
    public Texture diceDefaultTexture;
    public ParticleSystem explosion;
    public Canvas gameGUI;

    public GameObject diceList;
    public TimeOfDay timeOfDay;
    public GameObject fog;
    public bool fogDice = false;

    public LoseTurn loseTurn;

    public static bool diceStart = false;

    bool firstRound = true;


    // Use this for initialization
    void Awake()
    {
        diceList = GameObject.Find("DiceList");

        diceDefaultTexture = diceImage.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture;

        ResetDice();

        /*
        //if the dice cup menu setting files exist, then load them into the gameboard arrays
        if (ES2.Exists(PlayerPrefs.GetString("Difficulty") + "targetShipDice"))
        {
            //load all the arrays from the dice cup saves
            shipDiceArray = new List<Sprite>();
            shipDiceArray = ES2.LoadList<Sprite>(PlayerPrefs.GetString("Difficulty") + "targetShipDice");
            shipDiceArray.Reverse();


            resourceDiceArray = new List<Sprite>();
            resourceDiceArray = ES2.LoadList<Sprite>(PlayerPrefs.GetString("Difficulty") + "resourceDice");
            resourceDiceArray.Reverse();

            movementDiceArray = new List<Sprite>();
            movementDiceArray = ES2.LoadList<Sprite>(PlayerPrefs.GetString("Difficulty") + "movementNumDice");
            movementDiceArray.Reverse();

            windDiceArray = new List<Sprite>();
            windDiceArray = ES2.LoadList<Sprite>(PlayerPrefs.GetString("Difficulty") + "windMovementDice");
            windDiceArray.Reverse();

            colorDiceArray = new List<Sprite>();
            colorDiceArray = ES2.LoadList<Sprite>(PlayerPrefs.GetString("Difficulty") + "colorDice");
            colorDiceArray.Reverse();
        }

        //go through the parrot dice and the dice list and add the matching parrot tiles
        foreach (var item in diceList.GetComponent<MasterDiceList>().diceClassList)
        {
            foreach(var item2 in colorDiceArray)
            {
                if(item2.name == item.DiceImage.name)
                {
                    parrotTileArray.Add(item.ChildDiceImage);
                }
            }
        }
        */
    }

    void ResetDice()
    {
        movementDiceSprite.SetActive(false);
        basicDiceSprite.SetActive(false);
        windDiceSprite.SetActive(false);
        rewardDiceSprite.SetActive(false);
        hazardDiceSprite.SetActive(false);


    }

    //roll the dice
    public void RollDice()
    {
        ResetDice();

        ghostDiceTotal = 0;

        diceImage.SetActive(true);

        diceImage.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = diceDefaultTexture;

        /*
        //pick a random die from each dice array
        num = Random.Range(0, shipDiceArray.Count);
        shipDiceSprite.GetComponent<Image>().sprite = shipDiceArray[num];

        num = Random.Range(0, resourceDiceArray.Count);
        resourceDiceSprite.GetComponent<Image>().sprite = resourceDiceArray[num];

        num = Random.Range(0, movementDiceArray.Count);
        movementDiceSprite.GetComponent<Image>().sprite = movementDiceArray[num];
        */

        /*
        do
        {
            num = Random.Range(0, windDiceArray.Count);

            foreach (var item in inventorySystem.WindDice)
            {
                if (item.Image.name == windDiceArray[num].name)
                {
                    if (item.Name == "Fog")
                    {
                        break;
                    }
                    else
                    {
                        firstRound = false;
                    }
                }
            }

        } while (firstRound);
        */

        StartCoroutine(GetDiceValue(basicDiceArray, basicDiceSprite, 0));

        StartCoroutine(GetDiceValue(movementDiceArray, movementDiceSprite, 1));

        StartCoroutine(GetDiceValue(windDiceArray, windDiceSprite, 2));

        StartCoroutine(GetDiceValue(rewardDiceArray, rewardDiceSprite, 3));

        StartCoroutine(GetDiceValue(hazardDiceArray, hazardDiceSprite, 4));
        
        //num = Random.Range(0, windDiceArray.Count);
       
        //windDiceSprite.GetComponent<Image>().sprite = windDiceArray[num];

        /*

        //while we roll a parrot the same color as the player, roll again til it isn't
        //this will avoid anchors and parrots the same color
        Color tempColor = new Color();
        do {
            num = Random.Range(0, colorDiceArray.Count);

            foreach (var item in diceList.GetComponent<MasterDiceList>().diceClassList)
            {
                if (item.DiceImage.name == colorDiceArray[num].name)
                {
                    tempColor = item.DiceColor;
                }
            }
        } while (tempColor == gameLoop.players.playersList[gameLoop.playersTurn - 1].safeColor);

        colorDiceSprite.GetComponent<Image>().sprite = colorDiceArray[num];

        
        AddRats();
        FindTotalGhostShips();
        PunishPlayer();
        */
    }

    public 

    IEnumerator GetDiceValue(List<Sprite> faces, GameObject dice, int diceIndex)
    {
        yield return new WaitUntil(() => diceStart);

        diceStart = false;

        num = Random.Range(0, faces.Count);

        if (num.Equals(faces.Count - 1))
        {
            MapLoad.diceVals[diceIndex] = -1;
        }
        else
        {
            MapLoad.diceVals[diceIndex] = num + 1;
        }

        dice.GetComponent<Image>().sprite = faces[num];

        diceImage.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = faces[num].texture;

        dice.SetActive(true);

        if (num.Equals(faces.Count - 1))
        {
            ghostDiceTotal++;
        }

        explosion.gameObject.SetActive(true);

        explosion.Play();
    }

    void AddRats()
    {
        foreach (var item in inventorySystem.GetComponent<InventorySystem>().ResourceDice)
        {
            if (item.Image.name == resourceDiceSprite.GetComponent<Image>().sprite.name)
            {
                if (item.Name == "Rat")
                {
                    var currentPlayer = gameLoop.players.playersList[gameLoop.playersTurn - 1];
                    currentPlayer.ratCount += Random.Range(1, 4);
                    break;
                }
            }
        }
    }

    public void AddFog()
    {
        //loop through wind dice, if fog is showing then show run the fog system
        foreach (var item in inventorySystem.GetComponent<InventorySystem>().WindDice)
        {
            if (item.Image.name == windDiceSprite.GetComponent<Image>().sprite.name)
            {
                if (item.Name == "Fog")
                {
                    timeOfDay.ChangeTimeOfDay(3);
                    gameLoop.lighthouse.ChangeLhLight();
                    fog.SetActive(true);
                    fogDice = true;
                    break;
                }
                else
                {
                    timeOfDay.ChangeTimeOfDay(gameLoop.timeOfDayNumber);
                    fog.SetActive(false);
                    fogDice = false;
                }
            }
        }
        gameLoop.lighthouse.ChangeLhLight(); //turn on the lighthouse lights
    }

    //loop through dice and see how many ghost ships we have
    void FindTotalGhostShips()
    {
        foreach(var item in inventorySystem.GetComponent<InventorySystem>().TarShipDice)
        {
            if(item.Image.name == shipDiceSprite.GetComponent<Image>().sprite.name)
            {
                if(item.Name == "Ghost")
                {
                    ghostDiceTotal++;
                }
            }
        }

        foreach (var item in inventorySystem.GetComponent<InventorySystem>().NumberDice)
        {
            if (item.Image.name == movementDiceSprite.GetComponent<Image>().sprite.name)
            {
                if (item.Name == "Ghost")
                {
                    ghostDiceTotal++;
                }
            }
        }

        foreach (var item in inventorySystem.GetComponent<InventorySystem>().ResourceDice)
        {
            if (item.Image.name == resourceDiceSprite.GetComponent<Image>().sprite.name)
            {
                if (item.Name == "Ghost")
                {
                    ghostDiceTotal++;
                }
            }
        }

        foreach (var item in inventorySystem.GetComponent<InventorySystem>().WindDice)
        {
            if (item.Image.name == windDiceSprite.GetComponent<Image>().sprite.name)
            {
                if (item.Name == "Ghost")
                {
                    ghostDiceTotal++;
                }
            }
        }

        foreach (var item in inventorySystem.GetComponent<InventorySystem>().ParrotDice)
        {
            if (item.Image.name == colorDiceSprite.GetComponent<Image>().sprite.name)
            {
                if (item.Name == "Ghost")
                {
                    ghostDiceTotal++;
                }
            }
        }

        ghostDiceTotal += gameLoop.players.playersList[gameLoop.playersTurn - 1].ghostDice;

    }


    //punish the player if he has 3, 4, or 5 ghost dice
    public void PunishPlayer()
    {
      var currentPlayer = gameLoop.players.playersList[gameLoop.playersTurn - 1];
        if (ghostDiceTotal == 5)
        {
            Debug.Log("You Lose The Game!");
        }

        else if (ghostDiceTotal == 4)
        {
            currentPlayer.gold = 0;
            Debug.Log("You Lose Your Gold!");
        }

        else if (ghostDiceTotal == 3)
        {
            //gameLoop.newTurn();
            //loseTurn.GetComponent<LoseTurn>().ShowWindow();
            loseTurn.CheckTurnLoss();
            Debug.Log("You Lose Your Turn!");
        }     
    }
}
