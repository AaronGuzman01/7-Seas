using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomPosition : MonoBehaviour
{
    public HashSet<int> portX = new HashSet<int>();
    public HashSet<int> shipX = new HashSet<int>();
    public HashSet<int> monsterX = new HashSet<int>();
    public HashSet<int> portY = new HashSet<int>();
    public HashSet<int> shipY = new HashSet<int>();
    public HashSet<int> monsterY = new HashSet<int>();

    List<GameObject> ports;
    List<GameObject> monsters;
    List<GameObject> ships;

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

    public void GetPosition(int[,] mapTiles, int[,] mapObjects)
    {
        int col, row, objectCount = 0;
        int tilePositionX, tilePositionY;
        int positionX, positionY;

        while (!found)
        {
            col = Random.Range(1, 25);
            row = Random.Range(1, 25);

            if (!portX.Contains(col) || !portY.Contains(row))
            {
                if (!portX.Contains(col))
                {
                    portX.Add(col);
                }

                if (!portY.Contains(row))
                {
                    portY.Add(row);
                }

                tilePositionX = 16 * col;
                tilePositionY = 16 * row;

                while (objectCount < count)
                {
                    positionX = Random.Range(tilePositionX - 1, tilePositionX + 15);
                    positionY = Random.Range(tilePositionY - 1, tilePositionY + 15);

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

                found = true;
            }
        }
    }

    public void SetPosition(List<GameObject> objects, int[,] mapTiles, int[,] mapObjects, int x, int y)
    {

        int objectIndex;

        objectIndex = Random.Range(0, objects.Count);

        if (mapTiles[x, y] < 2 && mapObjects[x, y] == 0)
        {
            GameObject gameObject = Instantiate(objects[objectIndex]);

            gameObject.SetActive(true);

            gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(34 + x, -24 + y, 0));

            if (type == 0)
            {
                mapObjects[x, y] = 1;
            }
            else if (type == 1)
            {
                gameObject.transform.position = gameObject.transform.position + (Vector3.up / 2);

                mapObjects[x, y] = 1;
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
