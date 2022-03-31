using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class diceNcupController : MonoBehaviour
{
    //swabie, seaman, capitain
    public static bool[] cups = { false, false, false };
    public Sprite swabieOn;
    public Sprite swabieOff;
    public Sprite seamanOn;
    public Sprite seamanOff;
    public Sprite captainOn;
    public Sprite captainOff;
    public Button cupBtn;
    
    private void Start()
    {
        string cup = PlayerPrefs.GetString("Cup");
        Debug.Log(cup);

        if (cup.Equals("Swabie")) {
            Debug.Log("loading Swabie");
            GameObject.Find("Swabie").GetComponent<Image>().sprite = swabieOn;
            cups[0] = true;
            loadSwabie();
        }

        if (cup.Equals("Seaman")) {
            Debug.Log("loading Seaman");
            GameObject.Find("Seaman").GetComponent<Image>().sprite = seamanOn;
            cups[1] = true;
            loadSeaman();
        }
        if (cup.Equals("Captain")) {
            Debug.Log("loading Capitain");
            GameObject.Find("Captain").GetComponent<Image>().sprite = captainOn;
            cups[2] = true;
            loadCaptain();
        }
    }
    private void loadSwabie()
    {
        GameObject parent = GameObject.Find("NavBG");
        for(int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.nav[i].ToString();
        }

        parent = GameObject.Find("NumMoveBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.move[i].ToString();
        }

        parent = GameObject.Find("WindMoveBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.wind[i].ToString();
        }

        parent = GameObject.Find("CreatureBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.creature[i].ToString();
        }
    }
    private void loadSeaman()
    {
        GameObject parent = GameObject.Find("NavBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.nav[i].ToString();
        }

        parent = GameObject.Find("NumMoveBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.move[i].ToString();
        }

        parent = GameObject.Find("WindMoveBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.wind[i].ToString();
        }

        parent = GameObject.Find("CreatureBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.creature[i].ToString();
        }
    }
    private void loadCaptain()
    {
        GameObject parent = GameObject.Find("NavBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.nav[i].ToString();
        }

        parent = GameObject.Find("NumMoveBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.move[i].ToString();
        }

        parent = GameObject.Find("WindMoveBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.wind[i].ToString();
        }

        parent = GameObject.Find("CreatureBG");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.creature[i].ToString();
        }
    }
    public void selctCup()
    {
        //deselect prev cup
        if (cups[0])
        {
            GameObject.Find("Swabie").GetComponent<Image>().sprite = swabieOff;
            cups[0] = false;
        }
        if (cups[1])
        {
            GameObject.Find("Seaman").GetComponent<Image>().sprite = seamanOff;
            cups[1] = false;
        }
        if (cups[2])
        {
            GameObject.Find("Captain").GetComponent<Image>().sprite = captainOff;
            cups[2] = false;
        }

        //select new cup and load saved components
        if (cupBtn.name.Equals("Swabie"))
        {
            GameObject.Find("Swabie").GetComponent<Image>().sprite = swabieOn;
            cups[0] = true;
            PlayerPrefs.SetString("Cup", "Swabie");
            loadSwabie();
        }
        if (cupBtn.name.Equals("Seaman"))
        {
            GameObject.Find("Seaman").GetComponent<Image>().sprite = seamanOn;
            cups[1] = true;
            PlayerPrefs.SetString("Cup", "Seaman");
            loadSeaman();
        }
        if (cupBtn.name.Equals("Captain"))
        {
            GameObject.Find("Captain").GetComponent<Image>().sprite = captainOn;
            cups[2] = true;
            PlayerPrefs.SetString("Cup", "Captain");
            loadCaptain();
        }
    }
}
