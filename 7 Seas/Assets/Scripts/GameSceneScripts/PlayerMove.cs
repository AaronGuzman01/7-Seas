using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameLoop gameLoop;
    //public TileSprite tile;
    public Players players;
    public Player currentPlayer;
    public TilingEngine tE;
    bool move;
    float timeStartedMoving;
    float timeTaken = .2f;
    Vector3 startPoint;
    Vector3 endPoint;
    CameraControl camControl;
    public GameObject pirateWheel;
    public GameObject volcano;
    public Volcano myVolcano;
    public GameObject loseTurnWindow;
    public DiceManager diceManager;
    public InventorySystem inventorySystem;
    public int windCount = 0;
    public int windTotal = 0;
    public int movementCount = 0;
    public int movementTotal = 0;
    public CameraControl camContrl;
    public GameObject windUISystem;
    public RoundEvents roundEvents;
    public Text movementUIText;
    public Text windUIText;
    public GameObject alertWindow;

    // Use this for initialization
    void Start()
    {
        move = false;
        camControl = Camera.main.GetComponent<CameraControl>();
        myVolcano = volcano.GetComponent<Volcano>();

    }

    void FixedUpdate()
    {

        //if set to move, lerp between players start and end points
        if (move)
        {
            float timeSinceMoving = Time.time - timeStartedMoving;
            float percentComp = timeSinceMoving / timeTaken;
            currentPlayer.transform.position = Vector3.Lerp(startPoint, endPoint, percentComp);
            camControl.CullTiles();

            if (percentComp >= 1.0f)
            {
                move = false;
                myVolcano.CheckPlayerOnLava();
            }
        }
    }

    //Update is called once per frame
    void Update()
    {

        //get mouse left click
        if (Input.GetMouseButtonDown(0) && !pirateWheel.activeSelf && !loseTurnWindow.activeSelf &&
            !alertWindow.activeSelf && !camContrl.fullScreen && !gameLoop.gameWon)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(Mathf.FloorToInt((mousePos.x + 1f) / 2), -Mathf.FloorToInt((mousePos.y + 1f) / 2), mousePos.z);

            currentPlayer = players.playersList[gameLoop.playersTurn - 1];
            Vector2 playerVect = new Vector2(currentPlayer.transform.position.x / tE.tileSize, -currentPlayer.transform.position.y / tE.tileSize);
            Vector2 clickVect = new Vector2(mousePos.x, mousePos.y);
            float dist = Vector2.Distance(clickVect, playerVect);


            //if not already moving
            if (!move && Input.touchCount < 2 &&
                mousePos.x >= 0 && mousePos.x <= 31 &&
                mousePos.y >= 0 && mousePos.y <= 31)
            {

                var tileSprite = tE.terrainMap[(int)mousePos.x, (int)mousePos.y];

                //and if tile destination is one away
                if (dist == 1f || dist == Mathf.Sqrt(2))
                {
                    //if we wont to move to a target ship but dont have enough ammo, then dont move there
                    if (IsTargetShip(clickVect))
                    {
                        if (currentPlayer.canonBalls <= 0 || currentPlayer.powderKegs <= 0)
                        {
                            gameLoop.alertWindow.ShowAlertWindow("You don't have enough amunition!");
                            return;
                        }
                    }

                    //if player is clicking on his home port, de-rat his ship
                    if (CheckIsHomePort(currentPlayer, clickVect))
                    {
                        currentPlayer.ratCount = 0;
                        gameLoop.UpdateUI();
                    }

                    //if tile to move to is ocean or buried treasure spot
                    if (tileSprite.TileType == Tiles.shoal ||
                        tileSprite.TileType == Tiles.ocean ||
                        tileSprite.TileType == Tiles.deepblue ||
                        tileSprite.TileType == Tiles.burTreas)
                    {

                        //check if the player is going to a parrot or has enough dice moves left/wind direction
                        if (CheckMovesLeft(playerVect, clickVect))
                        {
                            //set stats for lerping to new tile
                            move = true;
                            timeStartedMoving = Time.time;
                            startPoint = currentPlayer.transform.position;
                            endPoint = new Vector3(tileSprite.TileLocation.x * tE.tileSize, -tileSprite.TileLocation.y * tE.tileSize, currentPlayer.transform.position.z);

                            //mark player's location
                            currentPlayer.shipPos = tileSprite.TileLocation;

                            //see if player is on his anchor 
                            if(CheckIsAnchor(clickVect))
                            {
                                currentPlayer.onSafeColor = true;
                            }
                            else
                            {
                                currentPlayer.onSafeColor = false;
                            }

                            //mark what type of tile the player is on
                            currentPlayer.shipOn = (int)tE.terrainMap[(int)tileSprite.TileLocation.x, (int)tileSprite.TileLocation.y].TileType;

                            RotatePlayer(playerVect, clickVect);


                            //set lat and long 
                            gameLoop.curLat.text = players.playersList[gameLoop.playersTurn - 1].shipPos.y.ToString();
                            gameLoop.curLong.text = players.playersList[gameLoop.playersTurn - 1].shipPos.x.ToString();

                            //if went to buried treasure spot, then bury the treasure
                            if (tileSprite.TileType == Tiles.burTreas && currentPlayer.gold > 0)
                            {
                                currentPlayer.buriedTreasure += currentPlayer.gold;
                                gameLoop.alertWindow.ShowAlertWindow("You've buried your treasure!");
                                currentPlayer.gold = 0;
                                gameLoop.UpdateUI();
                            }


                        }

                        
                    }

                }

                gameLoop.showAvailableMoves();

            }
        }
    }

    //take click position and compare it to target ships to see if its a target ship we clicked on
    public bool IsTargetShip(Vector2 clickV)
    {
        foreach (var tarShip in gameLoop.targetShips.tarShipList)
        {
            var tarShipPos = new Vector2(Mathf.FloorToInt((tarShip.transform.position.x + 1f) / 2), -Mathf.FloorToInt((tarShip.transform.position.y + 1f) / 2));
            if (clickV == tarShipPos)
            {
                return true;
            }
        }
        return false;
    }

    //take click position and compare it to home ports to see if its a home port
    public bool CheckIsHomePort(Player currentPlayer, Vector2 clickV)
    {
        if (clickV == currentPlayer.homePort)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //check if click position is a parrot
    public bool CheckIsParrot(Vector2 playerV, Vector2 clickV)
    {
        
        //var parrotDiceName = diceManager.GetComponent<DiceManager>().colorDiceSprite.GetComponent<Image>().sprite.name;
        foreach (var item in gameLoop.DiceList.GetComponent<MasterDiceList>().diceClassList)
        {
            //find parrot sprite needed to show from dice
            //if (parrotDiceName == item.DiceImage.name)
            {
                

            }
        }
        //if we land on a parrot dont deduct moves and return true
        if (1>2)
        {
            //Debug.Log("moving to a parrot!");
            return true;
        }
        else { return false; }
    }

    //check if click position is an anchor
    public bool CheckIsAnchor(Vector2 clickV)
    {
        var anchor = tE.circlesList[(int)clickV.y * 32 + (int)clickV.x].GetComponent<SpriteRenderer>();
        if (anchor.sprite.name == gameLoop.anchorSprite.name && anchor.material.color == currentPlayer.safeColor)
        {
            return true;
        }
        return false;
    }

    //Before moving, see if we have enough dice or parrot moves
    public bool CheckMovesLeft(Vector2 playerV, Vector2 clickV)
    {
        if (CheckIsParrot(playerV, clickV))
        {
            return true;
        }
        //else if we move in wind direction, deduct from wind and return true
        else if (windTotal > 0 && GetDirectionToMove(playerV, clickV) == gameLoop.GetDirection(gameLoop.direction))
        {
            //Debug.Log("initial wind total is: " + windTotal);
            ++windCount;
            windTotal = GetWindCount() - windCount;
            //DisplayWindUI();
            windUIText.text = "Wind Moves Left: " + windTotal;

            //hide wind arrow when wind reaches zero
            if (windTotal <= 0)
            {
                currentPlayer.transform.Find("arrow").gameObject.SetActive(false);
            }

            //Debug.Log("wind total: " + windTotal);

            return true;
        }
        //else we duduct from move dice
        else if (movementTotal > 0)
        {
            ++movementCount;
            movementTotal = GetMovementCount() - movementCount;
            //Debug.Log("movement total: " + movementTotal);
            movementUIText.text = "Moves Left: " + movementTotal;
            return true;
        }
        //else we can no longer move
        else
        {
            return false;
        }
    }

    //set amount of wind
    public void SetWindCount()
    {
        string windDiceName = diceManager.GetComponent<DiceManager>().windDiceSprite.GetComponent<Image>().sprite.name;
        int count = 0;
        foreach (var item in inventorySystem.WindDice)
        {
            if (item.Image.name == windDiceName)
            {
                count = item.WindAmnt;
                break;
            }
        }
        if (roundEvents.currentEvent == "Storm Wind 2")
            count *= 2;

        windTotal = count - windCount;
        windUIText.text = "Wind Moves Left: " + windTotal;
        if (windTotal > 0)
        {
            currentPlayer.transform.Find("arrow").gameObject.SetActive(true);
        }
    }

    //set amount of moves
    public void SetMovementCount()
    {
        string movementDiceName = diceManager.GetComponent<DiceManager>().movementDiceSprite.GetComponent<Image>().sprite.name;
        int count = 0;
        foreach (var item in inventorySystem.NumberDice)
        {
            if (item.Image.name == movementDiceName)
            {
                count = item.MovementAmnt;
                break;
            }
        }

        movementTotal = count - movementCount;
        movementUIText.text = "Moves Left: " + movementTotal;
    }

    //get amount of wind moves
    public int GetWindCount()
    {
        currentPlayer = players.playersList[gameLoop.playersTurn - 1];
        string windDiceName = diceManager.GetComponent<DiceManager>().windDiceSprite.GetComponent<Image>().sprite.name;
        int count = 0;
        foreach (var item in inventorySystem.WindDice)
        {
            if (item.Image.name == windDiceName)
            {
                count = item.WindAmnt;
                break;
            }
        }
        //if have wind, show arrow
        if (count > 0)
        {
            currentPlayer.transform.Find("arrow").gameObject.SetActive(true);
        }
        //else hide arrow
        else
        {
            currentPlayer.transform.Find("arrow").gameObject.SetActive(false);
        }
        if (roundEvents.currentEvent == "Storm Wind 2")
            count *= 2;

        return count;
    }

    //get amount of movement moves
    public int GetMovementCount()
    {
        string movementDiceName = diceManager.GetComponent<DiceManager>().movementDiceSprite.GetComponent<Image>().sprite.name;
        int count = 0;
        foreach (var item in inventorySystem.NumberDice)
        {
            if (item.Image.name == movementDiceName)
            {
                count = item.MovementAmnt;
                break;
            }
        }
        return count;
    }

    //find direction for moving with wind
    public string GetDirectionToMove(Vector2 playerV, Vector2 clickV)
    {
        //rotate the player in the direction of the movement
        float xDir = clickV.x - playerV.x;
        float yDir = clickV.y - playerV.y;
        //var playerSprite = currentPlayer.GetComponentInChildren<SpriteRenderer>();

        if (xDir == 0 && yDir == -1)//north
        {
            return "North";
        }
        else if (xDir == 0 && yDir == 1)//south
        {
            return "South";
        }
        else if (xDir == -1 && yDir == 0)//west
        {
            return "West";
        }
        else if (xDir == 1 && yDir == 0)//east
        {
            return "East";
        }
        else if (xDir == 1 && yDir == -1)//northeast
        {
            return "North East";
        }
        else if (xDir == -1 && yDir == 1)//southwest
        {
            return "South West";
        }
        else if (xDir == 1 && yDir == 1)//southeast
        {
            return "South East";
        }
        else if (xDir == -1 && yDir == -1)//northwest
        {
            return "North West";
        }
        else
        {
            return "Invalid Direction";
        }
    }

    //disble tiles not in the view of the camera
    void CullTiles()
    {
        var camVertSize = Camera.main.orthographicSize * 2;
        var camPosX = Camera.main.transform.position.x - Camera.main.orthographicSize - 1;
        var camPosY = Mathf.Abs(Camera.main.transform.position.y + Camera.main.orthographicSize - 1);

        //camera goes from x: 15 to 47 and y: -15 to -47
        for (var y = 0; y < tE.MapSize.y; y++)
        {
            for (var x = 0; x < tE.MapSize.x; x++)
            {
                tE._tiles[x, y].SetActive(false);

                if (x > Mathf.RoundToInt(camPosX) / 2 - 1 && x <= (Mathf.RoundToInt(camPosX) + camVertSize) / 2 + 1)
                {
                    if (y >= Mathf.RoundToInt(camPosY) / 2 - 1 && y <= (Mathf.RoundToInt(camPosY) + camVertSize) / 2 + 1)
                    {
                        //tE._tiles[x, y].SetActive(true);
                        tE._tiles[x, y].GetComponent<SpriteRenderer>().enabled = false;

                    }
                }

            }

        }
    }

    //rotates the player to the direction he is going
    void RotatePlayer(Vector2 vect1, Vector2 vect2)
    {
        //rotate the player in the direction of the movement
        float xDir = vect2.x - vect1.x;
        float yDir = vect2.y - vect1.y;
        var playerSprite = currentPlayer.GetComponentInChildren<SpriteRenderer>();

        if (xDir == 0 && yDir == -1)//north
        {
            playerSprite.transform.rotation = Quaternion.identity;

            playerSprite.transform.Rotate(0, 0, 90);
        }
        else if (xDir == 0 && yDir == 1)//south
        {
            playerSprite.transform.rotation = Quaternion.identity;
            playerSprite.transform.Rotate(0, 0, -90);
        }
        else if (xDir == -1 && yDir == 0)//west
        {
            playerSprite.transform.rotation = Quaternion.identity;
            playerSprite.transform.Rotate(0, 0, 180);
        }
        else if (xDir == 1 && yDir == 0)//east
        {
            playerSprite.transform.rotation = Quaternion.identity;
            playerSprite.transform.Rotate(0, 0, 0);
        }
        else if (xDir == 1 && yDir == -1)//northeast
        {
            playerSprite.transform.rotation = Quaternion.identity;
            playerSprite.transform.Rotate(0, 0, 45);
        }
        else if (xDir == -1 && yDir == 1)//southwest
        {
            playerSprite.transform.rotation = Quaternion.identity;
            playerSprite.transform.Rotate(0, 0, 225);
        }
        else if (xDir == 1 && yDir == 1)//southeast
        {
            playerSprite.transform.rotation = Quaternion.identity;
            playerSprite.transform.Rotate(0, 0, 315);
        }
        else if (xDir == -1 && yDir == -1)//northwest
        {
            playerSprite.transform.rotation = Quaternion.identity;
            playerSprite.transform.Rotate(0, 0, 135);
        }
    }



}
