using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using UnityEngine.Advertisements;

public class GameLoop : MonoBehaviour
{
    public int MAX_PLAYERS = 8;

    //rats stuff
    public int RATS_MAX = 16;
    public Text ratsAccumulated;
    public Text ratsMaxText;

    public int playersTurn = 1;
    public int numOfPlayers = 2;
    public GameObject diceShip;
    public GameObject diceColor;
    public GameObject diceMov;
    public GameObject diceWind;
    public GameObject diceResource;
    //public GameObject diceDirection;
    public int direction = 0;
    public List<Vector2> portsArray;
    public Button rollDiceButton;
    public TilingEngine tE;
    public Players players;
    public Player player;
    public PlayerMove playerMove;
    Color[] playerColors = new Color[9];
    SpriteRenderer spriteRend;
    public int treasureTargetAmnt;
    public RoundEvents roundEvents;
    public int timeOfDayNumber = 0;
    public TimeOfDay timeOfDay;
    public int arrowDegrees;
    public InventorySystem iS;
    public LighthouseLights lighthouse;
    public EventWheel eventWheel;
    public TargetShips targetShips;
    public InventorySystem inventorySystem;
    public AlertWindow alertWindow;
    bool saveExists = false;
    bool noMovesAlert = false;
    public EndTurnEvents endTurnEvents;
    public LoseTurn loseTurn;

    public List<int> playersOrder = new List<int>();


    public string shipType;
    public string colorType;
    public string movType;
    public string windType;
    public string resourceType;

    //Lat and Long
    public Text prevLat;
    public Text curLat;
    public Text prevLong;
    public Text curLong;

    //treasure texts
    public Text treasureText;
    public Text treasureOnShipText;
    public Text treasureBuriedText;

    //toggle debug menu
    bool debugOn = false;
    public GUIStyle style; //style for debug menu
    List<string> touchInfo = new List<string>();
    public DiceManager diceManager;
    public GameObject DiceList;

    //screen info
    float screenWidth;
    float screenHeight;
    float screenRatio;
    int heightRatio;

    //parrot stuff
    Sprite parrotSprite;
    Color parrotColor;

    //anchor stuff
    public Sprite anchorSprite;
    Sprite tempSprite;
    Sprite tempAnchorSprite;
    //Sprite tempSprite = new Sprite ;
    //Sprite tempAnchorSprite = new Sprite;

    //time wheel
    Transform tW;
    Quaternion timeWheelRot;

    //arrow
    Transform arrowTrans;
    Quaternion arrowRot;

    //camera
    public Camera mainCamera;

    //save engine
    public SaveEngine saveEngine;
    public GameControl gameControl;

    //win scenario stuff
    public GameObject winWindow;
    public Text gameWinText;
    public AudioClip gameWinMusic;
    public Camera hudCamera;
    public bool gameWon = false;
    public GameObject pirateWheel;
    public GameObject eventSpinButton;

    float orthoSize = 9f;

    void Awake()
    {
        //during the start, if we have a load game requested then load, otherwise get info from the setup screen
        if (PlayerPrefs.HasKey("LoadGame") && PlayerPrefs.GetInt("LoadGame") == 1)
        {
            numOfPlayers = ES2.Load<int>(Application.persistentDataPath + "/numOfPlayers");
            treasureTargetAmnt = ES2.Load<int>(Application.persistentDataPath + "/treasureTargetAmnt");
        }
        else
        {
            numOfPlayers = PlayerPrefs.GetInt("PlayerCount");
            treasureTargetAmnt = PlayerPrefs.GetInt("TreasureAmount");
        }
    }


