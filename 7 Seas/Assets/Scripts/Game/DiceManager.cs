using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DiceManager : MonoBehaviour {
    public int num = 0;
    public int ghostDiceTotal = 0;

    public AudioSource cameraSource;
    public AudioSource diceSource;
    public AudioClip[] clips;
    public GameObject basicDiceSprite;
    public GameObject movementDiceSprite;
    public GameObject windDiceSprite;
    public GameObject rewardDiceSprite;
    public GameObject hazardDiceSprite;

    public GameObject navMenu;

    public InventorySystem inventorySystem;
    public GameLoop gameLoop;

    public List<Sprite> basicDiceArray;
    public List<Sprite> movementDiceArray;
    public List<Sprite> windDiceArray;
    public List<Sprite> rewardDiceArray;
    public List<Sprite> hazardDiceArray;

    public GameObject diceImage;
    public Texture diceDefaultTexture;
    public ParticleSystem explosion;
    public Canvas gameGUI;

    public GameObject diceList;
    public TimeOfDay timeOfDay;
    public GameObject fog;
    public bool fogDice = false;

    public LoseTurn loseTurn;

    public static bool diceStart = false;

    List<int> diceVals = new List<int>();

    int[] navDist;
    int[] moveDist;
    int[] windDist;
    int[] creatureDist;

    List<int> nav = new List<int>();
    List<int> move = new List<int>();
    List<int> wind = new List<int>();
    List<int> creature = new List<int>();

    // Use this for initialization
    void Awake()
    {
        diceList = GameObject.Find("DiceList");

        diceDefaultTexture = diceImage.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture;

        ResetDice();

        if (PlayerPrefs.GetString("Cup") == "Seaman")
        {
            navDist = Seaman.nav;
            moveDist = Seaman.nav;
            windDist = Seaman.nav;
            creatureDist = Seaman.nav;
        }
        else if (PlayerPrefs.GetString("Cup") == "Captain")
        {
            navDist = Captain.nav;
            moveDist = Captain.move;
            windDist = Captain.wind;
            creatureDist = Captain.creature;
        }
        else
        {
            navDist = Swabie.nav;
            moveDist = Swabie.move;
            windDist = Swabie.wind;
            creatureDist = Swabie.creature;
        }

        SetDistribution(navDist, nav);
        SetDistribution(moveDist, move);
        SetDistribution(windDist, wind);
        SetDistribution(creatureDist, creature);
    }
    
    void SetDistribution(int[] distribution, List<int> dice)
    {
        int count = 0;

        foreach (int amount in distribution)
        {
            for (int i = 0; i < amount; i++)
            {
                dice.Add(count);
            }

            count++;
        }
    }

    void ResetDice()
    {
        movementDiceSprite.SetActive(false);
        basicDiceSprite.SetActive(false);
        windDiceSprite.SetActive(false);
        rewardDiceSprite.SetActive(false);
        hazardDiceSprite.SetActive(false);
        navMenu.SetActive(false);
        diceVals.Clear();
        diceVals = new List<int>();
    }

    //roll the dice
    public void RollDice()
    {
        ResetDice();

        ghostDiceTotal = 0;

        diceImage.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = diceDefaultTexture;



        StartCoroutine(GetDiceValue(movementDiceArray, movementDiceSprite, 0));

        StartCoroutine(GetDiceValue(basicDiceArray, basicDiceSprite, 1));

        StartCoroutine(GetDiceValue(windDiceArray, windDiceSprite, 2));

        StartCoroutine(GetDiceValue(rewardDiceArray, rewardDiceSprite, 3));

        StartCoroutine(GetDiceValue(hazardDiceArray, hazardDiceSprite, 4));
    }

    public void SetNavigationMenu()
    {
        Text[] navTexts = navMenu.GetComponentsInChildren<Text>();

        if (diceVals[1] != -1)
        {
            navTexts[2].text = 1.ToString();
        }
        else
        {
            navTexts[2].text = 0.ToString();
        }

        if (diceVals[0] != -1)
        {
            navTexts[0].text = diceVals[0].ToString();
        }
        else
        {
            navTexts[0].text = 0.ToString();
        }

        if (diceVals[2] != -1 && diceVals[2] != 1)
        {
            navTexts[1].text = diceVals[2].ToString();
        }
        else
        {
            navTexts[1].text = 0.ToString();
        }

        navMenu.SetActive(true);
    }

    IEnumerator GetDiceValue(List<Sprite> faces, GameObject dice, int diceIndex)
    {
        yield return new WaitUntil(() => diceStart);

        diceStart = false;

        if (diceIndex == 0)
        {
            num = GetRandomFace(nav);
        }
        else if (diceIndex == 1)
        {
            num = GetRandomFace(move);
        }
        else if (diceIndex == 2)
        {
            num = GetRandomFace(wind);
        }
        else if (diceIndex == 4)
        {
            num = GetRandomFace(creature);
        }
        else
        {
            num = Random.Range(0, faces.Count);
        }

        if (num.Equals(faces.Count - 1) && diceIndex != 3)
        {
            MapLoad.diceVals[diceIndex] = -1;
            MapLoad.ghostCount++;
        }
        else if (num.Equals(faces.Count - 2) && diceIndex == 1)
        {
            MapLoad.diceVals[diceIndex] = -1;
            MapLoad.rats = true;
        }
        else if (num.Equals(faces.Count - 2) && diceIndex == 2)
        {
            MapLoad.diceVals[diceIndex] = -1;
            MapLoad.fog = true;
        }
        else if (num.Equals(faces.Count - 2) && diceIndex == 4)
        {
            MapLoad.diceVals[diceIndex] = -1;
            MapLoad.siren = true;
        }
        else
        {

            if (diceIndex > 1)
            {
                if (diceIndex == 3)
                {
                    MapLoad.reward = num + 1;
                }

                if (diceIndex == 4)
                {
                    MapLoad.hazard = 12 + num;
                }

                MapLoad.diceVals[diceIndex] = num + 1;
            }
            else
            {
                MapLoad.diceVals[diceIndex] = num + 2;
            }
        }

        if (diceIndex == 1)
        {
            if (MapLoad.diceVals[diceIndex] == -1)
            {
                diceVals.Add(-1);
            }
            else
            {
                diceVals.Add(1);
            }
        }
        else if (diceIndex == 0)
        {
            if (MapLoad.diceVals[diceIndex] == -1)
            {
                diceVals.Add(-1);
            }
            else
            {
                diceVals.Add(MapLoad.diceVals[diceIndex]);
            }
        }
        else if (diceIndex == 2)
        {
            if (MapLoad.diceVals[diceIndex] == -1)
            {
                diceVals.Add(-1);
            }
            else
            {
                diceVals.Add(MapLoad.diceVals[diceIndex]);
            }
        }

        dice.GetComponent<Image>().sprite = faces[num];

        diceImage.transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = faces[num].texture;

        dice.SetActive(true);

        if (num.Equals(faces.Count - 1))
        {
            ghostDiceTotal++;
        }

        explosion.gameObject.SetActive(true);

        explosion.Play();
        diceSource.PlayOneShot(clips[0], 0.3f);

        if (diceIndex == 4)
        {
            SetNavigationMenu();
            StartCoroutine(WaitForDice());
        }
    }

    IEnumerator WaitForDice()
    {
        yield return new WaitForSeconds(1);
        diceSource.Stop();
        cameraSource.UnPause();
    }

    int GetRandomFace(List<int> distribution)
    {
        int index = Random.Range(0, distribution.Count);

        return distribution[index];
    }

    public void PlayDiceMusic()
    {
        cameraSource.Pause();
        diceSource.Play();
    }

    }
