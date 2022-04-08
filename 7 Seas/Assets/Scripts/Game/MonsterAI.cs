using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterAI : MonoBehaviour
{
    public static Canvas monsterContainer;
    public static List<PlayerShip> players;
    public int index;
    HashSet<int> positionsOccupied = new HashSet<int>();
    Tilemap tilemap;
    int tileIndex = 0;
    int x, y, newX, newY;
    int[,] tiles;
    int[,] objects;
    int searchType = 0;
    bool found = false;
    bool moved = false;

    public void SetMaps(int[,] tiles, int[,] objects, Tilemap tilemap)
    {
        this.tiles = tiles;
        this.objects = objects;
        this.tilemap = tilemap;
    }

    public void SetCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void StartMoving()
    {
        if (!moved)
        {
            SearchForPlayer();
        }

        if (!moved)
        {
            SearchForPosition();
        }

        moved = false;
    }

    void SearchForPlayer()
    {
        found = false;
        searchType = 0;

        while (positionsOccupied.Count < 8 && !found)
        {
            tileIndex = Random.Range(0, 8);

            SetDirection(tileIndex);
        }

        if (found)
        {
            foreach (PlayerShip player in players)
            {
                if (player.GetCurrentPosition() == tilemap.WorldToCell(transform.position))
                {
                    player.SetMonsterCombat();

                    break;
                }
            }
        }

        positionsOccupied.Clear();
    }

    void SearchForPosition()
    {
        found = false;
        searchType = 1;

        while (positionsOccupied.Count < 8 && !found)
        {
            tileIndex = Random.Range(0, 8);

            SetDirection(tileIndex);
        }

        positionsOccupied.Clear();
    }

    void SearchDirection(int row, int col)
    {
        for(int i = 1; i <= 3; i++)
        {
            if (searchType == 0 && CheckPlayerPosition(x + (i * row), y + (i * col)))
            {
                found = true;
                MoveToPosition();
                break;
            }
            else if (searchType == 1 && CheckMapPosition(x + (i * row), y + (i * col)))
            {
                found = true;
                MoveToPosition();
                break;
            }
        }
    }

    bool CheckPlayerPosition(int x, int y)
    {
        if ((x >= 0 && x < 80 && y >= 0 && y < 80) && tiles[x,y] <= 2 && objects[x, y] == -1)
        {
            newX = x;
            newY = y;

            return true;
        }

        return false;
    }
    bool CheckMapPosition(int x, int y)
    {
        if ((x >= 0 && x < 80 && y >= 0 && y < 80) && tiles[x, y] <= 2 && objects[x, y] == 0)
        {
            newX = x;
            newY = y;

            return true;
        }

        return false;
    }

    void SetDirection(int index)
    {
       switch(index)
        {
            case 0:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(-1, 1);
                }
                break;

            case 1:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(0, 1);
                }
                break;

            case 2:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(1, 1);
                }
                break;

            case 3:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(-1, 0);
                }
                break;

            case 4:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(1, 0);
                }
                break;

            case 5:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(-1, -1);
                }
                break;

            case 6:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(0, -1);
                }
                break;

            default:
                if (!positionsOccupied.Contains(index))
                {
                    positionsOccupied.Add(index);
                    SearchDirection(1, -1);
                }
                break;

        }
            
    }

    void MoveToPosition()
    {
        objects[x, y] = 0;
        objects[newX, newY] = 2;

        x = newX;
        y = newY;
        moved = true;

        transform.position = tilemap.GetCellCenterWorld(new Vector3Int(-34 + newX, (newY - 32) * -1, 0));
    }

    public static bool FindMonster(Vector3Int playerPos, Tilemap map, int index)
    {
        bool found = false;

        for (int i = 1; i <= 7; i++)
        {
            found = CheckInPosition(playerPos + new Vector3Int(i, 0, 0), map, index);
            if (found)
                break;

            found = CheckInPosition(playerPos + new Vector3Int(-i, 0, 0), map, index);
            if (found)
                break;

            found = CheckInPosition(playerPos + new Vector3Int(0, i, 0), map, index);
            if (found)
                break;

            found = CheckInPosition(playerPos + new Vector3Int(0, -i, 0), map, index);
            if (found)
                break;

            found = CheckInPosition(playerPos + new Vector3Int(i, i, 0), map, index);
            if (found)
                break;

            found = CheckInPosition(playerPos + new Vector3Int(-i, -i, 0), map, index);
            if (found)
                break;

            if (found)
                break;

            found = CheckInPosition(playerPos + new Vector3Int(-i, i, 0), map, index);
            if (found)
                break;

            found = CheckInPosition(playerPos + new Vector3Int(i, -i, 0), map, index);
            if (found)
                break;

        }

        return found;
    }

    static bool CheckInPosition(Vector3Int pos, Tilemap map, int index)
    {
        for (int i = 0; i < monsterContainer.transform.childCount; i++)
        {
            GameObject monster = monsterContainer.transform.GetChild(i).gameObject;

            if (map.WorldToCell(monster.transform.position) == pos)
            {
                if (monster.GetComponent<MonsterAI>().index == index)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
