using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapLoad : MonoBehaviour
{
    public static bool[] activePlayers;

    public static HashSet<int> portIndices = new HashSet<int>();
    public static List<int> playersHit = new List<int>();
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
    public Text treasureLimit;
    public Text treasureCurrent;
    public Text player;
    public Text ratsText;

    public Camera main;
    public GameObject[] mainGUI;
    public GameObject[] diceGUI;
    public GameObject[] cameraGUI;
    public GameObject[] portGUI;
    public Button[] skipButtons;
    public GameObject[] displays;

    public GameObject[] ships;
    public Sprite[] mapObjects;
    public Canvas[] objectContainers;
    public GameObject[] positionTiles;
    public GameObject[] gameTiles;
    public Canvas tiles;
    public Button[] hiddenBtns;
    public Material[] skyBox;
    public List<Sprite> portImages;
    public List<GameObject> ports;
    public List<GameObject> treasureShips;
    public List<GameObject> monsters;
    public GameObject diceSelector;
    public GameObject sirenObj;
    public GameObject arrow;
    public GameObject movingFX;
    public GameObject navMenu;
    public GameObject pauseMenu;
    public Text[] navTexts;
    public Text[] sextantTexts;
    public Text[] portTexts;
    public Text[] moveTexts;
    public Image playerImg;
    public Image[] moveSelection;
    public Text moveDisplay;
    public ParticleSystem fogFX;

    public float winAmount;
    public float time;
    public float degrees;
    
    float accel;

    int rounds = 1;
    int shipRounds = 1;
    int origIndex;

    int leftBound;
    int rightBound;
    int upperBound;
    int lowerBound;

    Tilemap tilemap;

    HashSet<Vector3Int> positions = new HashSet<Vector3Int>();
    HashSet<Vector3Int> validPos = new HashSet<Vector3Int>();
    HashSet<Vector3Int> portPos = new HashSet<Vector3Int>();
    HashSet<Vector3Int> playerPos = new HashSet<Vector3Int>();
    HashSet<Vector3Int> shipPos = new HashSet<Vector3Int>();
    HashSet<Vector3Int> monsterPos = new HashSet<Vector3Int>();
    HashSet<int> fogTilesX = new HashSet<int>();
    HashSet<int> fogTilesY = new HashSet<int>();

    GameObject currArrow;
    Vector3Int windDirection;

    GameObject[] players;
    public static List<PlayerShip> shipInfo;
    List<int> playerNums;

    public static int reward = -1;
    public static int hazard = -1;
    public static int ghostCount = 0;
    public static bool diceSet = false;
    public static int playerNum;
    public static int[] diceVals;
    public static bool rats = false;
    public static bool fog = false;
    public static bool siren = false;
    public int playerIndex;
    public static int diceIndex;
    public int moveCount;
    public int baseMove = 5;

    public static int maxPlayers;
    public static bool monsterFound = false;
    static bool continueGame = false;
    float fogTime;
    float arrowRot;
    bool posSet = false;
    bool rotate = false;
    bool port = false;
    bool playerCombat = false;
    bool shipCombat = false;
    bool monsterCombat = false;
    bool movingSkip = false;
    bool eventTriggered = false;
    Vector3 currPos;

    int[,] tilesInMap;
    int[,] objectsInMap;

    RandomPosition objectGenerator;

    public int[] cams;
    public static int camNum;

    public static int maxCams;

    void Start()
    {
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
        winAmount = PlayerPrefs.GetFloat("End");

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
            string[] lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/Player" + playerNums[i].ToString() + ".txt");

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
            else
            {
                portIndices.Add(i);
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

        //SetPlayerShips();
        //SetTreasureShips();
        //SetMonsters();

        MonsterAI.monsterContainer = objectContainers[3];
        MonsterAI.players = shipInfo;

        maxCams = maxPlayers;

        objectGenerator = new RandomPosition(shipInfo, ports, treasureShips, monsters, sirenObj, objectContainers);
        objectGenerator.SetTilemap(tilemap);
        objectGenerator.SetMapObjects(tilesInMap, objectsInMap);
        objectGenerator.GeneratePlayerPosition(tilesInMap, objectsInMap);
        objectGenerator.GenerateHomePortPositions(tilesInMap, objectsInMap);
        objectGenerator.GeneratePortPositions(tilesInMap, objectsInMap);
        objectGenerator.GenerateSirenPosition(tilesInMap, objectsInMap);
    }

    void Update()
    {
        mainGUI[10].GetComponent<Text>().text = "Round: " + rounds.ToString();
        MoveShip(players[playerIndex]);

        if (!isMoving && !isRolling && !playerCombat && !shipCombat && !monsterCombat && !port && !eventTriggered)
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
            UnPauseMusic();

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
        treasureLimit.text = "Carry Limit: " + shipInfo[playerIndex].GetTreasureLimit().ToString();
        player.text = "Player: " + (playerIndex + 1).ToString();
        ratsText.text = "RATS ON SHIP: " + shipInfo[playerIndex].GetRats().ToString();
        playerImg.sprite = portImages[playerIndex];
        UpdateSextant(); 
        
        if (shipInfo[playerIndex].GetRats() >= 10)
        {
            ratsText.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            ratsText.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (reward > -1)
        {
            if (reward == playerNums[playerIndex])
            {
                moveTexts[2].gameObject.SetActive(true);

                PlayerPrefs.SetInt("Double", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Double", 0);
            }

            reward = -1;
        }

        if (hazard > -1)
        {
            monsterFound = FindMonster();
        }

        if (rats)
        {
            shipInfo[playerIndex].AddRat();

            rats = false;
        }

        if (fog)
        {
            SetFog();

            fog = false;
        }

        if (ghostCount >= 3 && !eventTriggered && !isRolling)
        {
            ghostCount = 0;

            StartGhostShip();
        }

        if (siren)
        {
            for (int i = 0; i < objectContainers[4].transform.childCount; i++)
            {
                objectContainers[4].transform.GetChild(i).GetComponent<Siren>().CheckPlayersHit();
            }

            siren = false;
        }

        if (playersHit.Contains(playerIndex) && !eventTriggered && !isRolling) 
        {
            playersHit.Remove(playerIndex);

            StartSirenHit();
        }
    }

    private IEnumerator ChangeSky()
    {
        foreach (var sky in skyBox)
        {
            yield return new WaitForSeconds(15);

            RenderSettings.skybox = sky;
        }
    }

    void SetFog()
    {
        Vector3Int playerPos = shipInfo[playerIndex].GetCurrentPosition();
        int tileX = Mathf.FloorToInt(((playerPos.x + 34) / 16f));
        int tileY = Mathf.FloorToInt((((playerPos.y - 32) * -1) / 16f));

        if (!fogTilesX.Contains(tileX) || !fogTilesY.Contains(tileY))
        {
            ParticleSystem fog = Instantiate(fogFX, objectContainers[5].transform);
            fog.transform.position = tilemap.CellToWorld(new Vector3Int(-34 + (16 * tileX) + 8, 32 - (16 * tileY) - 15, 0));
            fog.transform.position += new Vector3(0, 12, 0);
            fog.gameObject.SetActive(true);
            fog.Play();

            fogTilesX.Add(tileX);
            fogTilesY.Add(tileY);

            StartCoroutine(StartFog());
        }
    }

    IEnumerator StartFog()
    {
        yield return new WaitUntil(() => !isRolling && !eventTriggered);
        displays[3].SetActive(true);

        StartCoroutine(WaitOnFog());

        yield return new WaitUntil(() => eventTriggered || fogTime > 5 || isMoving);

        displays[3].SetActive(false);
        fogTime = 0;
    }

    IEnumerator WaitOnFog()
    {
        while(fogTime < 6)
        {
            yield return new WaitForSeconds(1);
            fogTime++;
        }
    }

    void RemoveFog()
    {
        foreach (Transform fog in objectContainers[5].transform)
        {
            Destroy(fog.gameObject);

            fogTilesX.Clear();
            fogTilesY.Clear();
        }
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

                if (tile == 1)
                {
                    SetObjectTile(0, i, j);
                }
                else if (tile != 0)
                {
                    SetTile(tile, i, j);
                }
            }

            map = map.Remove(0, 1);
        }

        PlayerPrefs.SetString("Map2", map);
    }

    public void SetTile(int index, int row, int column)
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

    public void SetObjectTile(int index, int row, int column)
    {
        GameObject newObject = Instantiate(gameTiles[index]);

        newObject.transform.parent = objectContainers[0].transform;

        newObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(-34 + row, 32 - column, 0));

        newObject.transform.position = newObject.transform.position + new Vector3(0, 0.2f, 0);
    }

    public void MoveShip(GameObject ship)
    {
        if (posSet)
        {
            SetGUI(false, diceGUI);

            hiddenBtns[0].onClick.Invoke();

            ClearActiveTiles();

            if (rotate && !movingSkip)
            {
                isMoving = true;

                float currDeg = degrees + accel;

                Transform shipTransform = ship.transform.Find("ship");

                Vector3 direction = (currPos - new Vector3(shipTransform.position.x, 0, shipTransform.position.z)).normalized;

                Quaternion lookRotation = Quaternion.LookRotation(direction);

                if (Quaternion.Angle(shipTransform.rotation, lookRotation) < 2f)
                {
                    rotate = false;

                    movingFX.SetActive(false);

                    accel = 0;
                }
                else
                {
                    shipTransform.rotation = Quaternion.Slerp(shipTransform.rotation, lookRotation, Time.deltaTime * currDeg);

                    accel += 0.01f;

                    movingFX.transform.rotation = shipTransform.rotation;

                    movingFX.transform.position = new Vector3(ship.transform.position.x, 0.55f, ship.transform.position.z);

                    movingFX.SetActive(true);
                }
            }
            else
            {
                if (Vector3.Distance(ship.transform.position, currPos) < 0.01f || movingSkip)
                {
                    if (rotate)
                    {
                        rotate = false;

                        movingFX.SetActive(false);

                        accel = 0;
                    }

                    SetGUI(true, diceGUI);

                    movingFX.SetActive(false);
                    skipButtons[0].gameObject.SetActive(false);

                    posSet = false;

                    isMoving = false;
                    movingSkip = false;

                    hiddenBtns[1].onClick.Invoke();

                    PlayerPrefs.SetString("Ship1", "PLAYER " + shipInfo[playerIndex].GetPlayerNum().ToString());

                    ResultsManager.players[0] = shipInfo[playerIndex];

                    if (port)
                    {
                        SetGUI(false, diceGUI);
                        diceSelector.SetActive(false);
                        portTexts[0].gameObject.SetActive(false);
                        portTexts[1].gameObject.SetActive(false);
                        portTexts[2].gameObject.SetActive(false);

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
                        diceSelector.SetActive(false);
                        CannonMinigame.setPlayer = true;
                        CannonMinigame.currShip = 1;
                        CannonMinigame.shipsInfo[0] = shipInfo[playerIndex];
                        CannonMinigame.shipsInfo[1] = GetEnemeyShip();
                        CannonMinigame.SetShips();

                        PlayerPrefs.SetString("Enemy", "Player");

                        RenderSettings.skybox = skyBox[5];

                        PauseMusic();
                        hiddenBtns[2].onClick.Invoke();
                    }
                    else if (shipCombat)
                    {
                        diceSelector.SetActive(false);
                        CannonMinigame.setTreasure = true;
                        CannonMinigame.shipsInfo[0] = shipInfo[playerIndex];
                        CannonMinigame.SetTreasureShip(GetTreasureShip());

                        PlayerPrefs.SetString("Enemy", "Treasure");

                        RenderSettings.skybox = skyBox[5];

                        PauseMusic();
                        hiddenBtns[2].onClick.Invoke();
                    }
                }
                else
                {
                    ship.transform.position = Vector3.MoveTowards(ship.transform.position, currPos, Time.deltaTime * time);

                    movingFX.transform.position = new Vector3(ship.transform.position.x, 0.6f, ship.transform.position.z);

                    movingFX.SetActive(true);
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

    GameObject GetTreasureShip()
    {
        Vector2Int playerPos = (Vector2Int)tilemap.WorldToCell(shipInfo[playerIndex].GetShip().transform.position);

        for (int i = 0; i < objectContainers[2].transform.childCount; i++)
        {
            Vector2Int shipPos = (Vector2Int)tilemap.WorldToCell(objectContainers[2].transform.GetChild(i).position);

            if (shipPos == playerPos)
            {
                return objectContainers[2].transform.GetChild(i).gameObject;
            }
        }

        return null;
    }

    GameObject GetMonster()
    {
        Vector2Int playerPos = (Vector2Int)tilemap.WorldToCell(shipInfo[playerIndex].GetShip().transform.position);

        for (int i = 0; i < objectContainers[3].transform.childCount; i++)
        {
            Vector2Int monsterPos = (Vector2Int)tilemap.WorldToCell(objectContainers[3].transform.GetChild(i).position);

            if (monsterPos == playerPos)
            {
                return objectContainers[3].transform.GetChild(i).gameObject;
            }
        }

        return null;
    }

    GameObject GetMonsterFromDice()
    {
        int index = origIndex - 12;

        return monsters[index];
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
        if (!monsterFound)
        {
            CheckPlayer();

            ChangePlayer();

            UpdateTurn();

            main.GetComponent<CameraController>().UpdateDummy();
        }
        else
        {
            StartMonsterCombat(1);
        }
    }

    public void UpdateTurn()
    {
        ClearActiveTiles();

        SetGUI(false, diceGUI);
        navMenu.SetActive(false);
        ratsText.transform.GetChild(0).gameObject.SetActive(false);
        moveTexts[2].gameObject.SetActive(false);
        moveTexts[3].gameObject.SetActive(false);
        moveTexts[4].gameObject.SetActive(false);
        displays[3].SetActive(false);

        if (!monsterCombat)
        {
            mainGUI[1].SetActive(true);
        }
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

                        skipButtons[0].gameObject.SetActive(true);
                        main.GetComponent<CameraController>().ResetDummy();

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

                        CheckPreviousPosition();

                        UpdateNavigationMenu();
                    }
                }
            }
        }
    }

    void CheckPreviousPosition()
    {
        Vector3Int prevGridPos = shipInfo[playerIndex].GetPreviousPosition();

        if (objectsInMap[prevGridPos.x + 34, (prevGridPos.y - 32) * -1] != -1)
        {
            objectsInMap[prevGridPos.x + 34, (prevGridPos.y - 32) * -1] = 0;
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

            RectTransform diceSelect = moveSelection[0].rectTransform;
            RectTransform navSelect = moveSelection[1].rectTransform;

            if (diceVals[diceIndex] < 0)
            {
                diceSelect.gameObject.SetActive(false);
                navSelect.gameObject.SetActive(false);
                moveDisplay.text = "NO MOVES";

                diceIndex++;

                diceSet = true;
            }
            else
            {
                if (diceIndex == 0 && positions.Count == 0 && !isMoving && !posSet)
                {
                    diceSelect.localPosition = new Vector3(diceSelect.localPosition.x, 209, diceSelect.localPosition.z);
                    navSelect.localPosition = new Vector3(navSelect.localPosition.x, -31, navSelect.localPosition.z);
                    diceSelect.gameObject.SetActive(true);
                    navSelect.gameObject.SetActive(true);
                    moveDisplay.text = "COMPASS";

                    if (shipInfo[playerIndex].GetCompass() > 0)
                    {
                        diceVals[diceIndex] += shipInfo[playerIndex].GetCompass();
                        moveTexts[0].text = "+ " + shipInfo[playerIndex].GetCompass().ToString();
                    }

                    DisplayMoves(diceVals[diceIndex]);

                    diceIndex++;
                }

                if (diceIndex == 1 && positions.Count == 0 && !isMoving && !posSet)
                {
                    if ((shipInfo[playerIndex].GetBase() > 0 || baseMove > 0) && moveCount == 0)
                    {
                        diceVals[diceIndex] += shipInfo[playerIndex].GetBase() + baseMove;
                        moveTexts[1].text = "+ " + (shipInfo[playerIndex].GetBase() + baseMove).ToString();
                    }

                    if (moveCount < diceVals[diceIndex])
                    {
                        diceSelect.localPosition = new Vector3(diceSelect.localPosition.x, 134, diceSelect.localPosition.z);
                        navSelect.localPosition = new Vector3(navSelect.localPosition.x, 30, navSelect.localPosition.z);
                        diceSelect.gameObject.SetActive(true);
                        navSelect.gameObject.SetActive(true);
                        moveDisplay.text = "NAVIGATE";

                        UpdateNavigationMenu();

                        moveCount++;

                        DisplayMoves(1);
                    }
                    else
                    {
                        diceSelect.gameObject.SetActive(false);
                        navSelect.gameObject.SetActive(false);
                        moveDisplay.text = "NO MOVES";

                        diceIndex++;

                        moveCount = 0;
                    }
                }

                if (diceIndex == 2 && positions.Count == 0 && !isMoving && !posSet)
                {
                    if (moveCount < diceVals[diceIndex] && diceVals[diceIndex] != 1)
                    {
                        diceSelect.localPosition = new Vector3(diceSelect.localPosition.x, 59, diceSelect.localPosition.z);
                        navSelect.localPosition = new Vector3(navSelect.localPosition.x, -1, navSelect.localPosition.z);
                        diceSelect.gameObject.SetActive(true);
                        navSelect.gameObject.SetActive(true);
                        moveDisplay.text = "WIND";

                        moveCount++;

                        DisplayMoves(-1);
                    }
                    else
                    {
                        diceSelect.gameObject.SetActive(false);
                        navSelect.gameObject.SetActive(false);
                        moveDisplay.text = "NO MOVES";

                        diceIndex++;

                        moveCount = 0;
                    }
                }
            }

            if (port || playerCombat || shipCombat || monsterCombat || eventTriggered)
            {
                diceSelect.gameObject.SetActive(false);
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

    void UpdateSextant()
    {
        Vector3Int ship = shipInfo[playerIndex].GetCurrentPosition();
        Vector3Int port = shipInfo[playerIndex].GetPortPosition();

        sextantTexts[0].text = (ship.x + 34).ToString();
        sextantTexts[1].text = ((ship.y - 32) * -1).ToString();
        sextantTexts[2].text = (port.x + 34).ToString();
        sextantTexts[3].text = ((port.y - 32) * -1).ToString();
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
                moveTexts[4].gameObject.SetActive(true);
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
        if ((pos.x + 34 < 80) && ((pos.y - 32) * -1) < 80 &&
            (pos.x + 34 >= 0) && ((pos.y - 32) * -1) >= 0) {
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

            if ((tilePlaced <= 2 && tilePlaced > -1) || (objectPlaced < 0 && objectPlaced > -2 && tilePlaced > -1) || (objectPlaced == 0 && tilePlaced == -1))
            {
                if ((shipInfo[playerIndex].GetRats() < 10 || tilePlaced == -1) && objectPlaced < 2) {
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
                }
            }

            transform = temp.transform;

            transform.position = tilemap.GetCellCenterWorld(pos);

            transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);

            positions.Add(pos);
        }
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

    public void CheckPlayer()
    {
        CheckForWin();
    }

    public void ChangePlayer()
    {
        shipInfo[playerIndex].ResetCrew();
        CheckPreviousPosition();

        if (playerIndex + 1 >= maxPlayers && !isMoving)
        {
            playerIndex = 0;
            playerNum = playerNums[playerIndex] - 1;
            camNum = cams[0];

            rounds++;
            shipRounds++;

            if (shipRounds >= 5)
            {
                shipRounds = 1;

                AddShips();
            }

            ShipMovement();
            MonsterMovement();
            RemoveFog();
            fogTilesX.Clear();
            fogTilesY.Clear();

            if (shipInfo[playerIndex].HasMonsterCombat() && !monsterCombat)
            {
                StartMonsterCombat(0);
            }
        }
        else
        {
            if (!isMoving)
            {
                playerIndex++;
                playerNum = playerNums[playerIndex] - 1;
                camNum = cams[playerNum - 1];

                if (shipInfo[playerIndex].HasMonsterCombat() && !monsterCombat)
                {
                    StartMonsterCombat(0);
                }
            }
        }

        ghostCount = 0;
        CameraController.index = playerIndex;
    }

    void StartMonsterCombat(int type)
    {
        monsterCombat = true;
        clickable = false;
        SetGUI(false, mainGUI);
        mainGUI[1].SetActive(false);
        displays[0].SetActive(true);
        tiles.gameObject.SetActive(false);
        diceSelector.SetActive(false);
        CannonMinigame.setMonster = true;
        ResultsManager.players[0] = shipInfo[playerIndex];
        CannonMinigame.shipsInfo[0] = shipInfo[playerIndex];

        if (type == 0)
        {

            CannonMinigame.SetMonster(GetMonster());
            StartCoroutine(WaitForCombat(type));
        }
        else
        {
            SetGUI(false, diceGUI);
            CannonMinigame.SetMonster(GetMonsterFromDice());
            StartCoroutine(WaitForCombat(type));
        }

        PlayerPrefs.SetString("Enemy", "Monster");

        RenderSettings.skybox = skyBox[2];
    }

    IEnumerator WaitForCombat(int type)
    {
        yield return new WaitForSeconds(5);

        PauseMusic();
        hiddenBtns[2].onClick.Invoke();
        displays[0].SetActive(false);

        if (type == 0)
        {
            yield return new WaitUntil(() => !shipInfo[playerIndex].HasMonsterCombat());
        }
        else
        {
            yield return new WaitUntil(() => !monsterFound);
        }

        main.enabled = true;
        monsterCombat = false;
        clickable = true;
        tiles.gameObject.SetActive(true);
        RenderSettings.skybox = skyBox[0];
        CannonMinigame.DestroyObjects();
        UnPauseMusic();

        if (type == 1)
        {
            EndTurn();
        }
        else
        {
            mainGUI[1].SetActive(true);
        }
    }

    void StartSirenHit()
    {
        clickable = false;
        mainGUI[1].SetActive(false);
        tiles.gameObject.SetActive(false);
        eventTriggered = true;
        SetGUI(false, diceGUI);
        displays[1].SetActive(true);
        diceSelector.SetActive(false);

        StartCoroutine(WaitForSirenHit());
    }

    IEnumerator WaitForSirenHit()
    {
        yield return new WaitForSeconds(6);

        clickable = true;
        mainGUI[1].SetActive(true);
        tiles.gameObject.SetActive(true);
        displays[1].SetActive(false);
        eventTriggered = false;
        SetGUI(true, diceGUI);

        EndTurn();
    }

    void StartGhostShip()
    {
        clickable = false;
        mainGUI[1].SetActive(false);
        tiles.gameObject.SetActive(false);
        eventTriggered = true;
        SetGUI(false, diceGUI);
        displays[2].SetActive(true);
        diceSelector.SetActive(false);

        StartCoroutine(WaitForGhostShip());
    }

    IEnumerator WaitForGhostShip()
    {
        yield return new WaitForSeconds(6);

        clickable = true;
        mainGUI[1].SetActive(true);
        tiles.gameObject.SetActive(true);
        displays[2].SetActive(false);
        eventTriggered = false;
        SetGUI(true, diceGUI);

        EndTurn();
    }

    void CheckForWin()
    {
        if (shipInfo[playerIndex].GetTotalTreasure() >= winAmount)
        {
            WinManager.player = shipInfo[playerIndex];

            hiddenBtns[3].onClick.Invoke();
        }
    }

    public void Add()
    {
        shipInfo[playerIndex].AddToTreasure(100);

        shipInfo[playerIndex].DepositTreasure();
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
                moveTexts[4].text = "E";
                return new Vector3Int(1, 0, 0);
            case 1:
                arrowRot = -90f;
                moveTexts[4].text = "W";
                return new Vector3Int(-1, 0, 0);
            case 3:
                arrowRot = 0f;
                moveTexts[4].text = "N";
                return new Vector3Int(0, 1, 0);
            case 4:
                arrowRot = 180f;
                moveTexts[4].text = "S";
                return new Vector3Int(0, -1, 0);
            case 5:
                arrowRot = 45f;
                moveTexts[4].text = "NE";
                return new Vector3Int(1, 1, 0);
            case 6:
                arrowRot = -135f;
                moveTexts[4].text = "SW";
                return new Vector3Int(-1, -1, 0);
            case 7:
                arrowRot = -45;
                moveTexts[4].text = "NW";
                return new Vector3Int(-1, 1, 0);
            default:
                arrowRot = 135f;
                moveTexts[4].text = "SE";
                return new Vector3Int(1, -1, 0);
        }
    }

    public void ResetRoll()
    {
        diceIndex = 0;
        moveCount = 0;
        diceSet = false;

        ClearActiveTiles();
        moveTexts[0].text = "";
        moveTexts[1].text = "";

        mainGUI[1].SetActive(false);
    }

    public void AddPlayerTreasure()
    {
        portTexts[0].gameObject.SetActive(false);
        portTexts[1].gameObject.SetActive(false);

        if (shipInfo[playerIndex].GetCurrentTreasure() > 0)
        {
            shipInfo[playerIndex].DepositTreasure();

            portTexts[0].text = "YOUR GOLD HAS BEEN ADDED TO YOUR BANK!";
            portTexts[0].gameObject.SetActive(true);
        }
        else
        {
            portTexts[0].text = "YOU ARE NOT CARRYING ANY GOLD";
            portTexts[0].gameObject.SetActive(true);
        }
    }

    public void RemoveRats()
    {
        portTexts[0].gameObject.SetActive(false);
        portTexts[1].gameObject.SetActive(false);

        if (shipInfo[playerIndex].GetRats() > 0)
        {
            shipInfo[playerIndex].ResetRats();

            portTexts[1].text = "YOUR SHIP HAS BEEN CLEARED OF THOSE PESKY RATS!";
            portTexts[1].gameObject.SetActive(true);
        }
        else
        {
            portTexts[1].text = "YOUR SHIP HAS NO RATS";
            portTexts[1].gameObject.SetActive(true);
        }
    }

    public void AddCrew()
    {
        portTexts[2].gameObject.SetActive(false);

        Crew.AddCrew(shipInfo[playerIndex], portTexts[2]);
    }

    public void SkipShipMovement()
    {
        movingSkip = true;
        skipButtons[0].gameObject.SetActive(false);

        Transform ship = players[playerIndex].transform.GetChild(0);
        
        Vector3 direction = (currPos - new Vector3(ship.position.x, 0, ship.position.z)).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        ship.rotation = lookRotation;
        players[playerIndex].transform.position = currPos;
    }

    bool FindMonster()
    {
        bool found = MonsterAI.FindMonster(shipInfo[playerIndex].GetCurrentPosition(), tilemap, hazard);
        origIndex = hazard;

        if (found)
        {
            moveTexts[3].gameObject.SetActive(true);

            hazard = -1;

            return true;
        }

        hazard = -1;

        return false;
    }

    void AddShips()
    {
        Debug.Log("New ships have been added");

        objectGenerator.SetNewShips(tilesInMap, objectsInMap);
    }
    
    void ShipMovement()
    {
        Debug.Log("Ships have moved");

        for (int i = 0; i < objectContainers[2].transform.childCount; i++)
        {
            GameObject ship = objectContainers[2].transform.GetChild(i).gameObject;

            ship.GetComponent<ShipAI>().StartMoving();
        }
    }

    void MonsterMovement()
    {
        Debug.Log("Monsters have moved");

        for (int i = 0; i < objectContainers[3].transform.childCount; i++)
        {
            GameObject monster = objectContainers[3].transform.GetChild(i).gameObject;

            monster.GetComponent<MonsterAI>().StartMoving();
        }
    }

    public void PauseGameScene()
    {
        SetGUI(false, diceGUI);
        main.enabled = false;
        tiles.gameObject.SetActive(false);
    }

    public void SetPauseScreen()
    {
        pauseMenu.SetActive(true);
        PauseGameScreen();
    }

    public void PauseGameScreen()
    {
        eventTriggered = true;
        clickable = false;
        SetGUI(false, mainGUI);
        mainGUI[1].SetActive(false);
        tiles.gameObject.SetActive(false);
    }

    public void UnpauseGameScreen()
    {
        eventTriggered = false;
        clickable = true;
        SetGUI(true, mainGUI);
        mainGUI[1].SetActive(true);
        tiles.gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        mainGUI[11].SetActive(true);
    }

    public static void ContinueGame()
    {
        continueGame = true;
    }

    void PauseMusic()
    {
        main.GetComponent<AudioListener>().enabled = false;
        main.GetComponent<AudioSource>().Pause();
    }

    void UnPauseMusic()
    {
        main.GetComponent<AudioListener>().enabled = true;
        main.GetComponent<AudioSource>().UnPause();
    }

    //TEST FUNCTION
    public void DISPLAY()
    {
        ClearActiveTiles();

        DisplayMoves(20);
    }
}
