using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public static PlayerShip player;
    public GameObject[] ships;
    public Text winner;
    public Text gold;
    public AudioSource[] fireworks;

    // Start is called before the first frame update
    void Start()
    {
        fireworks[0].PlayDelayed(3.5f);
        fireworks[1].PlayDelayed(3.5f);
        fireworks[2].PlayDelayed(3.5f);

        GameObject ship = Instantiate(FindShip());

        ship.transform.position = new Vector3(0, 0, 0);
        ship.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, -90, 0));

        winner.text = "Player " + player.GetPlayerNum().ToString() + " is the Winner!";
        gold.text = "Treasure Collected: " + player.GetTotalTreasure().ToString();
    }

    GameObject FindShip()
    {
        foreach(GameObject ship in ships)
        {
            if (ship.name == player.GetName())
            {
                return ship;
            }
        }

        return null;
    }
}