    // Use this for initialization
    void Start()
    {

        if (ES2.Exists(Application.persistentDataPath + "/saveExists"))
        {
            saveExists = ES2.Load<bool>(Application.persistentDataPath + "/saveExists");
        }

        treasureText.text = PlayerPrefs.GetInt("TreasureAmount").ToString();
        tW = GameObject.Find("TimeWheelSky").transform;

        //beging time of day at morning
        timeOfDayNumber = 0;


        //DiceMovement dM = diceMov.GetComponent<DiceMovement>();
        portsArray = new List<Vector2>();
        findPorts();
        players.InitializePlayer();





        //set camera to player
        mainCamera.transform.position = new Vector3(Mathf.Clamp(players.playersList[0].transform.position.x, orthoSize - 1, 63 - orthoSize),
            Mathf.Clamp(players.playersList[0].transform.position.y, orthoSize - 63, 1 - orthoSize), mainCamera.transform.position.z);
        //mainCamera.GetComponent<CameraControl>().CullTiles();

        player.gameObject.SetActive(false);


        //set all the colors
        playerColors[0] = new Color32(255, 204, 0, 255);//amber
        playerColors[1] = new Color32(0, 204, 255, 255);//blue
        playerColors[2] = new Color32(204, 102, 51, 255);//brown
        playerColors[3] = new Color32(51, 204, 102, 255);//green
        playerColors[4] = new Color32(255, 102, 51, 255);//orange
        playerColors[5] = new Color32(204, 51, 255, 255);//purple
        playerColors[6] = new Color32(255, 0, 0, 255);//red
        playerColors[7] = new Color32(255, 255, 255, 255);//white
        playerColors[8] = Color.clear;//ghost ship



        int j = 0;
        //change the color of the octagon and set the player's safe color
        for (int i = 0; i < MAX_PLAYERS; ++i)
        {
            int num = i + 1;
            if (PlayerPrefs.GetInt("Player" + num) == 1)
            {
                players.playersList[j].octagon.GetComponent<Renderer>().material.color = playerColors[i];
                players.playersList[j].safeColor = playerColors[i];
                ++j;
            }

        }

        players.SetPortColors();

        //set initial arrow
        changeArrow();

        //randomize dice at the start
        if (PlayerPrefs.GetString("cannonVisited") == "false")
        {
            //diceManager.RollDice();
            playerMove.windTotal = playerMove.GetWindCount();
            playerMove.movementTotal = playerMove.GetMovementCount();
            //playerMove.DisplayWindUI();
            playerMove.SetWindCount();
            playerMove.SetMovementCount();

            //initialize playersorder list
            for (int i = 1; i < 8 + 1; ++i)
            {
                playersOrder.Add(i);
            }


            //show only color circles for the player
            showPlayersColors();

            //set parrot color
            //changeParrot();

            //highlight avail moves
            showAvailableMoves();

            //set lat and long 
            prevLat.text = players.playersList[playersTurn - 1].shipPos.y.ToString();
            prevLong.text = players.playersList[playersTurn - 1].shipPos.x.ToString();

            //add initial inventory amounts to the first player
            iS.addResourceInventory();

            //saveEngine.Load();

            //set game boarder color to player color
            players.setBoarderColor();

            //initialize first players inventory UI
            UpdateUI();


            //Advertisement.Show();

            //set lat and long 
            curLat.text = players.playersList[playersTurn - 1].shipPos.y.ToString();
            curLong.text = players.playersList[playersTurn - 1].shipPos.x.ToString();
        }


        if (PlayerPrefs.GetString("cannonVisited") == "true")
        {
            saveEngine.Load();
            //set camera to player
            mainCamera.transform.position = new Vector3(Mathf.Clamp(players.playersList[playersTurn - 1].transform.position.x, orthoSize - 1, 63 - orthoSize),
                Mathf.Clamp(players.playersList[playersTurn - 1].transform.position.y, orthoSize - 63, 1 - orthoSize), mainCamera.transform.position.z);
            //Debug.Log("save engine");
            CheckForWin();
            if (!gameWon)
            {
                UpdateUI();
                endTurnEvents.EndTurn();
                mainCamera.transform.position = new Vector3(Mathf.Clamp(players.playersList[playersTurn - 1].transform.position.x, orthoSize - 1, 63 - orthoSize),
                    Mathf.Clamp(players.playersList[playersTurn - 1].transform.position.y, orthoSize - 63, 1 - orthoSize), mainCamera.transform.position.z);
            }
            
        }


        //handle event wheel on return to screen
        /*if (playersTurn == 1)
        {
            eventWheel.ResetEventWheel();
        }*/
        if (playersTurn != 1 || PlayerPrefs.GetString("wheelSpun") == "true")
        {
            eventWheel.endTurnButton.SetActive(true);
            eventWheel.pirateWheel.SetActive(false);
            eventWheel.evSpinButton.SetActive(false);
            eventWheel.toggleActive = false;
            eventWheel.toggleRotate = false;
            eventWheel.textgen = false;
            eventWheel.rotZ = UnityEngine.Random.Range(740, 1040);
            eventWheel.PirateText.text = "";
        }

        //if we requested to load game then load game
        if(PlayerPrefs.HasKey("LoadGame") && PlayerPrefs.GetInt("LoadGame") == 1)
        {
            Debug.Log("Loading game!!!!!!!!!!");
            //PlayerPrefs.SetInt("LoadGame", 0);
            saveEngine.Load();

            for (int i = 0; i < numOfPlayers; ++i)
            {

                players.playersList[i].octagon.GetComponent<Renderer>().material.color = players.playersList[i].safeColor;

            }

            players.SetPortColors();
            players.setBoarderColor();
            UpdateUI();
            mainCamera.transform.position = new Vector3(Mathf.Clamp(players.playersList[playersTurn - 1].transform.position.x, orthoSize - 1, 63 - orthoSize),
                Mathf.Clamp(players.playersList[playersTurn - 1].transform.position.y, orthoSize - 63, 1 - orthoSize), mainCamera.transform.position.z);
            treasureText.text = PlayerPrefs.GetInt("TreasureAmount").ToString();

        }

    }

