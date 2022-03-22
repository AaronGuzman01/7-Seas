using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctionality : MonoBehaviour
{
    public Slider SliderX, SliderY;
    public Button Fire, Left, Right, Ready;
    public GameObject FX;
    public GameObject[] Cannons;
    public Camera cameraOne;

    GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTutorialClick()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnSinglePirateClick()
    {
        SceneManager.LoadScene(1);
    }
    
    public void OnMultiPirateClick()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OnFightClick()
    {
        SceneManager.LoadScene("MultiPirateMap");
    }

    // loads main menu
    public void onMainMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    public void onOptionsClick()
    {
        SceneManager.LoadScene("Options");
    }

    public void OnReadyClick()
    {
        SliderX.interactable = true;
        SliderY.interactable = true;
        Fire.interactable = true;
        Left.interactable = true;
        Right.interactable = true;
        Ready.gameObject.SetActive(false);

        if (PlayerPrefs.GetString("Enemy").Equals("Player") || PlayerPrefs.GetString("Enemy").Equals("Treasure"))
        {
            enemy.GetComponent<ship_movement>().isMoving = true;
            enemy.GetComponent<ShipCombatTarget>().SetStart();
            FX.GetComponent<ship_movement>().isMoving = true;
        }
        else
        {
            enemy.GetComponent<Monstermovement>().isMoving = true;
        }
    }

    public void SetEnemy(GameObject setEnenmy)
    {
        enemy = setEnenmy;
    }

    public void OnExitGameClick()
    {
        Application.Quit();
    }

    public void DisableControls()
    {
        SliderX.interactable = false;
        SliderY.interactable = false;
        Fire.interactable = false;
        Left.interactable = false;
        Right.interactable = false;
        Ready.gameObject.SetActive(true);
        enemy.GetComponent<ship_movement>().isMoving = false;
        FX.GetComponent<ship_movement>().isMoving = false;
        foreach(GameObject cannon in Cannons)
        {
            cannon.GetComponent<Cannon_Firing>().Reload();
            cannon.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            for (int i = 0; i < 15; ++i)
            {
                cannon.GetComponent<Cannon_Firing>().cannonArray[i, 0] = .5f;
                cannon.GetComponent<Cannon_Firing>().cannonArray[i, 1] = .5f;
            }
        }
        SliderX.value = 0.5f;
        SliderY.value = 0.5f;
        cameraOne.GetComponent<switchCannon>().swapCameraOne();
    }

    public void DifficultyChange()
    {
        Slider slider = GameObject.Find("DifficultySlider").GetComponent<Slider>();
        Text difficulty = GameObject.Find("DifficultyText").GetComponent<Text>();
        if (slider.value == 0)
        {
            difficulty.text = "POWDER MONKEY";
            PlayerPrefs.SetFloat("DifficultySlider", slider.value);
            PlayerPrefs.SetString("Difficulty", difficulty.text);
        }
        else if (slider.value == 1)
        {
            difficulty.text = "BOATSWAIN";
            PlayerPrefs.SetFloat("DifficultySlider", slider.value);
            PlayerPrefs.SetString("Difficulty", difficulty.text);
        }
        else if (slider.value == 2)
        {
            difficulty.text = "QUARTERMASTER";
            PlayerPrefs.SetFloat("DifficultySlider", slider.value);
            PlayerPrefs.SetString("Difficulty", difficulty.text);
        }
        else if (slider.value == 3)
        {
            difficulty.text = "CAPTAIN";
            PlayerPrefs.SetFloat("DifficultySlider", slider.value);
            PlayerPrefs.SetString("Difficulty", difficulty.text);
        }
        PlayerPrefs.Save();
    }
}