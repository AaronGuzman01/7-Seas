using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundEvents : MonoBehaviour
{

    public GameLoop gameLoop;
    public Volcano volcano;
    public string currentEvent;
    public TimeOfDay timeOfDay;
    public GameObject fog;
    public DiceManager diceManager;
    public Leviathan leviathan;

    // Use this for initialization
    void Start()
    {

        //changeEvent();

    }

    public void changeEvent()
    {

        //if a new round, randomize events and select one
        if (gameLoop.playersTurn == 1)
        {
            int num = Random.Range(0, 11);
            switch (num)
            {
                case 0:
                    currentEvent = "Volcanoes Get Lava";
                    break;
                case 1:
                    currentEvent = "Lose Turn Shoal";
                    break;
                case 2:
                    currentEvent = "Lose Turn Ocean";
                    break;
                case 3:
                    currentEvent = "Lose Turn Deep Blue";
                    break;
                case 4:
                    currentEvent = "Sirens on Reef";
                    break;
                case 5:
                    currentEvent = "Storm Wind 2";
                    break;
                case 6:
                    currentEvent = "Double Gold";
                    break;
                case 7:
                    currentEvent = "Leviathan";
                    break;
                case 8:
                    currentEvent = "Fog";
                    break;
                case 9:
                    currentEvent = "Add Rats";
                    break;
                case 10:
                    currentEvent = "Extra Ghost Ship";
                    break;
                default:
                    currentEvent = "error out of range";
                    break;
            }

        }

        //run check events to see if anything has changed at the end of the round
        CheckFog();
        CheckRats();
        CheckGhostShip();
        CheckLeviathan();
        CheckWind();

    }

    //check if double wind
    public void CheckWind()
    {
        if(currentEvent == "Storm Wind 2")
        {
            gameLoop.playerMove.SetWindCount();
        }
    }

    //check if leviathan
    public void CheckLeviathan()
    {
        if(currentEvent == "Leviathan")
        {
            leviathan.eatPlayer = true;
        }
    }

    //check if fog 
    public void CheckFog()
    {
        if (currentEvent == "Fog" || diceManager.fogDice)
        {
            timeOfDay.ChangeTimeOfDay(3);
            fog.SetActive(true);
            gameLoop.lighthouse.ChangeLhLight();
        }
        else
        {
            timeOfDay.ChangeTimeOfDay(gameLoop.timeOfDayNumber);
            fog.SetActive(false);
        }
    }

    //check if rats add them
    public void CheckRats()
    {
        if (currentEvent == "Add Rats")
        {   
            //add between 1 and 3 rats to each player   
            foreach(var player in gameLoop.players.playersList)
            {
                player.ratCount += Random.Range(1, 4);
            }
            gameLoop.UpdateUI();
        }
    }

    //check for extra ghost ship and add them
    public void CheckGhostShip()
    {
        if (currentEvent == "Extra Ghost Ship")
        {
            //add 1 ghost ship dice to each player 
            foreach (var player in gameLoop.players.playersList)
            {
                player.ghostDice = 1;
            }
            //diceManager.PunishPlayer();
        }
        else
        {
            //else reset players ghost dice to zero 
            foreach (var player in gameLoop.players.playersList)
            {
                player.ghostDice = 0;
            }
        }
    }



}
