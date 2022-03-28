using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomPosition : MonoBehaviour
{
    public HashSet<int> tileX = new HashSet<int>();
    public HashSet<int> tileY = new HashSet<int>();

    List<GameObject> ports;
    List<GameObject> monsters;
    List<GameObject> ships;
    GameObject[] players;

    Tilemap tilemap;
    int type;
    int count;
    bool found = false;

    public RandomPosition(List<GameObject> mapObects, int type, int count)
    {
        if (type == 0)
        {
            ports = mapObects;
        }
        else if (type == 1)
        {
            monsters = mapObects;
        }
        else
        {
            ships = mapObects;
        }

        this.type = type;
        this.count = count;
    }

    public RandomPosition(GameObject[] players)
    {
        this.players = players;
    }

    public void SetTilemap(Tilemap map)
    {
        tilemap = map;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GeneratePosition(int[,] mapTiles, int[,] mapObjects)
    {
        int  objectCount = 0, tryCount = 0;
        int tilePositionX, tilePositionY;
        int positionX, positionY;

        for (int i = 1; i <= 5; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                tilePositionX = 16 * i;
                tilePositionY = 16 * j;

                while (objectCount < count && tryCount < (16 * 16))
                {
                    positionX = Random.Range(tilePositionX - 16, tilePositionX);
                    positionY = Random.Range(tilePositionY - 16, tilePositionY);

                    tryCount++;
                    objectCount++;

                    if (type == 0)
                    {
                        SetPosition(ports, mapTiles, mapObjects, positionX, positionY);
                    }
                    else if (type == 1)
                    {
                        SetPosition(monsters, mapTiles, mapObjects, positionX, positionY);
                    }
                    else
                    {
                        SetPosition(ships, mapTiles, mapObjects, positionX, positionY);
                    }
                }

                tryCount = 0;
                objectCount = 0;
            }
        }
    }

    public void GeneratePlayerPosition(int[,] mapTiles, int[,] mapObjects)
    {

    }

    public void SetPosition(List<GameObject> objects, int[,] mapTiles, int[,] mapObjects, int x, int y)
    {

        int objectIndex;

        objectIndex = Random.Range(0, objects.Count);

        if (mapTiles[x, y] < 2 && mapObjects[x, y] == 0)
        {
            GameObject gameObject = Instantiate(objects[objectIndex]);

            gameObject.SetActive(true);

            gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(-34 + x, (y - 32) * -1, 0));

            if (type == 0)
            {
                mapObjects[x, y] = 3;
            }
            else if (type == 1)
            {
                gameObject.GetComponent<Monstermovement>().enabled = false;
                gameObject.transform.position = gameObject.transform.position + (Vector3.up / 2);

                mapObjects[x, y] = 2;
            }
            else
            {
                gameObject.transform.position = gameObject.transform.position + Vector3.up;

                mapObjects[x, y] = 1;
            }
        }
    }

    public void ResetPositions()
    {

    }
}
