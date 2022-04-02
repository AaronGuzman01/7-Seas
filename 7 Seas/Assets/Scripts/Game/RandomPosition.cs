using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomPosition : MonoBehaviour
{
    HashSet<string> tilesUsed = new HashSet<string>();
    Canvas container;
    List<GameObject> ports;
    List<GameObject> monsters;
    List<GameObject> ships;
    GameObject siren;
    List<PlayerShip> players;

    Tilemap tilemap;
    int type;
    int count;
    int playerIndex = 0;
    bool found = false;

    public RandomPosition(List<GameObject> mapObects, Canvas container, int type, int count)
    {
        if (type == 1)
        {
            monsters = mapObects;
        }
        else
        {
            ships = mapObects;
        }

        this.type = type;
        this.count = count;
        this.container = container;
    }

    public RandomPosition(List<PlayerShip> players, List<GameObject> mapObects, Canvas container, int type, int count)
    {
        ports = mapObects;

        this.type = type;
        this.count = count;
        this.players = players;
        this.container = container;
    }

    public RandomPosition(GameObject siren, Canvas container, int type, int count)
    {
        this.siren = siren;

        this.type = type;
        this.count = count;
        this.container = container;
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
                    
                    if (type == 1)
                    {
                        SetPosition(monsters, mapTiles, mapObjects, positionX, positionY);
                    }
                    else
                    {
                        SetPosition(ships, mapTiles, mapObjects, positionX, positionY);
                    }

                    if (found)
                    {
                        found = false;
                        objectCount++;
                    }
                }

                tryCount = 0;
                objectCount = 0;
            }
        }
    }

    public void GeneratePortPosition(int[,] mapTiles, int[,] mapObjects) 
    {
        int objectCount = 0, tryCount = 0;
        int tileX, tileY, tilePositionX, tilePositionY;
        int positionX, positionY;

        foreach (GameObject port in ports)
        {
            objectCount++;

            if (!found && objectCount < ports.Count)
            {
                while (!found && tryCount < 25)
                {
                    tileX = Random.Range(1, 6);
                    tileY = Random.Range(1, 6);

                    if (!tilesUsed.Contains(tileX.ToString() + ' ' + tileY.ToString()))
                    {
                        tilesUsed.Add(tileX.ToString() + ' ' + tileY.ToString());

                        tilePositionX = 16 * tileX;
                        tilePositionY = 16 * tileY;

                        while (tryCount < 25 && !found)
                        {
                            positionX = Random.Range(tilePositionX - 16, tilePositionX);
                            positionY = Random.Range(tilePositionY - 16, tilePositionY);

                            SetPositionWithObject(port, mapTiles, mapObjects, positionX, positionY);

                            tryCount++;
                        }

                        tryCount = 0;
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 5; i++)
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        if (!tilesUsed.Contains(i.ToString() + ' ' + j.ToString()))
                        {
                            tilesUsed.Add(i.ToString() + ' ' + j.ToString());

                            tilePositionX = 16 * i;
                            tilePositionY = 16 * j;

                            while (tryCount < 25 && !found)
                            {
                                positionX = Random.Range(tilePositionX - 16, tilePositionX);
                                positionY = Random.Range(tilePositionY - 16, tilePositionY);

                                SetPositionWithObject(port, mapTiles, mapObjects, positionX, positionY);

                                tryCount++;
                            }

                            tryCount = 0;
                        }

                        found = false;
                    }
                }
            }

            found = false;
            playerIndex++;
        }
    }
    public void GeneratePlayerPosition(int[,] mapTiles, int[,] mapObjects)
    {

    }

    public void GenerateSirenPosition(int[,] mapTiles, int[,] mapObjects)
    {
        int tryCount = 0;
        int tilePositionX, tilePositionY;
        int positionX, positionY;

        for (int i = 1; i <= 5; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                if (!tilesUsed.Contains(i.ToString() + ' ' + j.ToString()))
                {
                    tilesUsed.Add(i.ToString() + ' ' + j.ToString());

                    tilePositionX = 16 * i;
                    tilePositionY = 16 * j;

                    while (tryCount < 25 && !found)
                    {
                        positionX = Random.Range(tilePositionX - 16, tilePositionX);
                        positionY = Random.Range(tilePositionY - 16, tilePositionY);

                        SetPositionWithObject(siren, mapTiles, mapObjects, positionX, positionY);

                        tryCount++;
                    }

                    tryCount = 0;
                }

                found = false;
            }
        }
    }

    void SetPosition(List<GameObject> objects, int[,] mapTiles, int[,] mapObjects, int x, int y)
    {
        int objectIndex;

        objectIndex = Random.Range(0, objects.Count);

        if (mapTiles[x, y] < 2 && mapTiles[x, y] > -1 && mapObjects[x, y] == 0)
        {
            GameObject gameObject = Instantiate(objects[objectIndex]);

            gameObject.transform.parent = container.transform;

            gameObject.SetActive(true);

            gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(-34 + x, (y - 32) * -1, 0));

            if (type == 1)
            {
                gameObject.GetComponent<Monstermovement>().enabled = false;
                gameObject.transform.position = gameObject.transform.position + (Vector3.up / 2);

                mapObjects[x, y] = 2;
            }
            else
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

                mapObjects[x, y] = 1;
            }

            found = true;
        }
    }

    void SetPositionWithObject(GameObject gameObject, int[,] mapTiles, int[,] mapObjects, int x, int y)
    {
        if (type == 0 && mapTiles[x, y] < 2 && mapObjects[x, y] == 0)
        {
            mapTiles[x, y] = -1; 
                
            GameObject newObject = Instantiate(gameObject);

            newObject.transform.parent = container.transform;

            newObject.SetActive(true);

            newObject.transform.position = tilemap.CellToWorld(new Vector3Int(-34 + x, (y - 32) * -1, 0));

            if (playerIndex < players.Count)
            {
                players[playerIndex].SetPortPosition(tilemap.WorldToCell(newObject.transform.position));
            }

            newObject.transform.position = new Vector3(newObject.transform.position.x + 2, 0.97f, newObject.transform.position.z + 2);

            found = true;
        }
        if (type == 3 && mapTiles[x, y] == 2 && mapObjects[x, y] == 0)
        {
            mapObjects[x, y] = 3;

            GameObject newObject = Instantiate(gameObject);

            newObject.transform.parent = container.transform;

            newObject.SetActive(true);

            newObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(-34 + x, (y - 32) * -1, 0));

            newObject.transform.position = newObject.transform.position + new Vector3(0, 1.2f, 0);

            found = true;
        }
    }
}
