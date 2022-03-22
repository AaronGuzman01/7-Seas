using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    float totalTreasure;
    float currTreasure;
    int playerNum;
    GameObject ship;
    string shipName;
    Vector3Int currPosition;
    Vector3Int prevPosition;
    int[] masts;
    int[] cannons;
    int crew;
    int treasure;
    int damage;
    public PlayerShip (int num, GameObject ship, int[] masts, int[] cannons, int crew, int treasure, int damage)
    {
        playerNum = num;
        this.ship = ship;
        shipName = ship.name;
        this.masts = masts;
        this.cannons = cannons;
        this.crew = crew;
        this.treasure = 100 * treasure;
        this.damage = damage;
        totalTreasure = 0;
        currTreasure = 0;
    }

    public int GetPlayerNum()
    {
        return playerNum;
    }

    public void SetTreasure(float amount)
    {
        currTreasure = amount;
    }

    public void AddTreasure(float amount)
    {
        currTreasure += amount;
    }

    public float GetTreasure()
    {
        return currTreasure;
    }

    public void DepositTreasure()
    {
        totalTreasure += currTreasure;

        currTreasure = 0;
    }

    public void SetCurrentPosition(Vector3Int pos)
    {
        currPosition = pos;
    }

    public void SetPreviousPosition(Vector3Int pos)
    {
        prevPosition = pos;
    }

    public GameObject GetShip()
    {
        return ship;
    }

    public string GetName()
    {
        return shipName;
    }

    public Vector3Int GetCurrentPosition()
    {
        return currPosition;
    }
}
