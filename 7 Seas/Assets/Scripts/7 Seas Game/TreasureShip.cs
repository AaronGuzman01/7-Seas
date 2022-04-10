using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            Destroy(moving);

            explosion = Instantiate(explosion);
            explosion.transform.position = transform.position + new Vector3(1, 2, 0);
            explosion.Play();

            StartCoroutine(DestoryShip());

            setDestroy = true;
        }

    }

    IEnumerator DestoryShip()
    {
        destroyedShip.transform.position = transform.position + new Vector3(0, 0, -1.3f);

        yield return new WaitForSeconds(3f);

        destroyedShip.gameObject.SetActive(true);
        transform.position = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.5f);
        Destroy(explosion.gameObject);

        yield return new WaitForSeconds(5f);

        SceneManager.UnloadSceneAsync("Cannon");
        SceneManager.LoadScene("CannonResults", LoadSceneMode.Additive);
    }
}
