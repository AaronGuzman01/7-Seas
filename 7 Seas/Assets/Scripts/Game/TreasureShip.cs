using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureShip : MonoBehaviour
{
    public string nation;
    public int gold;
    public int hits;
    public bool clone = false;
    public GameObject destroyedShip;
    public ParticleSystem explosion;
    public GameObject moving;
    bool setDestroy = false;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("ShipHits") <= 0 && !setDestroy && clone)
        {
            GetComponent<ship_movement>().isMoving = false;
            explosion = Instantiate(explosion);
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            Destroy(moving);

            explosion.transform.position = transform.position + new Vector3(1.5f, 2, 1);

            explosion.Play();

            destroyedShip.transform.position = transform.position;
            Destroy(transform.gameObject);

            destroyedShip.gameObject.SetActive(true);

            setDestroy = true;
        }
    }
}
