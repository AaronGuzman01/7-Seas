using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndReset : MonoBehaviour
{
    public void save()
    {
        if (diceNcupController.cups[0])//swabie
        {
            GameObject parent = GameObject.Find("NavBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.nav[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("NumMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.move[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("WindMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.wind[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("CreatureBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.creature[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }
        }
        if (diceNcupController.cups[1])//seaman
        {
            GameObject parent = GameObject.Find("NavBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.nav[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("NumMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.move[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("WindMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.wind[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("CreatureBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.creature[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }
        }
        if (diceNcupController.cups[2])//captain
        {
            GameObject parent = GameObject.Find("NavBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.nav[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("NumMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.move[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("WindMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.wind[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }

            parent = GameObject.Find("CreatureBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.creature[i] = System.Int32.Parse(parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text);
            }
        }
    }
    public void reset()
    {
        if (diceNcupController.cups[0])//swabie
        {
            GameObject parent = GameObject.Find("NavBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.nav[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.nav[i].ToString();
            }

            parent = GameObject.Find("NumMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.move[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.move[i].ToString();
            }

            parent = GameObject.Find("WindMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.wind[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.wind[i].ToString();
            }

            parent = GameObject.Find("CreatureBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Swabie.creature[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Swabie.creature[i].ToString();
            }
        }
        if (diceNcupController.cups[1])//seaman
        {
            GameObject parent = GameObject.Find("NavBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.nav[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.nav[i].ToString();
            }

            parent = GameObject.Find("NumMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.move[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.move[i].ToString();
            }

            parent = GameObject.Find("WindMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.wind[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.wind[i].ToString();
            }

            parent = GameObject.Find("CreatureBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Seaman.creature[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Seaman.creature[i].ToString();
            }
        }
        if (diceNcupController.cups[2])//captain
        {
            GameObject parent = GameObject.Find("NavBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.nav[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.nav[i].ToString();
            }

            parent = GameObject.Find("NumMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.move[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.move[i].ToString();
            }

            parent = GameObject.Find("WindMoveBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.wind[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.wind[i].ToString();
            }

            parent = GameObject.Find("CreatureBG");
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Captain.creature[i] = 1;
                parent.transform.GetChild(i).Find("NumberBG").GetChild(0).GetComponent<Text>().text = Captain.creature[i].ToString();
            }
        }
    }
}
