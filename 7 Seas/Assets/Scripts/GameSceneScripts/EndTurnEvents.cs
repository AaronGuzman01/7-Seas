using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnEvents : MonoBehaviour {

    public DiceManager dicemanager;
    public GameLoop gameLoop;
    public Players players;
    public EventWheel eventWheel;
    public LighthouseLights lighthouseLights;
    public Volcano volcano;
    public InventorySystem inventorySystem;
    public LoseTurn loseTurn;
    public PlayerMove playerMove;


    //all of the functions that need to be executed in order at the end of a turn
    public void EndTurn()
    {
        loseTurn.endTurnButton.SetActive(true);
        playerMove.windCount = 0;
        playerMove.movementCount = 0;
        gameLoop.newTurn();
        players.setTurn();
        gameLoop.alertWindow.HideAlertWindow();
        //dicemanager.RollDice();
        eventWheel.ResetEventWheel();

        gameLoop.UpdateUI();
        //dicemanager.AddFog();
        gameLoop.roundEvents.CheckFog();
        playerMove.windTotal = playerMove.GetWindCount();
        //playerMove.DisplayWindUI();
        playerMove.SetWindCount();
        playerMove.SetMovementCount();

        playerMove.movementTotal = playerMove.GetMovementCount();
        gameLoop.showPlayersColors();
        gameLoop.showAvailableMoves();       
        volcano.WipeLava();
        lighthouseLights.ChangeLhLight();
        inventorySystem.addResourceInventory();
        loseTurn.CheckTurnLoss();
        gameLoop.ParrotAnchorShaderChange();
        gameLoop.TarShipShaderChange();
        volcano.CheckPlayerOnLava();


    }

}
