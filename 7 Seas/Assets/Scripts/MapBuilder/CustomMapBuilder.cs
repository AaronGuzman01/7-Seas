using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CustomMapBuilder : MonoBehaviour
{
    public Camera mainCamera;
    public TileBase[] firstMapTiles;
    public TileBase[] secondMapTiles;
    public Image selected;
    public Canvas saveMenu;
    public Canvas clearMenu;
    public Canvas loadMenu;
    public Canvas deleteMenu;
    public Canvas panel;
    public Canvas panelContainer;
    public Dropdown[] optionMenus;
    public Button[] arrows;
    public Text[] errors;
    public Text loadText;
    public Text tileNum;
    public Button toggleBtn;
    public Sprite[] toggleImgs;
    public Tilemap[] tilemaps;

    private List<Canvas> panels;
    private Canvas currentPanel;
    private string currentOption;
    private Tilemap firstMap;
    private Tilemap secondMap;
    private bool firstMapActive;
    private string mapPath;
    private TileBase currTile;
    private const int originX = 0;
    private const int originY = 0;
    private int currentX = 0;
    private int currentY = 0;
    private int currentRow = 1;
    private int currentCol = 1;
    private int maxTile = 5;
    private int tileSize = 16;
    private int panelIndex = 0;
    private int[,] tileNums;
    private string mapName;

    [SerializeField]
    private GameObject test;

    private float moveInterval = 132.8f;
    private float defaultX;
    private float defaultY;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;

        Debug.Log("Map Builder Started");

        firstMap = tilemaps[0];
        secondMap = tilemaps[1];

        selected.enabled = false;

        panels = new List<Canvas>();

        panel.enabled = false;

        mapPath = Application.persistentDataPath + "/Maps/Custom/";

        Directory.CreateDirectory(mapPath);

        optionMenus[0].onValueChanged.AddListener(delegate { SetMapMenu(0); });
        optionMenus[1].onValueChanged.AddListener(delegate { SetMapMenu(1); });

        CreatePanels(optionMenus[0], true);
        CreatePanels(optionMenus[1], false);
        SetTileNumbers();  
        HideSaveMenu();
        HideClearMenu();
        HideLoadMenu();
        HideDeleteMenu();
        ClearMap();

        panels[0].enabled = true;
        currentPanel = panels[0];
        optionMenus[0].value = 1;
        firstMapActive = true;

        Debug.Log(mainCamera.transform.position.y);
        defaultX = mainCamera.transform.position.x;
        defaultY = mainCamera.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && firstMapActive)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int gridPos = firstMap.WorldToCell(mousePos);

            if (currTile != null && firstMap.HasTile(gridPos) && 
                (gridPos.x >= currentX && gridPos.x < currentX + 16) &&
                (gridPos.y <= currentY && gridPos.y > currentY - 16))
            {
                firstMap.SetTile(gridPos, currTile);
            }
        }

        if (Input.GetMouseButton(0) && !firstMapActive)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int gridPos = secondMap.WorldToCell(mousePos);

            if (currTile != null &&
                (gridPos.x >= currentX && gridPos.x < currentX + 16) &&
                (gridPos.y <= currentY && gridPos.y > currentY - 16))
            {
                secondMap.SetTile(gridPos, currTile);
            }
        }
    }

    public void CreatePanels(Dropdown menu, bool tiles)
    {
        Button[] buttons;
        int totalTiles, currTile = 0, localPanel = 0;
        double panelCount;
        List<string> options = new List<string>();

        if (tiles)
        {
            totalTiles = firstMapTiles.Length;
        }
        else
        {
            totalTiles = secondMapTiles.Length;
        }
        
        panelCount = Math.Ceiling(totalTiles / 12d);

        for (int i = 0; i < panelCount; i++)
        {
            panels.Add(Instantiate(panel, panelContainer.transform));

            buttons = panels[panelIndex].GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                if (tiles)
                {
                    button.image.sprite = ((Tile)firstMapTiles[currTile]).sprite;

                    button.name = ((Tile)firstMapTiles[currTile]).name;

                    button.onClick.AddListener(SetFirstMapTile);

                    //UnityEventTools.AddPersistentListener(button.onClick, SetFirstMapTile);
                }
                else
                {
                    button.image.sprite = ((Tile)secondMapTiles[currTile]).sprite;

                    button.name = ((Tile)secondMapTiles[currTile]).name;

                    button.onClick.AddListener(SetSecondMapTile);

                    //UnityEventTools.AddPersistentListener(button.onClick, SetSecondMapTile);
                }

                currTile++;

                if (currTile >= totalTiles)
                {
                    break;
                }
            }

            if (tiles)
            {
                options.Add("Tiles " + (localPanel+ 1).ToString());

                panels[panelIndex].name = "Tiles " + (localPanel + 1).ToString();
            }
            else
            {
                options.Add("Objects " + (localPanel + 1).ToString());

                panels[panelIndex].name = "Objects " + (localPanel + 1).ToString();
            }

            panelIndex++;
            localPanel++;

            if (currTile >= totalTiles)
            {
                break;
            }
        }

        if (tiles)
        {
            optionMenus[0].AddOptions(options);
        }
        else
        {
            optionMenus[1].AddOptions(options);
        }
    }

    public void SetTileNumbers()
    {
        int count = 1;

        tileNums = new int[maxTile, maxTile];

        for (var i = 0; i < maxTile; i++)
        {
            for (var j = 0; j < maxTile; j++)
            {
                tileNums[i, j] = count++;
            }
        }
    }

    public void SetMapMenu(int map)
    {
        Dropdown dropdown = EventSystem.current.currentSelectedGameObject.GetComponentInParent<Dropdown>();

        if (!(dropdown.value == 0))
        {
            GameObject option = EventSystem.current.currentSelectedGameObject;

            selected.enabled = false;
            currTile = null;

            option.name = option.name.Substring(option.name.IndexOf(':') + 1) ;
            option.name = option.name.Trim();

            currentPanel.enabled = false;

            foreach (Canvas panel in panels)
            {
                if (panel.name == option.name)
                {
                    currentPanel = panel;

                    panel.enabled = true;
                }
            }

            if (map == 0)
            {
                firstMapActive = true;

                optionMenus[1].Select();
                optionMenus[1].value = 0;
            }
            else
            {
                firstMapActive = false;

                optionMenus[0].Select();
                optionMenus[0].value = 0;
            }
        }
    }

    public void SetFirstMapTile()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        Vector3 position = EventSystem.current.currentSelectedGameObject.transform.position;

        foreach (TileBase tile in firstMapTiles)
        {
            if (tile.name == button.name)
            {
                currTile = tile;
                break;
            }
        }

        selected.enabled = true;
        selected.transform.position = position;
    }

    public void SetSecondMapTile()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        Vector3 position = EventSystem.current.currentSelectedGameObject.transform.position;

        foreach (TileBase tile in secondMapTiles)
        {
            if (tile.name == button.name)
            {
                currTile = tile;
                break;
            }
        }

        selected.enabled = true;
        selected.transform.position = position;
    }

    public void ClearMap()
    {
        mainCamera.transform.position = new Vector3(-80.5f, 52, -100);

        currentRow = 1;
        currentCol = 1;

        currentX = originX;
        currentY = originY;

        tileNum.text = tileNums[currentRow - 1, currentCol - 1].ToString();

        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                firstMap.SetTile(new Vector3Int(originX + j, originY - i, 0), firstMapTiles[0]);
                secondMap.ClearAllTiles();
            }
        }
        
        mapName = null;

        loadText.enabled = false;

        HideClearMenu();
    }

    public void HideSaveMenu()
    {
        saveMenu.GetComponentInChildren<InputField>().text = "";

        saveMenu.enabled = false;
    }

    public void ShowSaveMenu()
    {
        saveMenu.enabled = true;
    }

    public void HideClearMenu()
    {
        clearMenu.enabled = false;
    }

    public void ShowClearMenu()
    {
        clearMenu.enabled = true;
    }

    public void HideLoadMenu()
    {
        loadMenu.GetComponentInChildren<InputField>().text = "";

        loadMenu.enabled = false;
    }

    public void ShowLoadMenu()
    {
        loadMenu.enabled = true;
    }

    public void ShowDeleteMenu()
    {
        if (mapName != null)
        {
            deleteMenu.enabled = true;
        }
    }

    public void HideDeleteMenu()
    {
        deleteMenu.enabled = false;
    }

    public void ClearErrors()
    {
        errors[0].text = "";
        errors[1].text = "";
    }

    public void SaveMap()
    {
        string input = saveMenu.GetComponentInChildren<InputField>().text;

        if (input != null && input != "")
        {
            string tileType = "";
            string tempStr = "";
            string name;

            for (int i = 0; i < 80; i++)
            {
                for (int j = 0; j < 80; j++)
                {
                    name = firstMap.GetTile(new Vector3Int(originX + i, originY - j, 0)).name;

                    for (int k = 0; k < firstMapTiles.Length; k++)
                    {
                        if (firstMapTiles[k].name == name)
                        {
                            tileType += k.ToString() + " ";

                            break;
                        }
                    }

                    if (secondMap.HasTile(new Vector3Int(originX + i, originY - j, 0)))
                    {
                        name = secondMap.GetTile(new Vector3Int(originX + i, originY - j, 0)).name;

                        for (int k = 0; k < firstMapTiles.Length; k++)
                        {
                            if (secondMapTiles[k].name == name)
                            {
                                tempStr += k.ToString() + " ";

                                break;
                            }
                        }
                    }
                    else
                    {
                        tempStr += (-1).ToString() + " ";
                    }
                }

                tileType += "\n";
                tempStr += "\n";
            }

            tileType += tempStr;

            WriteMap(input, tileType);

            HideSaveMenu();
        }
        else
        {
            errors[0].text = "Map name cannot be empty.";
        }
    }

    public void LoadMap()
    {
        string fileName = loadMenu.GetComponentInChildren<InputField>().text;
        string mapText;

        if (System.IO.File.Exists(mapPath + fileName + ".txt"))
        {
            mapText = System.IO.File.ReadAllText(mapPath + fileName + ".txt");

            mapName = fileName;

            WriteTiles(mapText);

            loadText.enabled = true;

            HideLoadMenu();
        }
        else
        {
            errors[1].text = "Map does not exist.";
        }
    }

    public void WriteMap(string fileName, string mapText)
    {
        if (fileName != "")
        {
            System.IO.File.WriteAllText(mapPath + fileName + ".txt", mapText);
        }
    }

    public void WriteTiles(string map)
    {
        int tile, space;

        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                tile = int.Parse(map.Substring(0, 1));

                map = map.Remove(0, 2);

                firstMap.SetTile(new Vector3Int(originX + i, originY - j, 0), firstMapTiles[tile]);
            }

            map = map.Remove(0, 1);
        }

        secondMap.ClearAllTiles();

        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                space = map.IndexOf(' ');

                tile = int.Parse(map.Substring(0, space));

                map = map.Remove(0, (map.Substring(0,space).Length) + 1);

                if (tile != -1)
                {
                    secondMap.SetTile(new Vector3Int(originX + i, originY - j, 0), secondMapTiles[tile]);
                }
            }

            map = map.Remove(0, 1);
        }
    }

    public void DeleteMap()
    {
        if (mapName != null)
        {
            System.IO.File.Delete(mapPath + mapName + ".txt");

            mapName = null;

            loadText.enabled = false;

            HideDeleteMenu();
        }
    }

    public void ShiftRight()
    {
        if (currentCol == maxTile)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x - 531.2f, mainCamera.transform.position.y, -100);
            currentCol = 1;
            currentX -= tileSize * 4;
        }
        else
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + moveInterval, mainCamera.transform.position.y, -100);

            currentCol++;

            currentX += tileSize;
        }

        ProcessTile();
    }

    public void ShiftLeft()
    {
        if (currentCol == 1)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 531.2f, mainCamera.transform.position.y, -100);
            currentCol = maxTile;
            currentX += tileSize * 4;
        }
        else
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x - moveInterval, mainCamera.transform.position.y, -100);

            currentCol--;

            currentX -= tileSize;
        }

        ProcessTile();
    }

    public void ShiftUp()
    {
        if (currentRow == 1)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 531.2f, -100);
            currentRow = maxTile;
            currentY -= tileSize * 4;
        }
        else
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + moveInterval, -100);

            currentRow--;

            currentY += tileSize;
        }

        ProcessTile();
    }

    public void ShiftDown()
    {
        if (currentRow == maxTile)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 531.2f, -100);
            currentRow = 1;
            currentY += tileSize * 4;
        }
        else
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - moveInterval, -100);

            currentRow++;

            currentY -= tileSize;
        }

        ProcessTile();
    }

    public void ShiftDownLeft()
    {
        ShiftDown();
        ShiftLeft();
    }

    public void ShiftDownRight()
    {
        ShiftDown();
        ShiftRight();
    }

    public void ShiftUpLeft()
    {
        ShiftUp();
        ShiftLeft();
    }

    public void ShiftUpRight()
    {
        ShiftUp();
        ShiftRight();
    }

    public void ProcessTile()
    {
        tileNum.text = tileNums[currentRow - 1, currentCol - 1].ToString();
    }

    public void Toggle()
    {
        Vector3 pos = mainCamera.transform.position;

        if (toggleBtn.image.sprite == toggleImgs[0])
        {
            toggleBtn.image.sprite = toggleImgs[1];

            maxTile = 10;

            tileSize = 8;

            moveInterval = 59.02f;

            mainCamera.transform.position = new Vector3(defaultX, defaultY, -100);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + (moveInterval * (currentCol - 1)), mainCamera.transform.position.y - (moveInterval * (currentRow - 1)), -100);

            currentX = currentX - 8 * (tileNums[0, currentCol - 1] - 1);

            currentY = currentY + 8 * (tileNums[0, currentRow - 1] - 1);

            SetTileNumbers();
        }
        else
        {
            toggleBtn.image.sprite = toggleImgs[0];

            maxTile = 5;

            tileSize = 16;

            moveInterval = 132.8f;

            mainCamera.transform.position = new Vector3(defaultX, defaultY, -100);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + (moveInterval * (currentCol - 1)), mainCamera.transform.position.y - (moveInterval * (currentRow - 1)), -100);

            currentX = currentX + 8 * (tileNums[0, currentCol - 1] - 1);
            currentY = currentY - 8 * (tileNums[0, currentRow - 1] - 1);

            if (tileNums[0, currentCol - 1] >= 5)
            {
                currentX = 64;

                currentCol = 5;

                pos = mainCamera.transform.position;

                mainCamera.transform.position = new Vector3(450.7f, pos.y, pos.z);
            }

            if (tileNums[0, currentRow - 1] >= 5)
            {
                currentY = -64;

                currentRow = 5;

                pos = mainCamera.transform.position;

                mainCamera.transform.position = new Vector3(pos.x, -479.2f, pos.z);
            }

            

            SetTileNumbers();
        }
        
        ProcessTile();
    }
}
