using System.Collections;
using UnityEngine;

public class MovePirate : MonoBehaviour
{
    public Transform ship;
    public GameObject[] ships;
    public int staticShipIndex;
    private Vector3 origPos;
    private bool wait = false;
    private bool set = false;
    private bool firstPass = true;

    // Start is called before the first frame update
    void Start()
    {
        origPos = new Vector3(-11.2f, -0.12f, -26.3f);

        StartCoroutine(SetShip());
    }

    // Update is called once per frame
    void Update()
    {
        if (set)
        {
            if (ship.position.z >= 15 && !wait)
            {
                StartCoroutine(SetShip());
            }
            else
            {
                ship.position = new Vector3(ship.position.x + 0.02f, ship.position.y, ship.position.z + 0.02f);
            }
        }
    }

    private IEnumerator SetShip()
    {
        set = false;

        yield return new WaitUntil(() => ships.Length > 0);

        if (this.ship)
        {
            yield return new WaitUntil(() => this.ship.position.z >= 15);
            Destroy(this.ship.gameObject);
        }

        if (!firstPass)
        {
            yield return new WaitForSeconds(5);
        }

        int num2 = Random.Range(0, 8);

        while (num2 == staticShipIndex)
        {
            num2 = Random.Range(0, 8);
        }

        GameObject ship = Instantiate(ships[num2]);

        ship.transform.position = origPos;
        ship.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 40f, 0));
        ship.transform.localScale = new Vector3(1, 1, 1);

        this.ship = ship.transform;

        set = true;
        wait = false;
        firstPass = false;
    }
}
