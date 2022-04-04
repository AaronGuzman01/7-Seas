using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomPosition : MonoBehaviour
{
    HashSet<string> tilesUsed = new HashSet<string>();
    Canvas[] containers;
    List<GameObject> ports;
    List<GameObject> monsters;
    List<GameObject> ships;
    GameObject siren;
    List<PlayerShip> players;
    HashSet<string> portNames = new HashSet<string>();
    int playerIndex = 0;
    bool found = false;

    Tilemap tilemap;

    public RandomPosition(List<PlayerShip> players, List<GameObject> ports, List<GameObject> ships, List<GameObject> monsters, 
        GameObject siren, Canvas[] containers)
    {
        this.players = players;
        this.ports = ports;
        this.ships = ships;
        this.monsters = monsters;
        this.siren = siren;
        this.containers = containers;

        SetPortNames();
    }

    void SetPortNames()
    {
        foreach (GameObject port in ports)
        {
            portNames.Add(port.name);
        }
    }

    public void SetTilemap(Tilemap map)
    {
        tilemap = map;
    }

    public void SetMapObjects(int[,] mapTiles, int[,] mapObjects)
    {
        int space, tile;
        string map = PlayerPrefs.GetString("Map2");

        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                space = map.IndexOf(' ');

                tile = int.Parse(map.Substring(0, space));

                map = map.Remove(0, (map.Substring(0, space).Length) + 1);

                if (tile != -1)
                {
                    if (tile < 8 && MapLoad.portIndices.Contains(tile))
                    {
                        int tileX, tileY;

                        if (i + 1 <= 16)
                        {
                            tileX = 1;
                        }
                        else
                        {
                            tileX = (int)Mathf.Ceil(i / 16f);
                        }

                        if (j + 1 <= 16)
                        {
                            tileY = 1;
                        }
                        else
                        {
                            tileY = (int)Mathf.Ceil(i / 16f);
                        }

                        tilesUsed.Add(tileX.ToString() + ' ' + tileY.ToString());

                        SetPortOrSiren(ports[tile], mapTiles, mapObjects, i, j, 0);

                        portNames.Remove(ports[tile].name);
                    }

                    if (tile == 9)
                    {
                        int index = Random.Range(0, ships.Count);

                        SetShipOrMonster(ships[index], mapTiles, mapObjects, i, j, 1);
                    }

                    if (tile >= 12 && tile <= 15)
                    {
                        switch(tile)
                        {
                            case 12:
                                SetShipOrMonster(monsters[0], mapTiles, mapObjects, i, j, 0);
                                break;
                            case 13:
                                SetShipOrMonster(monsters[1], mapTiles, mapObjects, i, j, 0);
                                break;
                            case 14:
                                SetShipOrMonster(monsters[2], mapTiles, mapObjects, i, j, 0);
                                break;
                            default:
                                SetShipOrMonster(monsters[3], mapTiles, mapObjects, i, j, 0);
                                break;
                        }
                    }
                }
            }

            map = map.Remove(0, 1);
        }
    }

    public void GenerateHomePortPositions(int[,] mapTiles, int[,] mapObjects) 
    {
        int objectCount = 0, tryCount = 0;
        int tileX, tileY, tilePositionX, tilePositionY;
        int positionX, positionY;

        found = false;
        playerIndex = 0;

        foreach (GameObject port in ports)
        {
            objectCount++;

            if (!found && objectCount < ports.Count && portNames.Contains(port.name))
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

                            SetPortOrSiren(port, mapTiles, mapObjects, positionX, positionY, 0);

                            tryCount++;
                        }

                        tryCount = 0;
                    }
                }
            }

            found = false;
            playerIndex++;
        }
    }


    public void GeneratePortPositions(int[,] mapTiles, int[,] mapObjects)
    {
        int tryCount = 0;
        int tileX, tileY, tilePositionX, tilePositionY;
        int positionX, positionY;

        found = false;
        playerIndex = 0;

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

                        SetPortOrSiren(ports[ports.Count - 1], mapTiles, mapObjects, positionX, positionY, 0);

                        tryCount++;
                    }

                    tryCount = 0;
                }

                found = false;
            }
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

                        SetPortOrSiren(siren, mapTiles, mapObjects, positionX, positionY, 1);

                        tryCount++;
                    }

                    tryCount = 0;
                }

                found = false;
            }
        }

        tilesUsed.Clear();
    }

    void SetShipOrMonster(GameObject tileObject, int[,] mapTiles, int[,] mapObjects, int x, int y, int type)
    {
        if (mapTiles[x, y] < 2 && mapTiles[x, y] > -1 && mapObjects[x, y] == 0)
        {
            GameObject gameObject = Instantiate(tileObject);

            gameObject.SetActive(true);

            gameObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(-34 + x, (y - 32) * -1, 0));

            if (type == 0)
            {
                gameObject.transform.parent = containers[3].transform;
                gameObject.GetComponent<Monstermovement>().enabled = false;
                gameObject.transform.position = gameObject.transform.position + (Vector3.up / 2);

                mapObjects[x, y] = 2;
            }
            else
            {
                gameObject.transform.parent = containers[2].transform;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

                mapObjects[x, y] = 1;
            }

            found = true;
        }
    }

    void SetPortOrSiren(GameObject gameObject, int[,] mapTiles, int[,] mapObjects, int x, int y, int type)
    {
        if (type == 0 && mapTiles[x, y] < 2 && mapObjects[x, y] == 0)
        {
            mapTiles[x, y] = -1; 
                
            GameObject newObject = Instantiate(gameObject);

            newObject.transform.parent = containers[1].transform;

            newObject.SetActive(true);

            newObject.transform.position = tilemap.CellToWorld(new Vector3Int(-34 + x, (y - 32) * -1, 0));

            if (playerIndex < players.Count)
            {
                players[playerIndex].SetPortPosition(tilemap.WorldToCell(newObject.transform.position));
            }

            newObject.transform.position = new Vector3(newObject.transform.position.x + 2, 0.97f, newObject.transform.position.z + 2);

            found = true;
        }
        if (type == 1 && mapTiles[x, y] == 2 && mapObjects[x, y] == 0)
        {
            mapObjects[x, y] = 3;

            GameObject newObject = Instantiate(gameObject);

            newObject.transform.parent = containers[4].transform;

            newObject.SetActive(true);

            newObject.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(-34 + x, (y - 32) * -1, 0));

            newObject.transform.position = newObject.transform.position + new Vector3(0, 1.2f, 0);

            newObject.GetComponent<Siren>().SetPlayers(players);
            newObject.GetComponent<Siren>().SetBounds((-34 + x) - 3, ((y - 32) * -1) + 3, (-34 + x) + 3, ((y - 32) * -1) - 3);
            newObject.GetComponent<Siren>().SetPosition(tilemap.WorldToCell(newObject.transform.position));

            found = true;
        }
    }
}
