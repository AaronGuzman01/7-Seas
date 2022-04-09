using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipAI : MonoBehaviour
{
    HashSet<int> positionsOccupied = new HashSet<int>();
    List<GameObject> players;
    List<GameObject> monsters;
    Tilemap tilemap;
    int tileIndex = 0;
    int x, y, newX, newY;
    int[,] tiles;
    int[,] objects;
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
            SearchForPosition();
        }

        moved = false;
    }

    void SearchForPosition()
    {
        found = false;

        while (positionsOccupied.Count < 8 && !found)
        {
            tileIndex = Random.Range(0, 8);

            SetDirection(tileIndex);
        }

        positionsOccupied.Clear();
    }

    void SearchDirection(int row, int col)
    {
        for (int i = 1; i <= 3; i++)
        {
            if (CheckMapPosition(x + (i * row), y + (i * col)))
            {
                found = true;
                MoveToPosition();
                break;
            }
        }
    }

    bool CheckMapPosition(int x, int y)
    {
        if ((x >= 0 && x < 80 && y >= 0 && y < 80) && tiles[x, y] <= 2 && tiles[x, y] > -1 && objects[x, y] == 0)
        {
            newX = x;
            newY = y;

            return true;
        }

        return false;
    }

    void SetDirection(int index)
    {
        switch (index)
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
}
