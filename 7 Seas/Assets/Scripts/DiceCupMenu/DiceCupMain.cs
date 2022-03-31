//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;


//public class DiceCupMain : MonoBehaviour
//{
//    //public static DiceCupMain DCM;
//    public static List<Sprite> tarShipDice;
//    public static List<Sprite> moveNumDice;
//    public static List<Sprite> windMovDice;
//    public static List<Sprite> resourceDice;
//    public static List<Sprite> colorDice;
//    public static bool loaded = false;
//    public static bool swabie = false;
//    public static bool seaman = true;
//    public static bool captain = false;
//    public GameObject numText;

//    //soundfx
//    public AudioClip buttonClick;
//    private AudioSource source;

//    public List<Sprite> swabieTarShipDice;

//    public List<int> tarShipGold;
//    public Text franceText;
//    public Text englandText;
//    public Text hollandText;
//    public Text spainText;
//    public Text italyText;
//    public Text portugalText;

//    void Awake()
//    {
//        //if dice arent loaded yet
//        if (!loaded)
//        {
//            tarShipDice = new List<Sprite>();
//            moveNumDice = new List<Sprite>();
//            windMovDice = new List<Sprite>();
//            resourceDice = new List<Sprite>();
//            colorDice = new List<Sprite>();
//        }

//        //load dice arrays
//        LoadTarShipArray();
//        LoadNumMovArray();
//        LoadWindMovArray();
//        LoadResourceArray();
//        LoadColorArray();

        

//    }
//    private void Start()
//    {
//        //initialize sounds
//        source = GameObject.Find("ButtonClick").GetComponent<AudioSource>();
//        buttonClick = source.GetComponent<AudioClip>();

//        //find all buttons and load click sounds into them
//        LoadButtonClicks();
//        MatchTargetShipGold();
//    }

//    public void LoadButtonClicks()
//    {
//        //find all buttons
//        var buttons = GameObject.FindObjectsOfType<Button>();
//        foreach(var button in buttons)
//        {
//            //add click listener that plays sound
//            button.onClick.AddListener(TaskOnClick);
//        }
//    }
//    //click listener plays button click effect
//    void TaskOnClick()
//    {
//        source.Play();
//    }

//    //match targetship gold amounts from the dice cup to the ship game objects
//    public void MatchTargetShipGold()
//    {
//        tarShipGold = new List<int>(new int[6]);
//        tarShipGold[0] = int.Parse(franceText.text);
//        tarShipGold[1] = int.Parse(englandText.text);
//        tarShipGold[2] = int.Parse(hollandText.text);
//        tarShipGold[3] = int.Parse(spainText.text);
//        tarShipGold[4] = int.Parse(italyText.text);
//        tarShipGold[5] = int.Parse(portugalText.text);
//    }

//    public void LoadTarShipArray()
//    {
//        //empty list
//        tarShipDice.Clear();

//        //find all target ships
//        var tarShips = GameObject.FindGameObjectsWithTag("TargetShip");
//        foreach(var item in tarShips)
//        {

//            int num = 0;
//            if (item.GetComponentInChildren<Text>().name != "GhostText")
//            {
//                num = 1;
//            }
//            else
//            {
//                num = int.Parse(item.GetComponentInChildren<Text>().text);
//            }

//            //dice icon
//            var images = item.GetComponentsInChildren<Image>();

//            //add all ships to array
//            for(int i=0; i<num; ++i)
//            {
//                tarShipDice.Add(images[5].sprite);
//            }

//        }

//    }

//    public void LoadNumMovArray()
//    {
//        //empty list
//        moveNumDice.Clear();

//        //find all target ships
//        var numMov = GameObject.FindGameObjectsWithTag("NumMovDice");
//        foreach (var item in numMov)
//        {
//            //text number
//            int num = int.Parse(item.GetComponentInChildren<Text>().text);

//            //dice icon
//            var images = item.GetComponentsInChildren<Image>();

//            //add all ships to array
//            for (int i = 0; i < num; ++i)
//            {
//                moveNumDice.Add(images[5].sprite);
//            }

//        }

//    }

//    public void LoadWindMovArray()
//    {
//        //empty list
//        windMovDice.Clear();

//        //find all target ships
//        var windMov = GameObject.FindGameObjectsWithTag("WindMovDice");
//        foreach (var item in windMov)
//        {
//            //text number
//            int num = int.Parse(item.GetComponentInChildren<Text>().text);

//            //dice icon
//            var images = item.GetComponentsInChildren<Image>();

//            //add all ships to array
//            for (int i = 0; i < num; ++i)
//            {
//                windMovDice.Add(images[5].sprite);
//            }

//        }

//    }

//    public void LoadResourceArray()
//    {
//        //empty list
//        resourceDice.Clear();

//        //find all target ships
//        var resource = GameObject.FindGameObjectsWithTag("ResourceDice");
//        foreach (var item in resource)
//        {
//            //text number
//            int num = int.Parse(item.GetComponentInChildren<Text>().text);

//            //dice icon
//            var images = item.GetComponentsInChildren<Image>();

//            //add all ships to array
//            for (int i = 0; i < num; ++i)
//            {
//                resourceDice.Add(images[5].sprite);
//            }

//        }

//    }

//    public void LoadColorArray()
//    {
//        //empty list
//        colorDice.Clear();

//        //find all target ships
//        var colors = GameObject.FindGameObjectsWithTag("ColorDice");
//        foreach (var item in colors)
//        {
//            //text number
//            int num = int.Parse(item.GetComponentInChildren<Text>().text);

//            //dice icon
//            var images = item.GetComponentsInChildren<Image>();

//            //add all ships to array
//            for (int i = 0; i < num; ++i)
//            {
//                colorDice.Add(images[5].sprite);
//            }

//        }

//    }


//}
