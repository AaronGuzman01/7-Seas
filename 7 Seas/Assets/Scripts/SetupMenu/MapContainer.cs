using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEditor;

public class MapContainer : MonoBehaviour {

    public Vector2 MapSize;
    public GameObject[,] tileMap;
    //float horizExtent;
    public GameObject tile;
    public GameObject mapContainer;
    GameObject newMapContainer;
    public float tileSize;
    public Sprite sprite;
    string text;
    public List<Sprite> tileList;
    public List<string> mapList;
    int mapListIndex = 0;
    public Text setSailFor;
    public List<string> mapNames;
    public TextAsset defaultMap;
    public TextAsset piratesCove;
    public string customMap;
    string customMapName;
    public GameObject leftArrow;
    public GameObject rightArrow;

    public Tilemap tilemap;
    public Tile[] tiles;
    private List<string> allFiles;
    private string customMapPath;
    private string defaultMapPath;
    private const int x = 0;
    private const int y = 0;
    private int currMap = 0;

    // Use this for initialization
    void Start () {
        string[] files;

        string defaultMapPath = Application.persistentDataPath + "/Maps/Default";
        string customMapPath = Application.persistentDataPath + "/Maps/Custom";

        allFiles = new List<string>();

        tilemap.transform.parent.SetParent(mapContainer.transform);

        files = Directory.GetFiles(defaultMapPath);

        foreach (string file in files) {
            allFiles.Add(file);
        }

        files = Directory.GetFiles(customMapPath);

        foreach (string file in files)
        {
            allFiles.Add(file);
        }

        LoadPreview(allFiles[0]);

        /*
        //initialize map names
        mapNames = new List<string>();
        mapNames.Add("Default Map");
        mapNames.Add("Pirate's Cove");
        if (PlayerPrefs.HasKey("customMapName"))
        {
            customMapName = PlayerPrefs.GetString("customMapName");
            mapNames.Add(customMapName);
        }

        setSailFor.text = "Set Sail For: " + mapNames[0];

        //initialize map viewer
        defaultMap = (TextAsset)Resources.Load("map1", typeof(TextAsset));
        piratesCove = (TextAsset)Resources.Load("PiratesCove", typeof(TextAsset));

        //initialize mapList
        mapList = new List<string>();
        mapList.Add(defaultMap.text);
        mapList.Add(piratesCove.text);

        //if customMapName exists, add custom map to mapList
        if (PlayerPrefs.HasKey("customMapName"))
        {
            customMap = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + customMapName + ".txt");
            mapList.Add(customMap);
        }

        tileSize = 2f;
        MapSize = new Vector2(32, 32);
        tileMap = new GameObject[(int)MapSize.x, (int)MapSize.y];
        newMapContainer = Instantiate(mapContainer) as GameObject;
        SetTiles();
        ReadTiles();
        ChangeArrowColor(leftArrow, new Color32(50, 50, 50, 255));

        //initialize first map to load
        PlayerPrefs.SetString("mapText", mapList[0]);
        */
    }

    void LoadPreview(string mapPath)
    {
        string map = System.IO.File.ReadAllText(mapPath);

        PlayerPrefs.SetString("Map", map);

        int tile, space;

        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                space = map.IndexOf(' ');

                tile = int.Parse(map.Substring(0, space));

                map = map.Remove(0, (map.Substring(0, space).Length) + 1);

                tilemap.SetTile(new Vector3Int(x + i, y + j, 0), tiles[tile]);
            }

            map = map.Remove(0, 1);
        }

        setSailFor.text = "Set Sail For: " + Path.GetFileNameWithoutExtension(mapPath);
    }

    /*
    void SetTiles()
    {
        //horizExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        for (var y = 0; y < MapSize.y; y++)
        {
            for (var x = 0; x < MapSize.x; x++)
            {
                GameObject newImage = Instantiate(tile) as GameObject;
                var image = newImage.GetComponent<SpriteRenderer>();
                image.enabled = true;
                image.sprite = sprite;
                newImage.transform.SetParent(newMapContainer.transform, false);
                //btn.image.rectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                //newImage.GetComponent<RectTransform>().sizeDelta = new Vector2(tileSize, tileSize);
                //newImage.transform.localScale = new Vector3(.125f, .125f, 1);
                newImage.transform.position = new Vector3(x*tileSize, -y*tileSize, 0);
                //newImage.transform.position = new Vector3(x - horizExtent + .5f, -y +
                //    (Camera.main.orthographicSize * 2 - 1) / 2f, 0);
                tileMap[x, y] = newImage;
            }
        }

        newMapContainer.transform.localScale = new Vector3(.149f, .149f, 1);
        newMapContainer.transform.position = new Vector3(.55f, 1.81f, -20f);
    }

    public void ReadTiles()
    {
        int index = 0;
        int num = 0;
        text = mapList[mapListIndex];
        //text = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + filename + ".txt");
        for (var y = 0; y < MapSize.y; y++)
        {
            for (var x = 0; x < MapSize.x; x++)
            {
                num = text[index] - '0';
                //ignore the new line character
                while (num < 0)
                {
                    index = index + 1;
                    num = text[index] - '0';
                }

                //set the index corresponding to sprite image as the tile image...
                tileMap[x, y].GetComponent<SpriteRenderer>().sprite = FindTiles(num);

                ++index;
            }
        }
    }

    //find a certain tile
    Sprite FindTiles(int num)
    {
        foreach (var item in tileList)
        {
            string itemNum = item.name;
            itemNum = "" + itemNum[0];
            if (int.Parse(itemNum) == num)
            {
                return item;
            }
        }
        return null;
    }

    public void ChangeArrowColor(GameObject arrowType, Color color)
    {
        ColorBlock cb = arrowType.GetComponent<Button>().colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        arrowType.GetComponent<Button>().colors = cb;
    }
    */

    public void ChangeLeftArrow()
    {
        if (currMap - 1 >= 0)
        {
            currMap--;

            LoadPreview(allFiles[currMap]);
        }

        /*
        if (mapListIndex > 0)
        {
            --mapListIndex;
        }
        PlayerPrefs.SetString("mapText", mapList[mapListIndex]);
        ReadTiles();
        setSailFor.text = "Set Sail For: " + mapNames[mapListIndex];

        //right arrow should be white if pressing left arrow
        ChangeArrowColor(rightArrow, Color.white);

        //grey out arrow to indicate end of list in this direction
        if (mapListIndex == 0)
        {
            ChangeArrowColor(leftArrow, new Color32(50,50,50,255));
        }
        else
        {
            ChangeArrowColor(leftArrow, Color.white);
        }
        */
    }
    public void ChangeRightArrow()
    {
        if (currMap + 1 < allFiles.Count)
        {
            currMap++;

            LoadPreview(allFiles[currMap]);
        } 

        /*
        if (mapListIndex < mapList.Count - 1)
        {
            ++mapListIndex;
        }
        PlayerPrefs.SetString("mapText", mapList[mapListIndex]);
        ReadTiles();
        setSailFor.text = "Set Sail For: " + mapNames[mapListIndex];

        //left arrow should be white if pressing right arrow
        ChangeArrowColor(leftArrow, Color.white);

        //grey out arrow to indicate end of list in this direction
        if (mapListIndex == mapList.Count - 1)
        {
            ChangeArrowColor(rightArrow, new Color32(50, 50, 50,255));
        }
        else
        {
            ChangeArrowColor(rightArrow, Color.white);
        }
        */
    }
}
