using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonMinigame : MonoBehaviour
{
    public AudioSource cannonAudio;
    public AudioClip[] clips;
    public static PlayerShip[] shipsInfo = new PlayerShip[2];
    public static GameObject[] ships = new GameObject[2];
    public static int currShip = 1;
    public Material[] skyBox;
    public GameObject ButtonManager;
    public GameObject SceneManager;
    public GameObject[] targets;
    public GameObject treasureShip;
    public GameObject destroyedShip;
    public ParticleSystem explosion;
    public GameObject moving;
    public Slider healthSlider;
    public Color healthColor;

    public static bool setPlayer;
    public static bool setTreasure;
    public static bool setMonster;

    public Text damage;
    

    static GameObject currTreasureShip;
    static GameObject monster;
    GameObject target;
    int skyIndex = 0;

    void Start()
    {
        PlayerPrefs.SetInt("Treasure Score", 0);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("DamageDoneMonster", 0);
    }

    void Update()
    {
        RenderSettings.skybox = skyBox[skyIndex];

        if (setPlayer && currShip == 1)
        {
            PlayerPrefs.SetInt("Player1Score", 0);

            setPlayer = false;

            currShip++;

            target = Instantiate(targets[0], ships[1].transform.GetChild(0));

            target.SetActive(true);

            ships[1].AddComponent<ship_movement>();
            ships[1].GetComponent<ship_movement>().height = 1010;

            ships[0].SetActive(false);

            scoreUpdate.absorb = shipsInfo[1].GetDamage();
            PlayerPrefs.SetString("Enemy", "Player");

            ButtonManager.GetComponent<ButtonFunctionality>().SetEnemy(ships[1]);

            RenderSettings.skybox = skyBox[0];

            cannonAudio.clip = clips[0];
            cannonAudio.Play();
        }
        else if (setPlayer && currShip == 2)
        {
            PlayerPrefs.SetInt("Player2Score", 0);

            setPlayer = false;

            currShip++;

            target = Instantiate(targets[0], ships[0].transform.GetChild(0));

            target.SetActive(true);

            ships[0].AddComponent<ship_movement>();
            ships[0].GetComponent<ship_movement>().height = 1010;

            scoreUpdate.absorb = shipsInfo[0].GetDamage();

            ButtonManager.GetComponent<ButtonFunctionality>().SetEnemy(ships[0]);

            RenderSettings.skybox = skyBox[0];

            cannonAudio.clip = clips[0];
            cannonAudio.Play();
        }
        else if (setTreasure)
        {
            PlayerPrefs.SetInt("ShipHits", currTreasureShip.GetComponent<TreasureShip>().hits);
            PlayerPrefs.SetInt("Gold", currTreasureShip.GetComponent<TreasureShip>().gold);
            PlayerPrefs.SetInt("PlayerHits", shipsInfo[0].GetHits());

            setTreasure = false;

            target = Instantiate(targets[2], currTreasureShip.transform);

            target.SetActive(true);

            currTreasureShip.AddComponent<ship_movement>();
            currTreasureShip.GetComponent<ship_movement>().height = 1010;
            currTreasureShip.GetComponent<TreasureShip>().destroyedShip = destroyedShip;
            currTreasureShip.GetComponent<TreasureShip>().explosion = explosion;
            currTreasureShip.GetComponent<TreasureShip>().moving = moving;
            currTreasureShip.GetComponent<TreasureShip>().clone = true;

            currTreasureShip.SetActive(true);

            ButtonManager.GetComponent<ButtonFunctionality>().SetEnemy(currTreasureShip);

            if (shipsInfo[0].GetHits() > 1)
            {
                damage.text = "+ " + (shipsInfo[0].GetHits() - 1).ToString() + " Hits";
            }

            RenderSettings.skybox = skyBox[0];

            cannonAudio.clip = clips[1];
            cannonAudio.Play();
        }
        else
        {
            if (setMonster)
            {
                PlayerPrefs.SetInt("PlayerHits", shipsInfo[0].GetHits());

                setMonster = false;

                healthSlider.fillRect.GetComponent<Image>().color = healthColor;
                healthSlider.gameObject.SetActive(true);

                monster.SetActive(true);

                target = Instantiate(targets[1], monster.transform);

                target.SetActive(true);

                target.transform.RotateAround(monster.transform.position, Vector3.up, -90f);
                target.transform.localScale = new Vector3(1, 2, 1);

                monster.GetComponent<Monstermovement>().enabled = true;
                monster.GetComponent<Monstermovement>().anim = monster.GetComponent<Animation>();
                monster.GetComponent<Monstermovement>().slider = healthSlider;

                ButtonManager.GetComponent<ButtonFunctionality>().SetEnemy(monster);

                if (shipsInfo[0].GetHits() > 1)
                {
                    damage.text = "+ " + (shipsInfo[0].GetHits() - 1).ToString() + " Hits";
                }

                RenderSettings.skybox = skyBox[1];

                cannonAudio.clip = clips[2];
                cannonAudio.Play();

                skyIndex = 1;
            }
        }
    }

    public static void SetShips()
    {
        ships[0] = Instantiate(shipsInfo[0].GetShip());
        ships[1] = Instantiate(shipsInfo[1].GetShip());

        ships[0].transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);
        ships[1].transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);

        ships[0].transform.position = new Vector3(0, 1010, 0);
        ships[1].transform.position = new Vector3(0, 1010, 0);
    }

    public static void SetTreasureShip(GameObject gameShip)
    {
        currTreasureShip = Instantiate(gameShip);

        currTreasureShip.transform.GetChild(0).transform.rotation = Quaternion.Euler(Vector3.zero);

        currTreasureShip.transform.position = new Vector3(0, 1010, 0);

        Destroy(gameShip);
    }

    public static void SetMonster(GameObject gameMonster)
    {
        monster = Instantiate(gameMonster);

        monster.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

        monster.transform.position = new Vector3(-50, 1010, 15);
    }

    public static void DestroyObjects()
    {
        Destroy(ships[0]);
        Destroy(ships[1]);
        Destroy(currTreasureShip);
        Destroy(monster);
    }

    public static void ChangeShips()
    {
        Destroy(ships[1]);

        ships[0].SetActive(true);
    }

    public void CheckEmptyHealthBar()
    {
        if (healthSlider.value == 0)
        {
            Color color = healthSlider.fillRect.GetComponent<Image>().color;
            color = new Color(color.r, color.g, color.b, 0);
        }
    }
}
