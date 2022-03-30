using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class diceButtons : MonoBehaviour
{
    public Text textBox;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void increase()
    {
        int num = System.Int32.Parse(textBox.GetComponent<Text>().text);

        if (num < 6)
            num++;

        textBox.GetComponent<Text>().text = num.ToString();
    }
    public void decrease()
    {
        int num = System.Int32.Parse(textBox.GetComponent<Text>().text);

        if (!(num <= 1))
            num--;
        textBox.GetComponent<Text>().text = num.ToString();
    }
}