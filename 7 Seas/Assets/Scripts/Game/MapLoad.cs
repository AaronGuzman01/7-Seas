using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapLoad : MonoBehaviour
{
    public static bool[] activePlayers;

    public static bool isMoving;
    public static bool isRolling;
    private Vector3 original;
    private Vector3 target;
    private float moveTime = 0.2f;
    private bool clickable;

    public Canvas buttons;
    public Text toggleText;
    public Text treasureGoal;
    public Text treasureTotal;
    public Text treasureCurrent;
    public Text player;

    public Camera main;
    public GameObject[] mainGUI;
    public GameObject[] diceGUI;
    public GameObject[] cameraGUI;
    public GameObject[] portGUI;

    public GameObject[] ships;
    public Sprite[] mapObjects;
    public Canvas[] objectContainers;
    public GameObject[] positionTiles;
    public Canvas tiles;
    public Button[] hiddenBtns;
    public Material[] skyBox;
    public List<Sprite> portImages;
    public List<GameObject> ports;
    public List<GameObject> treasureShips;
    public List<GameObject> monsters;
    public GameObject sirenObj;
    public GameObject arrow;
    public GameObject movingFX;
    public GameObject navMenu;
    public Text[] navTexts;
    public Image playerImg;

    public float time;
    public float degrees;
    
    float accel;

    int leftBound;
    int rightBound;
    int upperBound;
    int lowerBound;

    Tilemap tilemap;

    HashSet<Vector3Int> positions;
    HashSet<Vector3Int> validPos;
    HashSet<Vector3Int> portPos;
    HashSet<Vector3Int> playerPos;
    HashSet<Vector3Int> shipPos;
    HashSet<Vector3Int> monsterPos;

    GameObject currArrow;
    Vector3Int windDirection;

    GameObject[] players;
    List<PlayerShip> shipInfo;
    List<int> playerNums;

    public static bool diceSet = false;
    public static int playerNum;
    public static int[] diceVals;
    public static bool rats = false;
    public static bool fog = false;
    public static bool siren = false;
    public int playerIndex;
    public static int diceIndex;
    public int moveCount;

    static bool continueGame = false;
    public static int maxPlayers;
    float arrowRot;
    bool posSet = false;
    bool rotate = false;
    bool port = false;
    bool playerCombat = false;
    bool shipCombat = false;
    bool monsterCombat = false;
    Vector3 currPos;

    int[,] tilesInMap;
    int[,] objectsInMap;

    RandomPosition objectGenerator;

    public int[] cams;
    public static int camNum;

    public static int maxCams;

    void Start()
    {
        positions = new HashSet<Vector3Int>();
        validPos = new HashSet<Vector3Int>();
        portPos = new HashSet<Vector3Int>();
        playerPos = new HashSet<Vector3Int>();
        shipPos = new HashSet<Vector3Int>();
        monsterPos = new HashSet<Vector3Int>();
        tilesInMap = new int[80, 80];
        objectsInMap = new int[80, 80];
        shipInfo = new List<PlayerShip>();
        playerNums = new List<int>();
        activePlayers = new bool[8];
        diceVals = new int[5];
        cams = new int[8];
        maxPlayers = 0;
        diceIndex = 0;
        moveCount = 0;

        ResultsManager.skyBox = skyBox[0];

        treasureGoal.text = "Treasure Goal: " + PlayerPrefs.GetFloat("End").ToString();

        //Gets selected players
        for (int i = 0; i < 8; i++)
        {
            if (!PlayerPrefs.GetInt("Player" + (i + 1).ToString()).Equals(0))
            {
                activePlayers[i] = true;

                playerNums.Add(i + 1);

                maxPlayers++;
            }
            else
            {
                activePlayers[i] = false;
            }
        }

        players = new GameObject[maxPlayers];

        //Sets active players
        for (int i = 0; i < maxPlayers; i++)
        {
            players[i] = ships[playerNums[i] - 1];

            int mastCount = 0, cannonCount = 0, crew = 0, damage = 0, treasure = 0;
            int[] masts = new int[3];
            int[] cannons = new int[5];
            string[] lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/Player" + (i + 1).ToString() + ".txt");

            foreach (string line in lines)
            {
                string temp;

                if (line.Contains("mast"))
                {
                    if (line.Contains((mastCount + 1).ToString()) && mastCount < 3)
                    {
                        temp = line.Trim();

                        if (temp.EndsWith("s"))
                        {
                            masts[mastCount] = 1;
                        }
                        else if (temp.EndsWith("l"))
                        {
                            masts[mastCount] = 2;
                        }
                        else
                        {
                            masts[mastCount] = 0;
                        }

                        mastCount++;
                    }
                }

                if (line.Contains("cannon"))
                {
                    if (line.Contains((cannonCount + 1).ToString()) && cannonCount < 5)
                    {
                        temp = line.Trim();

                        if (temp.EndsWith("s"))
                        {
                            cannons[cannonCount] = 1;
                        }
                        else if (temp.EndsWith("l"))
                        {
                            cannons[cannonCount] = 2;
                        }
                        else
                        {
                            cannons[cannonCount] = 0;
                        }

                        cannonCount++;
                    }
                }

                if (line.Contains("crew"))
                {
                    if (line.Contains((crew + 1).ToString()) && crew < 2)
                    {
                        temp = line.Trim();

                        if (temp.EndsWith("t"))
                        {
                            crew++;
                        }
                    }
                }

                if (line.Contains("treasure"))
                {
                    if (line.Contains((treasure + 1).ToString()) && treasure < 3)
                    {
                        temp = line.Trim();

                        if (temp.EndsWith("t"))
                        {
                            treasure++;
                        }
                    }
                }

                if (line.Contains("damage"))
                {
                    if (line.Contains((damage + 1).ToString()) && damage < 2)
                    {
                        temp = line.Trim();

                        if (temp.EndsWith("t"))
                        {
                            damage++;
                        }
                    }
                }
            }

            shipInfo.Add(new PlayerShip(i + 1, players[i], masts, cannons, crew, treasure, damage));
        }

        //Destroy inactive players
        for (int i = 0; i < 8; i++)
        {
            if (activePlayers[i] == false)
            {
                Destroy(ships[i]);
            }
        }

        for (int i = 7; i >= 0; i--)
        {
            if (activePlayers[i] == false)
            {
                portImages.RemoveAt(i);
                ports.RemoveAt(i);
            }
        }

        /*
        //Assign active ships and cameras to player numbers
        int j = 0;
        for (int i = 0; i < 8; i++)
        {
            if (activePlayers[i] == true)
            {
                players[j] = ships[i];
                cams[j] = i;
                j++;
            }
        }
        */

        camNum = cams[0];
        playerNum = playerNums[0] - 1;
        playerIndex = 0;

        leftBound = -34;
        rightBound = 46;
        upperBound = 32;
        lowerBound = -48;

        tilemap = GetComponent<Tilemap>();

        clickable = true;

        LoadMap();

        SetPlayerShips();
        SetTreasureShips();
        SetMonsters(); 

        //Testing for switching sky
        //StartCoroutine(ChangeSky());

        maxCams = maxPlayers;

        objectGenerator = new RandomPosition(shipInfo, ports, objectContainers[1], 0, 1);
        objectGenerator.SetTilemap(tilemap);
        objectGenerator.GeneratePortPosition(tilesInMap, objectsInMap);

        objectGenerator = new RandomPosition(treasureShips, objectContainers[2], 2, 5);
        objectGenerator.SetTilemap(tilemap);
        objectGenerator.GeneratePosition(tilesInMap, objectsInMap);

        objectGenerator = new RandomPosition(monsters, objectContainers[3], 1, 5);
        objectGenerator.SetTilemap(tilemap);
        objectGenerator.GeneratePosition(tilesInMap, objectsInMap);

        objectGenerator = new RandomPosition(sirenObj, objectContainers[4], 3, 1);
        objectGenerator.SetTilemap(tilemap);
        objectGenerator.GenerateSirenPosition(tilesInMap, objectsInMap);

        DisplayMoves(1);
    }

    void Update()
    {
        MoveShip(players[playerIndex]);

        if (!isMoving && !isRolling && !playerCombat && !shipCombat && !monsterCombat && !port)
        {
            SetGUI(true, mainGUI);
            SetGUI(true, cameraGUI);
        }
        else
        {
            SetGUI(false, mainGUI);
            SetGUI(false, cameraGUI);
        }

        if (continueGame)
        {
            SetGUI(true, diceGUI);
            SetGUI(false, portGUI);

            main.enabled = true;
            tiles.gameObject.SetActive(true);
            clickable = true;
            diceSet = true;
            port = false;
            playerCombat = false;
            shipCombat = false;
            monsterCombat = false;
            continueGame = false;
            RenderSettings.skybox = skyBox[0];
            CannonMinigame.DestroyObjects();

            if (currArrow)
            {
                currArrow.SetActive(true);
            }
        }

        if (diceIndex >= 3)
        {
            ClearActiveTiles();

            diceSet = false;
        }

        treasureCurrent.text = "Player Treasure: " + shipInfo[playerIndex].GetCurrentTreasure().ToString();
        treasureTotal.text = "Total Treasure: " + shipInfo[playerIndex].GetTotalTreasure().ToString();
        player.text = "Player: " + (playerIndex + 1).ToString();
        playerImg.sprite = portImages[playerIndex];

        if (rats)
        {
            Debug.Log("Rats");

            rats = false;
        }

        if (fog)
        {
            Debug.Log("fog");

            fog = false;
        }

        if (siren)
        {
            Debug.Log("siren");

            siren = false;
        }

        /*
        //Arrow Key Movement
        if (Input.GetKey(KeyCode.UpArrow) && !isMoving && player1.transform.position.z < upperBound - 10)
        {
            Debug.Log("Up");
            StartCoroutine(MovePlayer(player1, Vector3.forward));
        }

        if (Input.GetKey(KeyCode.DownArrow) && !isMoving && player1.transform.position.z > lowerBound + 1)
        {
            Debug.Log("Down");
            StartCoroutine(MovePlayer(player1, Vector3.back));
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !isMoving && player1.transform.position.x > leftBound + 1)
        {
            Debug.Log("Left");
            StartCoroutine(MovePlayer(player1, Vector3.left));
        }

        if (Input.GetKey(KeyCode.RightArrow) && !isMoving && player1.transform.position.x < rightBound)
        {
            Debug.Log("Right");
            StartCoroutine(MovePlayer(player1, Vector3.right));
        }
        */
    }

    private IEnumerator ChangeSky()
    {
        foreach (var sky in skyBox)
        {
            yield return new WaitForSeconds(15);

            RenderSettings.skybox = sky;
        }
    }

    void SetPlayerShips()
    {
        int count = 0;
        Transform transform;

        foreach (GameObject player in players) {
            transform = player.transform;

           // if (objectsInMap[13 + 34, (2 + count - 32) * -1] == 0)
           // {
                transform.position = tilemap.GetCellCenterWorld(new Vector3Int(18, -24 + count, 0));

                objectsInMap[18 + 34, (-24 + count - 32) * -1] = -1;

                shipInfo[count].SetCurrentPosition(tilemap.WorldToCell(transform.position));
                shipInfo[count].SetPreviousPosition(tilemap.WorldToCell(transform.position));

                count++;
           // }
        }
    }

    void SetTreasureShips()
    {
        GameObject ship = Instantiate(treasureShips[0]);

        ship.SetActive(true);

        ship.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(17, -24, 0));
        ship.transform.position = ship.transform.position + (Vector3.up / 2);

        objectsInMap[17 + 34, (-24 - 32) * -1] = 1;
    }
    void SetMonsters()
    {
        GameObject monsterT = Instantiate(monsters[0]);

        monsterT.SetActive(true);

        monsterT.GetComponent<Monstermovement>().enabled = false;

        monsterT.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(19, -24, 0));
        monsterT.transform.position = monsterT.transform.position + Vector3.up;

        objectsInMap[19 + 34, (-24 - 32) * -1] = 2;
    }

    public void LoadMap()
    {
        int space, tile;
        string map = PlayerPrefs.GetString("Map");

        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                space = map.IndexOf(' ');

                tile = int.Parse(map.Substring(0, space));

                map = map.Remove(0, (map.Substring(0, space).Length) + 1);

                tilesInMap[i, j] = tile;

                if (tile != 0)
                {
                    SetObject(tile, i, j);
                }
            }

            map = map.Remove(0, 1);
        }

    }

    public void SetObject(int index, int row, int column)
    {
        GameObject newObject = new GameObject();
        SpriteRenderer sr = newObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        newObject.transform.parent = objectContainers[0].transform;

        sr.sprite = mapObjects[index];

        Vector3 pos = tilemap.GetCellCenterWorld(new Vector3Int(-34 + row, 32 - column, 0));

        pos = new Vector3(pos.x, pos.y + 0.75f, pos.z);

        newObject.transform.localScale = new Vector3(6, 6, 1);
        newObject.transform.rotation = Quaternion.Euler(90, 0, 0);
        newObject.transform.position = pos;
    }

    public void MoveShip(GameObject ship)
    {
        if (posSet)
        {
            SetGUI(false, diceGUI);

            hiddenBtns[0].onClick.Invoke();

            ClearActiveTiles();

            if (rotate)
            {
                isMoving = true;

                float currDeg = degrees + accel;

                Transform shipTransfom = ship.transform.Find("ship");

                Vector3 direction = (currPos - new Vector3(shipTransfom.position.x, 0, shipTransfom.position.z)).normalized;

                Quaternion lookRotation = Quaternion.LookRotation(direction);

                shipTransfom.rotation = Quaternion.Slerp(shipTransfom.rotation, lookRotation, Time.deltaTime * currDeg);

                accel += 0.01f;

                movingFX.transform.rotation = shipTransfom.rotation;

                movingFX.transform.position = new Vector3(ship.transform.position.x, 0.55f, ship.transform.position.z);

                movingFX.SetActive(true);

                if (Quaternion.Angle(shipTransfom.rotation, lookRotation) < 2f)
                {
                    rotate = false;

                    movingFX.SetActive(false);

                    accel = 0;
                }

            }
            else
            {
                ship.transform.position = Vector3.MoveTowards(ship.transform.position, currPos, Time.deltaTime * time);

                movingFX.transform.position = new Vector3(ship.transform.position.x, 0.6f, ship.transform.position.z);

                movingFX.SetActive(true);

                if (Vector3.Distance(ship.transform.position, currPos) < 0.001f)
                {
                    SetGUI(true, diceGUI);

                    movingFX.SetActive(false);

                    posSet = false;

                    isMoving = false;

                    hiddenBtns[1].onClick.Invoke();

                    PlayerPrefs.SetString("Ship1", "PLAYER " + shipInfo[playerIndex].GetPlayerNum().ToString());

                    ResultsManager.players[0] = shipInfo[playerIndex];

                    if (port)
                    {
                        SetGUI(false, diceGUI);

                        tiles.gameObject.SetActive(false);

                        if (currArrow)
                        {
                            currArrow.SetActive(false);
                        }

                        if (tilemap.WorldToCell(currPos).Equals(shipInfo[playerIndex].GetPortPosition()))
                        {
                            portGUI[0].SetActive(true);
                        }
                        else
                        {
                            portGUI[1].SetActive(true);
                        }
                    }
                    else if (playerCombat)
                    {
                        CannonMinigame.setPlayer = true;
                        CannonMinigame.currShip = 1;
                        CannonMinigame.shipsInfo[0] = shipInfo[playerIndex];
                        CannonMinigame.shipsInfo[1] = GetEnemeyShip();
                        CannonMinigame.SetShips();

                        PlayerPrefs.SetString("Enemy", "Player");

                        RenderSettings.skybox = skyBox[5];

                        hiddenBtns[2].onClick.Invoke();
                    }
                    else if (shipCombat)
                    {
                        CannonMinigame.setTreasure = true;
                        CannonMinigame.shipsInfo[0] = shipInfo[playerIndex];

                        PlayerPrefs.SetString("Enemy", "Treasure");

                        RenderSettings.skybox = skyBox[5];

                        hiddenBtns[2].onClick.Invoke();
                    }
                    else
                    {
                        if (monsterCombat)
                        {
                            CannonMinigame.setMonster = true;
                            CannonMinigame.SetMonster(monsters[0]);
                            CannonMinigame.shipsInfo[0] = shipInfo[playerIndex];

                            PlayerPrefs.SetString("Enemy", "Monster");

                            RenderSettings.skybox = skyBox[2];

                            hiddenBtns[2].onClick.Invoke();
                        }
                    }
                }
            }
        }
        else
        {
            ProcessShip(ship);
        }
    }

    PlayerShip GetEnemeyShip()
    {
        if (playerCombat)
        {
            foreach (PlayerShip player in shipInfo)
            {
                if (player.GetCurrentPosition() == shipInfo[playerIndex].GetCurrentPosition() && player.GetName() != shipInfo[playerIndex].GetName())
                {
                    PlayerPrefs.SetString("Ship2", "PLAYER " + player.GetPlayerNum().ToString());

                    ResultsManager.players[1] = player;

                    return player;
                }
            }
        }

        return null;
    }

    public void ClearActiveTiles()
    {
        Transform transform = tiles.transform;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        positions.Clear();
        playerPos.Clear();
        portPos.Clear();
        shipPos.Clear();
        monsterPos.Clear();
        validPos.Clear();
        Destroy(currArrow);
    }

    public void EndTurn()
    {
        ChangePlayer();

        UpdateTurn();
    }

    public void UpdateTurn()
    {
        ClearActiveTiles();

        SetGUI(false, diceGUI);
    }

    public void ProcessShip(GameObject ship)
    {
        GetMoves();

        if (Input.GetMouseButtonDown(0) && clickable && !ButtonNotClicked())
        {
            Ray ray = main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Vector3Int prevGridPos = tilemap.WorldToCell(ship.transform.position);
                Vector3Int gridPos = tilemap.WorldToCell(raycastHit.point);

                if (positions.Contains(gridPos) && validPos.Contains(gridPos))
                {
                    Vector3 pos = tilemap.GetCellCenterWorld(gridPos);

                    shipInfo[playerIndex].SetPreviousPosition(prevGridPos);
                    shipInfo[playerIndex].SetCurrentPosition(gridPos);

                    if (gridPos.x >= leftBound && gridPos.x <= rightBound && gridPos.y <= upperBound && gridPos.y >= lowerBound)
                    {
                        currPos = pos;
                        posSet = true;
                        rotate = true;
                        diceSet = true;

                        if (playerPos.Contains(gridPos))
                        {
                            objectsInMap[prevGridPos.x + 34, (prevGridPos.y - 32) * -1] += 1;
                            objectsInMap[gridPos.x + 34, (gridPos.y - 32) * -1] -= 1;

                            playerCombat = true;
                            clickable = false;
                        }
                        else
                        {
                            objectsInMap[prevGridPos.x + 34, (prevGridPos.y - 32) * -1] += 1;
                            objectsInMap[gridPos.x + 34, (gridPos.y - 32) * -1] -= 1;
                        }

                        if (portPos.Contains(gridPos))
                        {
                            port = true;
                            clickable = false;
                        }

                        if (shipPos.Contains(gridPos))
                        {
                            objectsInMap[gridPos.x + 34, (gridPos.y - 32) * -1] = -1;

                            shipCombat = true;
                            clickable = false;
                        }

                        if (monsterPos.Contains(gridPos))
                        {
                            objectsInMap[gridPos.x + 34, (gridPos.y - 32) * -1] = -1;

                            monsterCombat = true;
                            clickable = false;
                        }

                        UpdateNavigationMenu();
                    }
                }
            }
        }
    }

    bool ButtonNotClicked()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            foreach (GameObject comp in mainGUI)
            {
                if (EventSystem.current.currentSelectedGameObject.Equals(comp))
                {
                    return true;
                }
            }

            foreach (GameObject cameraObj in cameraGUI)
            {
                for (int i = 0; i < cameraObj.transform.childCount; i++)
                {
                    if (EventSystem.current.currentSelectedGameObject.Equals(cameraObj.transform.GetChild(i)));
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void GetMoves()
    {
        if (diceSet)
        {
            diceSet = false;

            if (diceVals[diceIndex] < 0)
            {
                diceIndex++;

                diceSet = true;
            }
            else
            {
                if (diceIndex == 0 && positions.Count == 0 && !isMoving && !posSet)
                {
                    DisplayMoves(diceVals[diceIndex]);

                    diceIndex++;
                }

                if (diceIndex == 1 && positions.Count == 0 && !isMoving && !posSet)
                {
                    if (moveCount < diceVals[diceIndex])
                    {
                        moveCount++;

                        DisplayMoves(1);
                    }
                    else
                    {
                        diceIndex++;

                        moveCount = 0;
                    }
                }

                if (diceIndex == 2 && positions.Count == 0 && !isMoving && !posSet)
                {
                    if (moveCount < diceVals[diceIndex] && diceVals[diceIndex] != 1)
                    {
                        moveCount++;

                        DisplayMoves(-1);
                    }
                    else
                    {
                        diceIndex++;

                        moveCount = 0;
                    }

                    Debug.Log("Wind Moves: " + diceVals[diceIndex]);
                }
            }
        }
    }

    void UpdateNavigationMenu()
    {
        if (diceIndex == 1)
        {
            navTexts[2].text = 0.ToString();

            if (diceVals[diceIndex] - moveCount >= 0)
            {
                navTexts[0].text = (diceVals[diceIndex] - moveCount).ToString();
            }
        }
        else if (diceIndex == 2 && diceVals[diceIndex] != 1)
        {
            navTexts[0].text = 0.ToString();

            if (diceVals[diceIndex] - moveCount >= 0)
            {
                navTexts[1].text = (diceVals[diceIndex] - moveCount).ToString();
            }
        }
        else if (diceIndex == 0 && diceVals[diceIndex] != 1)
        {
            navTexts[1].text = 0.ToString();
        }
    }

    void DisplayMoves(int count)
    {
        Vector3Int playerPos = tilemap.WorldToCell(players[playerIndex].transform.position);

        playerPos = new Vector3Int(playerPos.x, playerPos.y, 0);

        if (count == -1)
        {
            if (moveCount == 1)
            {
                windDirection = RandomDirection();
            }

            SetSelected(playerPos + windDirection);
            SetArrow(playerPos + windDirection);
        }
        else
        {
            for (int i = 1; i <= count; i++)
            {
                SetSelected(playerPos + new Vector3Int(i, 0, 0));
                SetSelected(playerPos + new Vector3Int(-i, 0, 0));
                SetSelected(playerPos + new Vector3Int(0, i, 0));
                SetSelected(playerPos + new Vector3Int(0, -i, 0));
                SetSelected(playerPos + new Vector3Int(i, i, 0));
                SetSelected(playerPos + new Vector3Int(-i, -i, 0));
                SetSelected(playerPos + new Vector3Int(-i, i, 0));
                SetSelected(playerPos + new Vector3Int(i, -i, 0));
            }
        }
    }

    void SetSelected(Vector3Int pos)
    {
        int tilePlaced = tilesInMap[pos.x + 34, (pos.y - 32) * -1];
        int objectPlaced = objectsInMap[pos.x + 34, (pos.y - 32) * -1];

        GameObject temp;
        Transform transform;

        if (tilePlaced < 0)
        {
            temp = Instantiate(positionTiles[2], tiles.transform);
        }
        else if (objectPlaced < 0 || objectPlaced > 0)
        {
            temp = Instantiate(positionTiles[1], tiles.transform);
        }
        else
        {
            temp = Instantiate(positionTiles[0], tiles.transform);
        }

        if ((tilePlaced <= 1 && tilePlaced > -1) || (objectPlaced < 0 && objectPlaced > -2 && tilePlaced > -1) || (objectPlaced == 0 && tilePlaced == -1))
        {
            validPos.Add(pos);

            if (tilePlaced == -1)
            {
                portPos.Add(pos);
            }
            else if (objectPlaced == -1)
            {
                playerPos.Add(pos);
            }
            else if (objectPlaced == 1)
            {
                shipPos.Add(pos);
            }
            else
            {
                if (objectPlaced == 2)
                {
                    monsterPos.Add(pos);
                }
            }
        }

        transform = temp.transform;

        transform.position = tilemap.GetCellCenterWorld(pos);

        transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);

        positions.Add(pos);
    }

    void SetArrow(Vector3Int pos)
    {
        GameObject temp = Instantiate(arrow);

        temp.transform.position = tilemap.GetCellCenterWorld(pos);
        temp.transform.position = new Vector3(temp.transform.position.x + 5.5f, temp.transform.position.y + 4, temp.transform.position.z - 4);
        temp.SetActive(true);

        currArrow = temp;

        RotateArrow();
    }

    void RotateArrow()
    {
        currArrow.transform.RotateAround(currArrow.transform.position + new Vector3(-5.5f, 0, 4), Vector3.up, arrowRot);
    }

    public void ToggleMovement()
    {
        if (clickable)
        {
            buttons.enabled = true;

            clickable = false;

            toggleText.text = "Move by clicking";
        }
        else
        {
            buttons.enabled = false;

            clickable = true;

            toggleText.text = "Move through buttons";
        }
    }

    public void ChangePlayer()
    {
        if (playerIndex + 1 >= maxPlayers && !isMoving)
        {
            playerIndex = 0;
            playerNum = playerNums[playerIndex] - 1;
            camNum = cams[0];

        }
        else
        {
            if (!isMoving)
            {
                playerIndex++;
                playerNum = playerNums[playerIndex] - 1;
                camNum = cams[playerNum - 1];
            }
        }

        treasureCurrent.text = "Player Treasure: " + shipInfo[playerIndex].GetCurrentTreasure().ToString();
        treasureTotal.text = "Total Treasure: " + shipInfo[playerIndex].GetTotalTreasure().ToString();
        player.text = "Player: " + (playerIndex + 1).ToString();

        navMenu.SetActive(false);
        mainGUI[1].SetActive(true);

        Debug.Log("Active Camera: " + (camNum + 1));
    }

    void SetGUI(bool enable, GameObject[] GUI)
    {
        if (enable)
        {
            foreach (GameObject comp in GUI)
            {
                if (comp.transform.name != "Roll")
                {
                    comp.SetActive(true);
                }
            }
        }
        else
        {
            foreach (GameObject comp in GUI)
            {
                if (comp.transform.name != "Roll")
                {
                    comp.SetActive(false);
                }
            }
        }
    }

    Vector3Int RandomDirection()
    {
        int num = Random.Range(0, 8);

        switch (num)
        {
            case 0:
                arrowRot = 90f;
                return new Vector3Int(1, 0, 0);
            case 1:
                arrowRot = -90f;
                return new Vector3Int(-1, 0, 0);
            case 3:
                arrowRot = 0f;
                return new Vector3Int(0, 1, 0);
            case 4:
                arrowRot = 180f;
                return new Vector3Int(0, -1, 0);
            case 5:
                arrowRot = 45f;
                return new Vector3Int(1, 1, 0);
            case 6:
                arrowRot = -135f;
                return new Vector3Int(-1, -1, 0);
            case 7:
                arrowRot = -45;
                return new Vector3Int(-1, 1, 0);
            default:
                arrowRot = 135f;
                return new Vector3Int(1, -1, 0);
        }
    }

    //Movement Coroutine for Arrow Key movement
    private IEnumerator MovePlayer(GameObject player, Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        original = player.transform.position;

        target = original + direction;

        player.transform.forward = direction;

        while (elapsedTime < moveTime)
        {
            player.transform.position = Vector3.Lerp(original, target, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = target;

        isMoving = false;
    }

    public void ResetRoll()
    {
        diceIndex = 0;
        diceSet = false;

        ClearActiveTiles();

        mainGUI[1].SetActive(false);
    }

    public void PauseGameScene()
    {
        SetGUI(false, diceGUI);
        main.enabled = false;
        tiles.gameObject.SetActive(false);
    }

    public static void ContinueGame()
    {
        continueGame = true;
    }
}
