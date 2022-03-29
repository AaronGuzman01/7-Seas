using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string level;
    public Text objective;
    public Text score;
    //public static List<PlayerShip> players;
    public GameObject FXEnvironment;
    public GameObject ButtonManager;
    public GameObject PlayerManager;
    private bool PlayersChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        //int gamesPlayed = PlayerPrefs.GetInt("ConsecutiveGamesPlayed");

        //PlayerPrefs.SetInt("ConsecutiveGamesPlayed", gamesPlayed + 1);
        PlayerPrefs.Save();

        //if (SceneManager.GetActiveScene().name == "SinglePirate")
        //{
            //StartCoroutine(LoadLevelAfterTime(35f, level));
        //}
        //else
        if (PlayerPrefs.GetString("Enemy").Equals("Player"))
        {
            objective.text = PlayerPrefs.GetString("Ship1").ToUpper() + " - SHOOT THE TARGET TO DO DAMAGE TO " + PlayerPrefs.GetString("Ship2").ToUpper() + "!";
        }
        else if (PlayerPrefs.GetString("Enemy").Equals("Treasure"))
        {
            
            //The code below is for setting the current player (change with when map is up)(Later 3/27/20)
            PlayerManager = PlayersManager.GetPlayerManager();
            // PlayersManager.SetPlayer();//comment out
            //players = PlayersManager.GetPlayers();
            //objective.text = PlayersManager.Opponent1.Name.ToUpper() + "  - AIM FOR THE COIN TO GET SOME SWEET, SWEET BOOTY!";
            objective.text = PlayerPrefs.GetString("Ship1").ToUpper() + "  - AIM FOR THE COIN TO GET SOME SWEET, SWEET BOOTY!";
        }
        else
        {
            PlayerManager = PlayersManager.GetPlayerManager();
            // PlayersManager.SetPlayer();//comment out
            //players = PlayersManager.GetPlayers();
            //objective.text = PlayersManager.Opponent1.Name.ToUpper() + " - SHOOT THE MONSTER TO DO DAMAGE. STRIKE FIRST TO AVOID DAMAGE!";
            objective.text = PlayerPrefs.GetString("Ship1").ToUpper() + " - SHOOT THE TARGET TO DAMAGE THE MONSTER. KILL IT FAST TO AVOID DAMAGE!";
            score.enabled = false;
        }
        /*
        else
        {
            //Threw these in here so that single pirate would work
            PlayerManager = PlayersManager.GetPlayerManager();
            //players = PlayersManager.GetPlayers();
            //objective.text = PlayersManager.Opponent1.Name.ToUpper() + " - SHOOT THE TARGETS TO DO DAMAGE TO " + PlayersManager.Opponent2.Name.ToUpper() + "!";
            //had to comment this part out for testing(testing 3/27/20)
            objective.text = PlayerPrefs.GetString("Ship1").ToUpper() + " - SHOOT THE TARGETS TO DO DAMAGE TO " + PlayerPrefs.GetString("Ship2").ToUpper() + "!";
            ChangeShips();
            ButtonManager.GetComponent<ButtonFunctionality>().DisableControls();
        }
        */
    }

    void ChangeShips()
    {
        string ship1 = PlayerPrefs.GetString("Ship1");
        string ship2 = PlayerPrefs.GetString("Ship2");

        PlayerPrefs.SetString("Ship1", ship2);
        PlayerPrefs.SetString("Ship2", ship1);

        /*
        if (opponent.Team == 1)
        {
            RedShip.SetActive(false);
            BlueShip.SetActive(true);
            BlueShip.transform.position = new Vector3(0f, 0f, 10f);
            
            ButtonManager.GetComponent<ButtonFunctionality>().SetEnemy();
        }
        else
        {
            RedShip.SetActive(true);
            BlueShip.SetActive(false);
            RedShip.transform.position = new Vector3(0f, 0f, 10f);
            ButtonManager.GetComponent<ButtonFunctionality>().enemy = RedShip;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneChangerReady()
    {
        if (PlayerPrefs.GetString("Enemy").Equals("Player"))
        {
            StartCoroutine(LoadLevelAfterTime(35f, "Cannon"));
            objective.text = "";
        }
        else if (PlayerPrefs.GetString("Enemy").Equals("Monster"))
        {
            StartCoroutine(LoadLevelAfterTime(60f, level));
            objective.text = "";
        }
        else
        {
            StartCoroutine(LoadLevelAfterTime(35f, level));
            objective.text = "";
        }
    }

    /*
    IEnumerator ChangePlayers(float timer)
    {
        yield return new WaitForSeconds(timer);
        PlayersChanged = true;
        string tempScore = score.text;
        tempScore = tempScore.Replace("DAMAGE DEALT: ", "");
        PlayerPrefs.SetInt("Player1Score", Convert.ToInt32(tempScore));
        PointsManager.ResetScore();
        FXEnvironment.transform.position = new Vector3(0f, 0f, 10f);
        objective.text = PlayersManager.Opponent2.Name.ToUpper() + " - SHOOT THE TARGETS TO DO DAMAGE TO " + PlayersManager.Opponent1.Name.ToUpper() + "!";
        ButtonManager.GetComponent<ButtonFunctionality>().DisableControls();
        PlayerPrefs.Save();
        ChangeShips();
        //Need to reload each gun or just reset scene
    }
    */

    IEnumerator LoadLevelAfterTime(float timer, string level)
    {
        yield return new WaitForSeconds(timer);
        if (PlayerPrefs.GetString("Enemy").Equals("Player"))
        {
            string tempScore = score.text;
            tempScore = tempScore.Replace("DAMAGE DEALT: ", "");
            ChangeShips();
            PointsManager.ResetScore();

            if (CannonMinigame.currShip - 1 == 1)
            {
                CannonMinigame.setPlayer = true;
                CannonMinigame.ChangeShips();

                PlayerPrefs.SetInt("Player1Score", Convert.ToInt32(tempScore));

                SceneManager.UnloadSceneAsync("Cannon");
                SceneManager.LoadScene(level, LoadSceneMode.Additive);
            }
            else
            {
                CannonMinigame.DestroyObjects();

                PlayerPrefs.SetInt("Player2Score", Convert.ToInt32(tempScore));

                SceneManager.UnloadSceneAsync("Cannon");
                SceneManager.LoadScene(this.level, LoadSceneMode.Additive);
            }
            /*
            string tempScore = score.text;
            tempScore = tempScore.Replace("DAMAGE DEALT: ", "");
            PlayerPrefs.SetInt("Player2Score", Convert.ToInt32(tempScore));
            */
        }
        else 
        {
            CannonMinigame.DestroyObjects();

            SceneManager.UnloadSceneAsync("Cannon");
            SceneManager.LoadScene(level, LoadSceneMode.Additive);
        }
        /*
        if (PlayerPrefs.GetInt("ConsecutiveGamesPlayed") >= 2)
        {
            GameObject UnityAdsManager = GameObject.Find("UnityAdsManager");
            UnityAdsManager AdsScript = UnityAdsManager.GetComponent<UnityAdsManager>();
            AdsScript.PlayInterstitialAd();
            AudioListener.volume = 0f;
            PlayerPrefs.SetInt("ConsecutiveGamesPlayed", 0);
            PlayerPrefs.Save();
        }
        */
    }
}
