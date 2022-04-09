using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class SetupMenu : MonoBehaviour {

    public Button btn;
    public Sprite btnPressed;
    public Sprite btnNotPressed;
    private static int num = 0;
    public Button difficultyBtn;
    private static Button selectedDifficulty;
    private bool isPressed;
    public  static bool resetSetup = false;
    public GameLoop gameLoop;
    private static string[] players = { "f", "f", "f", "f", "f", "f", "f", "f" };
    public AudioClip audioclip;
    //public static Button PlayBtn;

    // Use this for initialization
    void Start () {
        Screen.orientation = ScreenOrientation.Landscape;
        Debug.Log(PlayerPrefs.GetString("Difficulty"));
        if (resetSetup && btn != null)
        {
            PlayerPrefs.SetInt(btn.name, 0);
            PlayerPrefs.SetFloat("End", 0f);
        }

        if (btn != null && PlayerPrefs.GetInt(btn.name).Equals(1))
        {
            if (!isPressed)
            {
                btnPress();
            }
        }

        GameObject.Find("PlayGameButton").GetComponent<Button>().interactable = false;

        if (difficultyBtn != null)
        {
            if (PlayerPrefs.GetString("Difficulty").Equals("Easy") && difficultyBtn.name.Equals("EasyButton")) //&& !resetSetup)
            {
                SelectDifficulty();
            }
            else if (PlayerPrefs.GetString("Difficulty").Equals("Normal") && difficultyBtn.name.Equals("NormalButton"))// && !resetSetup)
            {
                SelectDifficulty();
            }
            else if (PlayerPrefs.GetString("Difficulty").Equals("Hard") && difficultyBtn.name.Equals("HardButton"))// && !resetSetup)
            {
                SelectDifficulty();
            }
        }
        //loadPlayers();
        /*
        PlayerPrefs.SetString("wheelSpun", "false");
        isPressed = false;

        if (btn != null)
        {
            btn = btn.GetComponent<Button>();
            loadPlayers();
        }

        //initialize this player as selected false
        PlayerPrefs.SetInt(this.name, 0);

        //initialize player pref values to zero each game
        PlayerPrefs.SetInt("TreasureSet", 0);
        PlayerPrefs.SetInt("TreasureAmount", 0);
        //PlayerPrefs.SetInt("PlayerCount", 0);
        //PlayerPrefs.SetString("Difficulty", "Seaman");

        PlayerPrefs.SetString("cannonVisited", "false");


        //determine whether we have a game to load or not
        if (PlayerPrefs.GetInt("LoadGame") == 0)
        {
            GameObject.Find("LoadGameButton").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("LoadGameButton").GetComponent<Button>().interactable = false;
        }
        if (difficultyBtn != null)
           loadDifficulty();
        */
    }

    private void Update()
    {
        if (num > 0 && selectedDifficulty != null)
            GameObject.Find("PlayGameButton").GetComponent<Button>().interactable = true;
        else
            GameObject.Find("PlayGameButton").GetComponent<Button>().interactable = false;

        if (resetSetup)
        {
            resetSetup = false;
        }
    }

    void loadDifficulty()
    {
        StreamReader rdr = new StreamReader(new FileStream(Application.persistentDataPath + "/Difficulty.txt", FileMode.Open, FileAccess.Read));
        string line = rdr.ReadLine();
        rdr.Close();
        if (line != null)
        {
            if (line.Equals("200") && difficultyBtn.name.Equals("EasyButton"))
                SelectDifficulty();
            if (line.Equals("150") && difficultyBtn.name.Equals("NormalButton"))
                SelectDifficulty();
            if (line.Equals("100") && difficultyBtn.name.Equals("HardButton"))
                SelectDifficulty();
        }
    }
    void loadPlayers()
    {
        int i = 0;
        foreach(string line in System.IO.File.ReadLines(Application.persistentDataPath + "/Players.txt"))
        {
            players[i] = line;
            i++;
        }

        if (btn.name.Equals("Player1"))
            if (players[0].Equals("t"))
            {
                btnPress();
            }

        if (btn.name.Equals("Player2"))
            if (players[1].Equals("t"))
            {
                btnPress();
            }

        if (btn.name.Equals("Player3"))
            if (players[2].Equals("t"))
            {
                btnPress();
            }

        if (btn.name.Equals("Player4"))
            if (players[3].Equals("t"))
            {
                btnPress();
            }
        if (btn.name.Equals("Player5"))
            if (players[4].Equals("t"))
            {
                btnPress();
            }

        if (btn.name.Equals("Player6"))
            if (players[5].Equals("t"))
            {
                btnPress();
            }

        if (btn.name.Equals("Player7"))
            if (players[6].Equals("t"))
            {
                btnPress();
            }

        if (btn.name.Equals("Player8"))
            if (players[7].Equals("t"))
            {
                btnPress();
            }
    }

    public void btnPress()
    {
        //toggle button press
        isPressed = !isPressed;
        
        
        //if is pressed toggle graphic and add player to list
        if (isPressed)
        {
            btn.image.overrideSprite = btnPressed;
            ++num;
            PlayerPrefs.SetInt(btn.name, 1);
            writePlayer("t");
        }
        //if is not pressed toggle graphic back and remove player from list
        else if (!isPressed)
        {
            btn.image.overrideSprite = btnNotPressed;
            --num;
            PlayerPrefs.SetInt(btn.name, 0);
            writePlayer("f");
        }
        
   

    }
    void writePlayer(string tORf)
    {
        if (btn.name.Equals("Player1"))
            players[0] = tORf;

        if (btn.name.Equals("Player2"))
            players[1] = tORf;

        if (btn.name.Equals("Player3"))
            players[2] = tORf;

        if (btn.name.Equals("Player4"))
            players[3] = tORf;

        if (btn.name.Equals("Player5"))
            players[4] = tORf;

        if (btn.name.Equals("Player6"))
            players[5] = tORf;

        if (btn.name.Equals("Player7"))
            players[6] = tORf;

        if (btn.name.Equals("Player8"))
            players[7] = tORf;
       
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Players.txt", string.Join("\n", players));
        
    }

    public void SelectDifficulty()
    {
        if (selectedDifficulty != null)
        {
            if (selectedDifficulty.Equals(difficultyBtn))
            {
                System.IO.File.WriteAllText(Application.persistentDataPath + "/Difficulty.txt", "");
                Color fixedColor = difficultyBtn.image.color;
                fixedColor.a = 0f;
                difficultyBtn.image.color = fixedColor;
                selectedDifficulty = null;
            }
        }
        else
        {
            if (difficultyBtn.name.Equals("EasyButton"))
            {
                System.IO.File.WriteAllText(Application.persistentDataPath + "/Difficulty.txt", "200");
                PlayerPrefs.SetString("Difficulty", "Easy");
                Color fixedColor = difficultyBtn.image.color;
                fixedColor.a = 0.175f;
                difficultyBtn.image.color = fixedColor;
                selectedDifficulty = difficultyBtn;
            }
            if (difficultyBtn.name.Equals("NormalButton"))
            {
                System.IO.File.WriteAllText(Application.persistentDataPath + "/Difficulty.txt", "150");
                PlayerPrefs.SetString("Difficulty", "Normal");
                Color fixedColor = difficultyBtn.image.color;
                fixedColor.a = 0.175f;
                difficultyBtn.image.color = fixedColor;
                selectedDifficulty = difficultyBtn;
            }
            if (difficultyBtn.name.Equals("HardButton"))
            {
                System.IO.File.WriteAllText(Application.persistentDataPath + "/Difficulty.txt", "100");
                PlayerPrefs.SetString("Difficulty", "Hard");
                Color fixedColor = difficultyBtn.image.color;
                fixedColor.a = 0.175f;
                difficultyBtn.image.color = fixedColor;
                selectedDifficulty = difficultyBtn;
            }
        }
    }
    //load game button
    public void LoadSavedGame()
    {
        //if a save exists, then allow the load button to work
        if (ES2.Exists(Application.persistentDataPath + "/saveExists"))
        {
            PlayerPrefs.SetInt("LoadGame", 1);
            SceneManager.LoadScene(2);
        }
    }

    public static void SetEndGame(int difficulty)
    {
        if (difficulty == 0)
        {
            PlayerPrefs.SetFloat("End", 2500f);
        }
        else if (difficulty == 1)
        {
            PlayerPrefs.SetFloat("End", 5000f);
        }
        else
        {
            PlayerPrefs.SetFloat("End", 10000f);
        }
    }

    public static void ResetSetup()
    {
        Debug.Log("Reset");
        resetSetup = true;
    }
}
