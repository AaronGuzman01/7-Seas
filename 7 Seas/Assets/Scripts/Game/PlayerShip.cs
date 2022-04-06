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
    Vector3Int portPosition;
    int[] masts;
    int[] cannons;
    int crew = 0;
    int rats = 0;
    int treasure = 1;
    int damage = 0;
    int hits = 1;
    int compass = 0;
    int baseNav = 0;
    bool boughtCrew = false;

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

    public void AddToTreasure(float amount)
    {
        if (currTreasure + amount > treasure)
        {
            currTreasure = treasure;
        }
        else
        {
            currTreasure += amount;
        }
    }

    public float GetCurrentTreasure()
    {
        return currTreasure;
    }

    public float GetTotalTreasure()
    {
        return totalTreasure;
    }

    public float GetTreasureLimit()
    {
        return treasure;
    }

    public void ClearTreasure()
    {
        currTreasure = 0;
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

    public void SetPortPosition(Vector3Int pos)
    {
        portPosition = pos;
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

    public Vector3Int GetPortPosition()
    {
        return portPosition;
    }

    public int[] GetCannons()
    {
        return cannons;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetHits()
    {
        return hits;
    }

    public int GetCompass()
    {
        return compass;
    }

    public int GetBase()
    {
        return baseNav;
    }

    public int GetCrew()
    {
        return crew;
    }

    public int GetRats()
    {
        return rats;
    }

    public void RemoveCrew()
    {
        crew--;
    }

    public void AddHits(int hits)
    {
        this.hits += hits;
    }

    public void AddCompass()
    {
        compass += 2;
    }

    public void AddBase()
    {
        baseNav += 2;
    }

    public void AddCombat()
    {
        hits += 2;
    }

    public bool AddLRCannon()
    {
        for (int i = 0; i < cannons.Length; i++)
        {
            if (cannons[i] == 0)
            {
                cannons[i] = 2;

                return true;
            }
        }

        for (int i = 0; i < cannons.Length; i++)
        {
            if (cannons[i] == 1)
            {
                cannons[i] = 2;

                return true;
            }
        }

        return false;
    }

    public void AddRat()
    {
        rats++;
    }

    public void ResetRats()
    {
        rats = 0;
    }

    public bool HasBought()
    {
        return boughtCrew;
    }

    public void SetCrewBought()
    {
        boughtCrew = true;
    }

    public void ResetCrew()
    {
        boughtCrew = false;
    }
}