using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Captain : MonoBehaviour
{
    public static string cupName = "captain";
    public static int[] nav = { 1, 1, 1, 1, 1, 1 };
    public static int[] move = { 1, 1, 1, 1, 1, 1 };
    public static int[] wind = { 1, 1, 1, 1, 1, 1 };
    public static int[] creature = { 1, 1, 1, 1, 1, 1 };

    public static string toString()
    {
        return string.Join("", nav) + "\n" +
               string.Join("", move) + "\n" +
               string.Join("", wind) + "\n" +
               string.Join("", creature);
    }
    public static void load()
    {
        int lineNum = 0;
        foreach(string i in File.ReadLines(Application.persistentDataPath + "/Captain.txt"))
        {
            if (lineNum == 0)//load nav
                for (int j = 0; j < nav.Length; j++)
                    nav[j] = System.Int32.Parse(i[j].ToString());
            
            if (lineNum == 1)//load move
                for (int j = 0; j < move.Length; j++)
                    move[j] = System.Int32.Parse(i[j].ToString());

            if (lineNum == 2)//load wind
                for (int j = 0; j < wind.Length; j++)
                    wind[j] = System.Int32.Parse(i[j].ToString());

            if (lineNum == 3)//load creature
                for (int j = 0; j < creature.Length; j++)
                    creature[j] = System.Int32.Parse(i[j].ToString());
            lineNum++;
        }
    }
    public static void save()
    {
        File.WriteAllText(Application.persistentDataPath + "/Captain.txt", toString());
    }
}