    private void FixedUpdate()
    {
        //rotate the time wheel      
        tW.rotation = Quaternion.RotateTowards(tW.rotation, timeWheelRot, Time.deltaTime * 150f);

        //rotate direction arrow
        if (!gameWon)
        {
            arrowTrans.rotation = Quaternion.RotateTowards(arrowTrans.rotation, arrowRot, Time.deltaTime * 120f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //toggle debug
        if (Input.GetKeyDown(KeyCode.Z) || Input.touchCount == 10)
        {
            debugOn = !debugOn;
        }

        //add mobile touches to an array
        if (debugOn)
        {
            touchInfo.Clear();
            for (int i = 0; i < Input.touchCount; ++i)
            {
                Touch touch = Input.GetTouch(i);
                string str = "Touch #" + (i + 1) + " at " + touch.position.ToString() + ", r=" + touch.radius;
                touchInfo.Add(str);
            }
        }

        //roll dice button if spacebar pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rollDiceButton.onClick.Invoke();

        }

        //set lat and long 
        curLat.text = players.playersList[playersTurn - 1].shipPos.y.ToString();
        curLong.text = players.playersList[playersTurn - 1].shipPos.x.ToString();


    }

    //load all port locations into an array and randomize it
    private void findPorts()
    {
        //int i = 0;
        //load ports into array
        for (var y = 0; y < tE.MapSize.y; y++)
        {
            for (var x = 0; x < tE.MapSize.x; x++)
            {
                if (tE.terrainMap[x, y].TileType == Tiles.ports)
                {
                    portsArray.Add(new Vector2(y, x));

                }
            }
        }

        //randomize array of ports
        for (var j = portsArray.Count - 1; j > 0; j--)
        {
            var rand = UnityEngine.Random.Range(0, j);
            var tmp = portsArray[j];
            portsArray[j] = portsArray[rand];
            portsArray[rand] = tmp;
        }
    }


    public void changeArrow()
    {
        if (!gameWon)
        {
            //loop through players and only show current player's arrow
            for (int i = 0; i < numOfPlayers; ++i)
            {
                spriteRend = players.playersList[i].arrow.GetComponent<SpriteRenderer>();
                if (players.playersList[i] == players.playersList[playersTurn - 1])
                {
                    spriteRend.enabled = true;
                }
                else if (players.playersList[i] != players.playersList[playersTurn - 1])
                {
                    spriteRend.enabled = false;
                }
            }

            //randomize and set the direction
            direction = UnityEngine.Random.Range(0, 8);

            if(saveExists)
            {
                direction = ES2.Load<int>(Application.persistentDataPath + "/windDirection");
            }

            //define the arrow's direction
            switch (direction)
            {
                case 0:
                    arrowDegrees = 90;//North
                    break;
                case 1:
                    arrowDegrees = -90;//South
                    break;
                case 2:
                    arrowDegrees = 0;//East
                    break;
                case 3:
                    arrowDegrees = 180;//West
                    break;
                case 4:
                    arrowDegrees = 45;//NorthEast
                    break;
                case 5:
                    arrowDegrees = 135;//NorthWest
                    break;
                case 6:
                    arrowDegrees = 315;//SouthEast
                    break;
                case 7:
                    arrowDegrees = 225;//SouthWest
                    break;
                default:
                    Debug.Log("Error, that is not a valid direction!");
                    break;
            }

            //get current arrow transform and set the arrow's rotation
            arrowTrans = players.playersList[playersTurn - 1].arrow.transform;
            arrowRot = Quaternion.Euler(0, 0, arrowDegrees);
        }
    }


    //only show protection and free colors
    public void showPlayersColors()
    {
        //parrotSprite = Sprite();

        //look at parrot dice and player color to get parrots and anchors
        foreach (var item in DiceList.GetComponent<MasterDiceList>().diceClassList)
        {
            //find parrot sprite needed to show from dice
            if (diceColor.GetComponent<Image>().sprite.name == item.DiceImage.name)
            {
                parrotSprite = item.ChildDiceImage;
                //parrotColor = item.DiceColor;

            }
            //find which board colors are the same as player color for anchor spots
            if (item.DiceColor == players.playersList[playersTurn - 1].safeColor)
            {
                tempAnchorSprite = item.ChildDiceImage;
                //Debug.Log("Found anchor color!");
            }

        }


        //hide all the parrots/anchors we dont need
        foreach (GameObject tiles in tE.circlesList)
        {
            var ren = tiles.GetComponent<SpriteRenderer>();
            //reset tiles
            if (!ren.enabled)
            {
                ren.enabled = true;

            }
            ren.material.color = Color.white;
            if (ren.sprite.name == anchorSprite.name)
            {
                ren.sprite = tempSprite;
            }
        }

        foreach (GameObject tiles in tE.circlesList)
        {
            var ren = tiles.GetComponent<SpriteRenderer>();

            //change to anchor image
            if (ren.sprite.name == tempAnchorSprite.name)
            {
                tempSprite = ren.sprite;
                ren.sprite = anchorSprite;
                ren.material.color = players.playersList[playersTurn - 1].safeColor;
            }

            //hide all the circles we dont need
            else if (parrotSprite.name != ren.sprite.name)
            {
                ren.enabled = false;
            }

        }
    }

    //highlight squares the player can move to
    public void showAvailableMoves()
    {
        if (!gameWon)
        {
            bool hasAMove = false;
            //var playerColor = players.playersList[playersTurn - 1].safeColor;
            foreach (var tileHilights in tE.highlightTileList)
            {
                //hide all highlights at start
                var ren = tileHilights.GetComponent<SpriteRenderer>();
                ren.enabled = false;

                var loc = new Vector3(Mathf.FloorToInt((tileHilights.transform.position.x + 1f) / 2), -Mathf.FloorToInt((tileHilights.transform.position.y + 1f) / 2), tileHilights.transform.position.z);
                var plLoc = players.playersList[playersTurn - 1].shipPos;


                //loop through tiles around player
                for (int y = -1; y < 2; ++y)
                {
                    for (int x = -1; x < 2; ++x)
                    {
                        //if player has movement dice left then show all moves
                        if (playerMove.movementTotal > 0)
                        {
                            if (loc.x == plLoc.x + x && loc.y == plLoc.y + y)
                            {
                                ren.enabled = true;
                                hasAMove = true;
                            }
                        }
                        //if player has wind dice left but no movement
                        else if (playerMove.windTotal > 0)
                        {
                            //if tile is in bonus direction then highlight
                            if (GetDirection(direction) == playerMove.GetDirectionToMove(plLoc, loc))
                            {
                                ren.enabled = true;
                                hasAMove = true;
                            }
                            //if there is a parrot tile near then highlight
                            if (loc.x == plLoc.x + x && loc.y == plLoc.y + y && plLoc != new Vector2(loc.x, loc.y))
                            {
                                if (playerMove.CheckIsParrot(plLoc, loc))
                                {
                                    ren.enabled = true;
                                    hasAMove = true;
                                }
                            }
                        }
                        //else just highlight any parrots
                        else if (loc.x == plLoc.x + x && loc.y == plLoc.y + y)
                        {
                            if (playerMove.CheckIsParrot(plLoc, loc) && plLoc != new Vector2(loc.x, loc.y))
                            {
                                hasAMove = true;
                                ren.enabled = true;
                            }
                        }

                    }
                }
            }

            //there are no more moves, notify the player!
            if (!hasAMove && !noMovesAlert)
            {
                noMovesAlert = true;
                alertWindow.ShowAlertWindow("No more moves left!");
            }

        }
    }

    //hide all highlights on the board
    public void HideHighlights()
    {
        foreach (var tileHilights in tE.highlightTileList)
        {
            var ren = tileHilights.GetComponent<SpriteRenderer>();
            ren.enabled = false;
        }
    }

    public void TarShipShaderChange()
    {
        foreach(var tarShip in targetShips.tarShipList)
        {
            var posX = tarShip.transform.position.x/2;
            var posY = tarShip.transform.position.y/-2;
            if (posX >= 0)
            {
                var rend = tE._tiles[(int)posX, (int)posY].GetComponent<SpriteRenderer>();
                var tarRend = tarShip.GetComponent<SpriteRenderer>();
                //Debug.Log(posX + ", " + posY);
                if (rend.material.shader == lighthouse.shader1)
                {
                    tarRend.material.shader = lighthouse.shader1;
                }
                else if (rend.material.shader == lighthouse.shader2)
                {
                    tarRend.material.shader = lighthouse.shader3;
                }
            }
        }
    }

    public void ParrotAnchorShaderChange()
    {
        for (int y = 0; y < 32; ++y)
        {
            for (int x = 0; x < 32; ++x)
            {
                var rend = tE._tiles[y, x].GetComponent<SpriteRenderer>();
                var circleRend = tE.circlesList[(x * 32) + y].GetComponent<SpriteRenderer>();
                //tE.circlesList[(x * 32) + y].GetComponent<SpriteRenderer>().material.shader = tE._tiles[y,x].GetComponent<SpriteRenderer>().material.shader;
                if (rend.material.shader == lighthouse.shader1)
                {
                    circleRend.material.shader = lighthouse.shader1;
                    //tE.circlesList[(x * 32) + y].GetComponent<SpriteRenderer>().material.color = players.playersList[playersTurn - 1].safeColor;
                }
                else if (rend.material.shader == lighthouse.shader2)
                {
                    circleRend.material.shader = lighthouse.shader3;
                    //.circlesList[(x * 32) + y].GetComponent<SpriteRenderer>().material.color = players.playersList[playersTurn - 1].safeColor;
                }
            }
        }
    }

    public void randomizePlayers()
    {

        //randomize players for new round
        for (var j = numOfPlayers - 1; j > 0; j--)
        {
            var rand = UnityEngine.Random.Range(0, j);
            var tmp = players.playersList[j];
            players.playersList[j] = players.playersList[rand];
            players.playersList[rand] = tmp;
        }

        playersOrder.Clear();
        for (int i = 0; i < numOfPlayers; ++i)
        {
            playersOrder.Add((int)players.playersList[i].name[players.playersList[i].name.Length - 1] - 48);

        }

    }

    public void LoadPlayerOrder()
    {

        for (int i = 0; i < numOfPlayers; ++i)
        {
            players.playersList[i] = players.originalPlayersList[playersOrder[i] - 1];
        }
    }

    public void newTurn()
    {
        noMovesAlert = false;
        //increment the player's turn and wrap it around
        ++playersTurn;

        //wrap players around
        if (playersTurn > numOfPlayers)
        {
            PlayerPrefs.SetString("wheelSpun", "false");
            //Advertisement.Show();
            randomizePlayers();
            playersTurn = 1;

            //increase the time of day each round
            ++timeOfDayNumber;
            //wrap the time of day back around
            if (timeOfDayNumber > 5)
            {
                timeOfDayNumber = 0;
            }
            //feed the time of day into the function
            timeOfDay.ChangeTimeOfDay(timeOfDayNumber);
            timeWheelRot = Quaternion.Euler(0, 0, 360 / 6 * timeOfDayNumber);

        }


        //move the arrow to the direction
        changeArrow();

        //set lat and long 
        prevLat.text = players.playersList[playersTurn - 1].shipPos.y.ToString();
        prevLong.text = players.playersList[playersTurn - 1].shipPos.x.ToString();

    }

    public void UpdateUI()
    {
        //initiate player's treasure amounts to the UI
        treasureOnShipText.text = players.playersList[playersTurn - 1].gold.ToString();
        treasureBuriedText.text = players.playersList[playersTurn - 1].buriedTreasure.ToString();

        //initiate rats amounts in UI
        ratsMaxText.text = RATS_MAX.ToString();
        ratsAccumulated.text = players.playersList[playersTurn - 1].ratCount.ToString();

        //kegs and cb count
        inventorySystem.kegText.text = "Kegs: " + players.playersList[playersTurn - 1].powderKegs;
        inventorySystem.cbText.text = "CBs: " + players.playersList[playersTurn - 1].canonBalls;
    }

    //see if win scenario is reached, if so then change game to show the winner
    public void CheckForWin()
    {
        foreach (var player in players.playersList)
        {
            if ((player.gold + player.buriedTreasure) >= PlayerPrefs.GetInt("TreasureAmount"))
            {
                gameWon = true;
                HideHighlights();
                mainCamera.orthographicSize = 5;
                pirateWheel.SetActive(false);
                eventSpinButton.SetActive(false);
                winWindow.SetActive(true);
                alertWindow.HideAlertWindow();
                loseTurn.HideWindow();
                
                gameWinText.text = "Player " + playersTurn + " Wins!";
                hudCamera.GetComponent<AudioSource>().clip = gameWinMusic;
                hudCamera.GetComponent<AudioSource>().Play();
                player.arrow.SetActive(false);
            }
        }
    }

    //find the direction
    public string GetDirection(int direction)
    {
        if (direction == 0)
            return "North";
        else if (direction == 1)
            return "South";
        else if (direction == 2)
            return "East";
        else if (direction == 3)
            return "West";
        else if (direction == 4)
            return "North East";
        else if (direction == 5)
            return "North West";
        else if (direction == 6)
            return "South East";
        else if (direction == 7)
            return "South West";
        else
            return "Error";
    }

    public void ToggleDebugMenu()
    {
        debugOn = !debugOn;
    }

    //DEBUG MENU  --press Z to activate
    void OnGUI()
    {
        if (debugOn)
        {
            int i = 0;
            int offset = 30;

            //gui background
            GUI.backgroundColor = new Color(0, 0, 0, 1);
            //GUI.Button(new Rect(0, 0, offset * diceManager.resourceDiceArray.Count + 10, Screen.height), "");

            //debug labels
            GUILayout.Label("FPS: " + (int)(1.0f / Time.smoothDeltaTime), style);
            GUILayout.Label("Difficulty: " + PlayerPrefs.GetString("Difficulty"), style);
            GUILayout.Label("Num of Players: " + PlayerPrefs.GetInt("PlayerCount"), style);
            GUILayout.Label("Playing to: " + PlayerPrefs.GetInt("TreasureAmount") + " gold", style);
            GUILayout.Label("Current Player: " + players.playersList[playersTurn - 1].name, style);
            GUILayout.Label("Current Player's Kegs: " + players.playersList[playersTurn - 1].powderKegs, style);
            GUILayout.Label("Current Player's Canon Balls: " + players.playersList[playersTurn - 1].canonBalls, style);
            GUILayout.Label("Treasure on Ship: " + players.playersList[playersTurn - 1].gold.ToString(), style);
            GUILayout.Label("Buried Treasure: " + players.playersList[playersTurn - 1].buriedTreasure.ToString(), style);
            int num = players.playersList[playersTurn - 1].buriedTreasure + players.playersList[playersTurn - 1].gold;
            GUILayout.Label("Total Treasure: " + num.ToString(), style);
            GUILayout.Label("Lat/Long: (" + players.playersList[playersTurn - 1].shipPos.y + ", " +
                players.playersList[playersTurn - 1].shipPos.x + ")", style);
            GUILayout.Label("Screen Width: " + Screen.width, style);
            GUILayout.Label("Screen Height: " + Screen.height, style);
            GUILayout.Label("Direction: " + GetDirection(direction), style);
            GUILayout.Label("Time of Day: " + timeOfDay.time, style);
            GUILayout.Label("Event: " + roundEvents.currentEvent, style);

            //calculate screen ratio
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            screenRatio = screenWidth / screenHeight;
            heightRatio = (int)Mathf.Round((16 / screenRatio));
            GUILayout.Label("Screen Aspect Fraction: " + Mathf.Round(screenRatio * 100) / 100, style);
            GUILayout.Label("Screen Aspect Ratio: 16:" + heightRatio, style);

            //touch debug labels
            foreach (string str in touchInfo)
            {
                GUILayout.Label(str, style);
            }

            /*
            //show our entire dice cup dice
            foreach (var item in diceManager.shipDiceArray)
            {
                Sprite s = item;
                Texture t = s.texture;
                Rect tr = s.textureRect;
                Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                GUI.DrawTextureWithTexCoords(new Rect(i, Screen.height - offset * 5, offset, offset), t, r);
                i += offset;
            }
            int j = 0;
            foreach (var item in diceManager.resourceDiceArray)
            {
                Sprite s = item;
                Texture t = s.texture;
                Rect tr = s.textureRect;
                Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                GUI.DrawTextureWithTexCoords(new Rect(j, Screen.height - offset * 4, offset, offset), t, r);
                j += offset;
            }
            int k = 0;
            foreach (var item in diceManager.movementDiceArray)
            {
                Sprite s = item;
                Texture t = s.texture;
                Rect tr = s.textureRect;
                Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                GUI.DrawTextureWithTexCoords(new Rect(k, Screen.height - offset * 3, offset, offset), t, r);
                k += offset;
            }
            int l = 0;
            foreach (var item in diceManager.windDiceArray)
            {
                Sprite s = item;
                Texture t = s.texture;
                Rect tr = s.textureRect;
                Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                GUI.DrawTextureWithTexCoords(new Rect(l, Screen.height - offset * 2, offset, offset), t, r);
                l += offset;
            }
            int m = 0;
            foreach (var item in diceManager.colorDiceArray)
            {
                Sprite s = item;
                Texture t = s.texture;
                Rect tr = s.textureRect;
                Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                GUI.DrawTextureWithTexCoords(new Rect(m, Screen.height - offset * 1, offset, offset), t, r);
                m += offset;
            }

            */


        }

    }


}
