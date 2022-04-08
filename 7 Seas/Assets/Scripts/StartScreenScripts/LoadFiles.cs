using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadFiles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string defaultMapPath = Application.persistentDataPath + "/Maps/Default";
        string customMapPath = Application.persistentDataPath + "/Maps/Custom";

        Directory.CreateDirectory(defaultMapPath);
        Directory.CreateDirectory(customMapPath);

        SetupMenu.ResetSetup();

        string[] players = { "f", "f", "f", "f", "f", "f", "f", "f" };
        string[] mast = { "mast 1:", "mast 2:", "mast 3:" };
        string[] cannon = { "cannon 1: s", "cannon 2:", "cannon 3:", "cannon 4:", "cannon 5:" };
        string[] crew = { "crew 1:", "crew 2:" };
        string[] treasure = { "treasure 1: t", "treasure 2:", "treasure 3:" };
        string[] damage = { "damage 1:", "damage 2:" };


        if (!File.Exists(Application.persistentDataPath + "/Swabie.txt"))
            File.WriteAllText(Application.persistentDataPath + "/Swabie.txt", Swabie.toString());
        else
            Swabie.load();

        if (!File.Exists(Application.persistentDataPath + "/Captain.txt"))
            File.WriteAllText(Application.persistentDataPath + "/Captain.txt", Captain.toString());
        else
            Captain.load();

        if (!File.Exists(Application.persistentDataPath + "/Seaman.txt"))
            File.WriteAllText(Application.persistentDataPath + "/Seaman.txt", Seaman.toString());
        else
            Seaman.load();    

        
        File.WriteAllText(Application.persistentDataPath + "/Players.txt", string.Join("\n", players));

        if (!File.Exists(Application.persistentDataPath + "/Difficulty.txt"))
        {
            File.WriteAllText(Application.persistentDataPath + "/Difficulty.txt", "200");
            PlayerPrefs.SetString("Difficulty", "Easy");
        }
        else
        {
            foreach(string points in File.ReadLines(Application.persistentDataPath + "/Difficulty.txt"))
            {
                if (points.Equals("200"))
                    PlayerPrefs.SetString("Difficulty", "Easy");
                else if (points.Equals("150"))
                    PlayerPrefs.SetString("Difficulty", "Normal");
                else if (points.Equals("100"))
                    PlayerPrefs.SetString("Difficulty", "Hard");
            }
            

        }
        
        for (int i = 1; i <= 8; i++)
        {
            if(!File.Exists(Application.persistentDataPath + "/Player" + i.ToString() + ".txt"))
                File.WriteAllText(Application.persistentDataPath + "/Player" + i.ToString() + ".txt", string.Join("\n", mast) + "\n" +
                                                                                                      string.Join("\n", cannon) + "\n" +
                                                                                                      string.Join("\n", crew) + "\n" +
                                                                                                      string.Join("\n", treasure) + "\n" +
                                                                                                      string.Join("\n", damage));
        }

        var textFile = Resources.Load<TextAsset>("Sea of Reward");

        File.WriteAllText(defaultMapPath + "/Sea of Reward.txt", textFile.text);
    }
}
