using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShipCustomizationBtns : MonoBehaviour
{
    // Start is called before the first frame update
    public Button emptyMastBtn;
    public Sprite[] mastImages;

    public Button emptyCannonBtn;
    public Sprite[] cannonImages;

    public Button crewBtn;
    public Button treasureBtn;
    public Button damageBtn;

    private static string[] mast = {"mast 1:", "mast 2:", "mast 3:"};
    private static string[] cannon = {"cannon 1:", "cannon 2:", "cannon 3:", "cannon 4:", "cannon 5:"};
    private static string[] crew = {"crew 1:", "crew 2:"};
    private static string[] treasure = {"treasure 1:", "treasure 2:", "treasure 3:"};
    private static string[] damage = { "damage 1:", "damage 2:" };

    private static bool readingPlayerFile = false;
    private static bool ranSavedChoices = false;
    private static int points;
    void Start()
    {
        if (!ranSavedChoices)
        {
            ranSavedChoices = true;
            fillSavedChoices();
        }        
    }

    private void Update()
    {
        if (points < 0)
        {
            GameObject.Find("coins").GetComponent<Text>().color = Color.red;
            GameObject.Find("save").GetComponent<Button>().enabled = false;
            GameObject.Find("save").GetComponent<Image>().enabled = true;
            GameObject.Find("save").GetComponent<Button>().GetComponentInChildren<Text>().text = "OUT OF POINTS";
        }
        else
        {
            GameObject.Find("coins").GetComponent<Text>().color = Color.black;
            GameObject.Find("save").GetComponent<Button>().enabled = true;
            GameObject.Find("save").GetComponent<Image>().enabled = false;
            GameObject.Find("save").GetComponent<Button>().GetComponentInChildren<Text>().text = "";
            int cannonNotSelected = 0;
            for (int i = 0; i < cannon.Length; i++)
                if (cannon[i].Equals("cannon " + (i + 1) + ":"))
                    cannonNotSelected++;

            int treasureNotSelected = 0;
            for (int i = 0; i < treasure.Length; i++)
                if (treasure[i].Equals("treasure " + (i + 1) + ":"))
                    treasureNotSelected++;

            if(cannonNotSelected == cannon.Length || treasureNotSelected == treasure.Length)
            {
                GameObject.Find("save").GetComponent<Button>().enabled = false;
                GameObject.Find("save").GetComponent<Image>().enabled = true;
                GameObject.Find("save").GetComponent<Button>().GetComponentInChildren<Text>().text = "1 MAST & TREASURE NEEDED";
            }
        }
        GameObject.Find("coins").GetComponent<Text>().text = points.ToString();
    }

    public void exit()
    {
        ranSavedChoices = false;
    }
    
    public void fillSavedChoices()
    {
        resetButtons();
        points = System.Int32.Parse(System.IO.File.ReadAllText(Application.persistentDataPath + "/Difficulty.txt"));
        StreamReader rdr = new StreamReader(new FileStream(Application.persistentDataPath + "/Players.txt", FileMode.Open, FileAccess.Read));
        int firstPlayer = 0;
        for (int i = 1; rdr.Peek() > -1; i++)
        {
            if (rdr.ReadLine().Equals("t"))
            {
                firstPlayer = i;
                break;
            }
        }

        if (firstPlayer > 0)
        {
            readingPlayerFile = true;

            StreamReader read = new StreamReader(new FileStream(Application.persistentDataPath + "/Player" + firstPlayer + ".txt", FileMode.Open, FileAccess.Read));
            while (read.Peek() > -1)
            {
                string line = read.ReadLine().Trim();
                if (line.StartsWith("mast "))
                {
                    if (line.StartsWith("mast 1:"))
                    {
                        if (line.Equals("mast 1: l"))
                        {
                            GameObject.Find("mast1").GetComponent<Image>().sprite = mastImages[2];
                            mast[0] = "mast 1: l";
                            points -= 10;
                        }
                        if (line.Equals("mast 1: s"))
                        {
                            GameObject.Find("mast1").GetComponent<Image>().sprite = mastImages[1];
                            mast[0] = "mast 1: s";
                            points -= 5;
                        }
                    }
                    if (line.StartsWith("mast 2:"))
                    {
                        if (line.Equals("mast 2: l"))
                        {
                            GameObject.Find("mast2").GetComponent<Image>().sprite = mastImages[2];
                            mast[1] = "mast 2: l";
                            points -= 10;
                        }
                        if (line.Equals("mast 2: s"))
                        {
                            GameObject.Find("mast2").GetComponent<Image>().sprite = mastImages[1];
                            mast[1] = "mast 2: s";
                            points -= 5;
                        }
                    }
                    if (line.StartsWith("mast 3:"))
                    {
                        if (line.Equals("mast 3: l"))
                        {
                            GameObject.Find("mast3").GetComponent<Image>().sprite = mastImages[2];
                            mast[2] = "mast 3: l";
                            points -= 10;
                        }
                        if (line.Equals("mast 3: s"))
                        {
                            GameObject.Find("mast3").GetComponent<Image>().sprite = mastImages[1];
                            mast[2] = "mast 3: s";
                            points -= 5;
                        }
                    }
                }
                if (line.StartsWith("cannon "))
                {
                    if (line.StartsWith("cannon 1:"))
                    {
                        if (line.Equals("cannon 1: s"))
                        {
                            GameObject.Find("cannon1").GetComponent<Image>().sprite = cannonImages[1];
                            cannon[0] = "cannon 1: s";
                            points -= 6;
                        }
                        if (line.Equals("cannon 1: l"))
                        {
                            GameObject.Find("cannon1").GetComponent<Image>().sprite = cannonImages[2];
                            cannon[0] = "cannon 1: l";
                            points -= 12;
                        }

                    }
                    if (line.StartsWith("cannon 2:"))
                    {
                        if (line.Equals("cannon 2: s"))
                        {
                            GameObject.Find("cannon2").GetComponent<Image>().sprite = cannonImages[1];
                            cannon[1] = "cannon 2: s";
                            points -= 6;
                        }
                        if (line.Equals("cannon 2: l"))
                        {
                            GameObject.Find("cannon2").GetComponent<Image>().sprite = cannonImages[2];
                            cannon[1] = "cannon 2: l";
                            points -= 12;
                        }
                    }
                    if (line.StartsWith("cannon 3:"))
                    {
                        if (line.Equals("cannon 3: s"))
                        {
                            GameObject.Find("cannon3").GetComponent<Image>().sprite = cannonImages[1];
                            cannon[2] = "cannon 3: s";
                            points -= 6;
                        }
                        if (line.Equals("cannon 3: l"))
                        {
                            GameObject.Find("cannon3").GetComponent<Image>().sprite = cannonImages[2];
                            cannon[2] = "cannon 3: l";
                            points -= 12;
                        }
                    }
                    if (line.StartsWith("cannon 4:"))
                    {
                        if (line.Equals("cannon 4: s"))
                        {
                            GameObject.Find("cannon4").GetComponent<Image>().sprite = cannonImages[1];
                            cannon[3] = "cannon 4: s";
                            points -= 6;
                        }
                        if (line.Equals("cannon 4: l"))
                        {
                            GameObject.Find("cannon4").GetComponent<Image>().sprite = cannonImages[2];
                            cannon[3] = "cannon 4: l";
                            points -= 12;
                        }
                    }
                    if (line.StartsWith("cannon 5:"))
                    {
                        if (line.Equals("cannon 5: s"))
                        {
                            GameObject.Find("cannon5").GetComponent<Image>().sprite = cannonImages[1];
                            cannon[4] = "cannon 5: s";
                            points -= 6;
                        }
                        if (line.Equals("cannon 5: l"))
                        {
                            GameObject.Find("cannon5").GetComponent<Image>().sprite = cannonImages[2];
                            cannon[4] = "cannon 5: l";
                            points -= 12;
                        }
                    }
                }
                if(line.StartsWith("crew "))
                {
                    if(line.StartsWith("crew 1:"))
                    {
                        if (line.Equals("crew 1: t"))
                        {
                            crew[0] = "crew 1: t";
                            GameObject.Find("crew1").GetComponent<Image>().enabled = true;
                            points -= 20;
                        }
                    }
                    if(line.StartsWith("crew 2:"))
                    {
                        if (line.Equals("crew 2: t"))
                        {
                            crew[1] = "crew 2: t";
                            GameObject.Find("crew2").GetComponent<Image>().enabled = true;
                            points -= 20;
                        }
                    }
                }
                if (line.StartsWith("treasure "))
                {
                    if (line.StartsWith("treasure 1:"))
                    {
                        if(line.Equals("treasure 1: t"))
                        {
                            treasure[0] = "treasure 1: t";
                            GameObject.Find("treasure1").GetComponent<Image>().enabled = true;
                            points -= 10;
                        }
                    }
                    if (line.StartsWith("treasure 2:"))
                    {
                        if (line.Equals("treasure 2: t"))
                        {
                            treasure[1] = "treasure 2: t";
                            GameObject.Find("treasure2").GetComponent<Image>().enabled = true;
                            points -= 10;
                        }
                    }
                    if (line.StartsWith("treasure 3:"))
                    {
                        if (line.Equals("treasure 3: t"))
                        {
                            treasure[2] = "treasure 3: t";
                            GameObject.Find("treasure3").GetComponent<Image>().enabled = true;
                            points -= 10;
                        }
                    }
                }
                if(line.StartsWith("damage "))
                {
                        
                    if(line.StartsWith("damage 1:"))
                    {

                        if (line.Equals("damage 1: t"))
                        {
                            damage[0] = "damage 1: t";
                            GameObject.Find("damage1").GetComponent<Image>().enabled = true;
                            points -= 20;
                        }
                    }
                    if(line.StartsWith("damage 2:"))
                    {

                        if (line.Equals("damage 2: t"))
                        {
                            damage[1] = "damage 2: t";
                            GameObject.Find("damage2").GetComponent<Image>().enabled = true;
                            points -= 20;
                        }
                    }
                }              
            }
            read.Close();
            checkIfNoSelection(firstPlayer);
        }
    }
    
    private void checkIfNoSelection(int player)
    {
        int cannonNotSelected = 0;
        for(int i = 0; i < cannon.Length; i++)
            if (cannon[i].Equals("cannon " + (i + 1) + ":"))
                cannonNotSelected++;

        int treasureNotSelected = 0;
        for (int i = 0; i < treasure.Length; i++)
            if (treasure[i].Equals("treasure " + (i + 1) + ":"))
                treasureNotSelected++;

        if (cannonNotSelected == cannon.Length)
        {
            cannon[0] = "cannon 1: s";
            GameObject.Find("cannon1").GetComponent<Image>().sprite = cannonImages[1];
            points -= 6;
        }
        if(treasureNotSelected == treasure.Length)
        {
            treasure[0] = "treasure 1: t";
            GameObject.Find("treasure1").GetComponent<Image>().enabled = true;
            points -= 10;
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Player" + player + ".txt", string.Join("\n", mast) + "\n" +
                                                                                                  string.Join("\n", cannon) + "\n" +
                                                                                                  string.Join("\n", crew) + "\n" +
                                                                                                  string.Join("\n", treasure) + "\n" +
                                                                                                  string.Join("\n", damage));
    }

    private void resetButtons()
    {
        GameObject.Find("mast1").GetComponent<Image>().sprite = mastImages[0];
        GameObject.Find("mast2").GetComponent<Image>().sprite = mastImages[0];
        GameObject.Find("mast3").GetComponent<Image>().sprite = mastImages[0];
        for(int i = 0; i < 3; i++)
            mast[i] = "mast " + (i + 1) + ":";

        GameObject.Find("cannon1").GetComponent<Image>().sprite = cannonImages[0];
        GameObject.Find("cannon2").GetComponent<Image>().sprite = cannonImages[0];
        GameObject.Find("cannon3").GetComponent<Image>().sprite = cannonImages[0];
        GameObject.Find("cannon4").GetComponent<Image>().sprite = cannonImages[0];
        GameObject.Find("cannon5").GetComponent<Image>().sprite = cannonImages[0];
        for (int i = 0; i < 5; i++)
            cannon[i] = "cannon " + (i + 1) + ":";


        if (GameObject.Find("crew1").GetComponent<Button>().image.enabled)
            GameObject.Find("crew1").GetComponent<Button>().image.enabled = false;

        if (GameObject.Find("crew2").GetComponent<Button>().image.enabled)
            GameObject.Find("crew2").GetComponent<Button>().image.enabled = false;

        for (int i = 0; i < 2; i++)
            crew[i] = "crew " + (i + 1) + ":";


        if (GameObject.Find("treasure1").GetComponent<Button>().image.enabled)
            GameObject.Find("treasure1").GetComponent<Button>().image.enabled = false;

        if (GameObject.Find("treasure2").GetComponent<Button>().image.enabled)
            GameObject.Find("treasure2").GetComponent<Button>().image.enabled = false;

        if (GameObject.Find("treasure3").GetComponent<Button>().image.enabled)
            GameObject.Find("treasure3").GetComponent<Button>().image.enabled = false;

        for (int i = 0; i < 3; i++)
            treasure[i] = "treasure " + (i + 1) + ":";


        if (GameObject.Find("damage1").GetComponent<Button>().image.enabled)
            GameObject.Find("damage1").GetComponent<Button>().image.enabled = false;

        if (GameObject.Find("damage2").GetComponent<Button>().image.enabled)
            GameObject.Find("damage2").GetComponent<Button>().image.enabled = false;

        for (int i = 0; i < 2; i++)
            damage[i] = "damage " + (i + 1) + ":";

    }
    
    public void navLeftOrRight()
    {
        int player = System.Int32.Parse(GameObject.Find("shipNum").GetComponent<Text>().text);
        StreamReader read = new StreamReader(new FileStream(Application.persistentDataPath + "/Player" + player + ".txt", FileMode.Open, FileAccess.Read));
        points = System.Int32.Parse(System.IO.File.ReadAllText(Application.persistentDataPath + "/Difficulty.txt"));
        resetButtons();

        while (read.Peek() > -1)
        {
            string line = read.ReadLine().Trim();
            if (line.StartsWith("mast "))
            {
                if (line.StartsWith("mast 1:"))
                {
                    if (line.Equals("mast 1: l"))
                    {
                        GameObject.Find("mast1").GetComponent<Image>().sprite = mastImages[2];
                        mast[0] = "mast 1: l";
                        points -= 10;
                    }
                    if (line.Equals("mast 1: s"))
                    {
                        GameObject.Find("mast1").GetComponent<Image>().sprite = mastImages[1];
                        mast[0] = "mast 1: s";
                        points -= 5;
                    }
                }
                if (line.StartsWith("mast 2:"))
                {
                    if (line.Equals("mast 2: l"))
                    {
                        GameObject.Find("mast2").GetComponent<Image>().sprite = mastImages[2];
                        mast[1] = "mast 2: l";
                        points -= 10;
                    }
                    if (line.Equals("mast 2: s"))
                    {
                        GameObject.Find("mast2").GetComponent<Image>().sprite = mastImages[1];
                        mast[1] = "mast 2: s";
                        points -= 5;
                    }
                }
                if (line.StartsWith("mast 3:"))
                {
                    if (line.Equals("mast 3: l"))
                    {
                        GameObject.Find("mast3").GetComponent<Image>().sprite = mastImages[2];
                        mast[2] = "mast 3: l";
                        points -= 10;
                    }
                    if (line.Equals("mast 3: s"))
                    {
                        GameObject.Find("mast3").GetComponent<Image>().sprite = mastImages[1];
                        mast[2] = "mast 3: s";
                        points -= 5;
                    }
                }
            }
            if (line.StartsWith("cannon "))
            {
                if (line.StartsWith("cannon 1:"))
                {
                    if (line.Equals("cannon 1: s"))
                    {
                        GameObject.Find("cannon1").GetComponent<Image>().sprite = cannonImages[1];
                        cannon[0] = "cannon 1: s";
                        points -= 6;
                    }
                    if (line.Equals("cannon 1: l"))
                    {
                        GameObject.Find("cannon1").GetComponent<Image>().sprite = cannonImages[2];
                        cannon[0] = "cannon 1: l";
                        points -= 12;
                    }

                }
                if (line.StartsWith("cannon 2:"))
                {
                    if (line.Equals("cannon 2: s"))
                    {
                        GameObject.Find("cannon2").GetComponent<Image>().sprite = cannonImages[1];
                        cannon[1] = "cannon 2: s";
                        points -= 6;
                    }
                    if (line.Equals("cannon 2: l"))
                    {
                        GameObject.Find("cannon2").GetComponent<Image>().sprite = cannonImages[2];
                        cannon[1] = "cannon 2: l";
                        points -= 12;
                    }
                }
                if (line.StartsWith("cannon 3:"))
                {
                    if (line.Equals("cannon 3: s"))
                    {
                        GameObject.Find("cannon3").GetComponent<Image>().sprite = cannonImages[1];
                        cannon[2] = "cannon 3: s";
                        points -= 6;
                    }
                    if (line.Equals("cannon 3: l"))
                    {
                        GameObject.Find("cannon3").GetComponent<Image>().sprite = cannonImages[2];
                        cannon[2] = "cannon 3: l";
                        points -= 12;
                    }
                }
                if (line.StartsWith("cannon 4:"))
                {
                    if (line.Equals("cannon 4: s"))
                    {
                        GameObject.Find("cannon4").GetComponent<Image>().sprite = cannonImages[1];
                        cannon[3] = "cannon 4: s";
                        points -= 6;
                    }
                    if (line.Equals("cannon 4: l"))
                    {
                        GameObject.Find("cannon4").GetComponent<Image>().sprite = cannonImages[2];
                        cannon[3] = "cannon 4: l";
                        points -= 12;
                    }
                }
                if (line.StartsWith("cannon 5:"))
                {
                    if (line.Equals("cannon 5: s"))
                    {
                        GameObject.Find("cannon5").GetComponent<Image>().sprite = cannonImages[1];
                        cannon[4] = "cannon 5: s";
                        points -= 6;
                    }
                    if (line.Equals("cannon 5: l"))
                    {
                        GameObject.Find("cannon5").GetComponent<Image>().sprite = cannonImages[2];
                        cannon[4] = "cannon 5: l";
                        points -= 12;
                    }
                }
            }
            if (line.StartsWith("crew "))
            {
                if (line.StartsWith("crew 1:"))
                {
                    if (line.Equals("crew 1: t"))
                    {
                        crew[0] = "crew 1: t";
                        GameObject.Find("crew1").GetComponent<Image>().enabled = true;
                        points -= 20;
                    }
                }
                if (line.StartsWith("crew 2:"))
                {
                    if (line.Equals("crew 2: t"))
                    {
                        crew[1] = "crew 2: t";
                        GameObject.Find("crew2").GetComponent<Image>().enabled = true;
                        points -= 20;
                    }
                }
            }
            if (line.StartsWith("treasure "))
            {
                if (line.StartsWith("treasure 1:"))
                {
                    if (line.Equals("treasure 1: t"))
                    {
                        treasure[0] = "treasure 1: t";
                        GameObject.Find("treasure1").GetComponent<Image>().enabled = true;
                        points -= 10;
                    }
                }
                if (line.StartsWith("treasure 2:"))
                {
                    if (line.Equals("treasure 2: t"))
                    {
                        treasure[1] = "treasure 2: t";
                        GameObject.Find("treasure2").GetComponent<Image>().enabled = true;
                        points -= 10;
                    }
                }
                if (line.StartsWith("treasure 3:"))
                {
                    if (line.Equals("treasure 3: t"))
                    {
                        treasure[2] = "treasure 3: t";
                        GameObject.Find("treasure3").GetComponent<Image>().enabled = true;
                        points -= 10;
                    }
                }
            }
            if (line.StartsWith("damage "))
            {

                if (line.StartsWith("damage 1:"))
                {

                    if (line.Equals("damage 1: t"))
                    {
                        damage[0] = "damage 1: t";
                        GameObject.Find("damage1").GetComponent<Image>().enabled = true;
                        points -= 20;
                    }
                }
                if (line.StartsWith("damage 2:"))
                {

                    if (line.Equals("damage 2: t"))
                    {
                        damage[1] = "damage 2: t";
                        GameObject.Find("damage2").GetComponent<Image>().enabled = true;
                        points -= 20;
                    }
                }
            }
        }
        read.Close();
        checkIfNoSelection(player);
    }

    public void mastClick()
    {
        if(emptyMastBtn.GetComponent<Image>().sprite.Equals(mastImages[2]))//if (mastClicks == 0)
        {
            emptyMastBtn.GetComponent<Image>().sprite = mastImages[0];
            if (emptyMastBtn.name.Equals("mast1"))
                mast[0] = "mast 1:";
            if (emptyMastBtn.name.Equals("mast2"))
                mast[1] = "mast 2:";
            if (emptyMastBtn.name.Equals("mast3"))
                mast[2] = "mast 3:";
            points += 10;
        }
        else if(emptyMastBtn.GetComponent<Image>().sprite.Equals(mastImages[0]))//else if(mastClicks == 1)
        {
            emptyMastBtn.GetComponent<Image>().sprite = mastImages[1];
            if (emptyMastBtn.name.Equals("mast1"))
                mast[0] = "mast 1: s";
            if (emptyMastBtn.name.Equals("mast2"))
                mast[1] = "mast 2: s";
            if (emptyMastBtn.name.Equals("mast3"))
                mast[2] = "mast 3: s";
            points -= 5;
        }
        else
        {
            emptyMastBtn.GetComponent<Image>().sprite = mastImages[2];
            if (emptyMastBtn.name.Equals("mast1"))
                mast[0] = "mast 1: l";
            if (emptyMastBtn.name.Equals("mast2"))
                mast[1] = "mast 2: l";
            if (emptyMastBtn.name.Equals("mast3"))
                mast[2] = "mast 3: l";
            points -= 5;
        }
    }
    
    public void cannonClick()
    {
        if(emptyCannonBtn.GetComponent<Image>().sprite.Equals(cannonImages[2]))//if (cannonClicks == 0)
        {
            emptyCannonBtn.GetComponent<Image>().sprite = cannonImages[0];
            if (emptyCannonBtn.name.Equals("cannon1"))
                cannon[0] = "cannon 1:";
            if (emptyCannonBtn.name.Equals("cannon2"))
                cannon[1] = "cannon 2:";
            if (emptyCannonBtn.name.Equals("cannon3"))
                cannon[2] = "cannon 3:";
            if (emptyCannonBtn.name.Equals("cannon4"))
                cannon[3] = "cannon 4:";
            if (emptyCannonBtn.name.Equals("cannon5"))
                cannon[4] = "cannon 5:";
            points += 12;
        }
        else if(emptyCannonBtn.GetComponent<Image>().sprite.Equals(cannonImages[0]))//else if(cannonClicks == 1)
        {
            emptyCannonBtn.GetComponent<Image>().sprite = cannonImages[1];
            if (emptyCannonBtn.name.Equals("cannon1"))
                cannon[0] = "cannon 1: s";
            if (emptyCannonBtn.name.Equals("cannon2"))
                cannon[1] = "cannon 2: s";
            if (emptyCannonBtn.name.Equals("cannon3"))
                cannon[2] = "cannon 3: s";
            if (emptyCannonBtn.name.Equals("cannon4"))
                cannon[3] = "cannon 4: s";
            if (emptyCannonBtn.name.Equals("cannon5"))
                cannon[4] = "cannon 5: s";
            points -= 6;
        }
        else
        {
            emptyCannonBtn.GetComponent<Image>().sprite = cannonImages[2];
            if (emptyCannonBtn.name.Equals("cannon1"))
                cannon[0] = "cannon 1: l";
            if (emptyCannonBtn.name.Equals("cannon2"))
                cannon[1] = "cannon 2: l";
            if (emptyCannonBtn.name.Equals("cannon3"))
                cannon[2] = "cannon 3: l";
            if (emptyCannonBtn.name.Equals("cannon4"))
                cannon[3] = "cannon 4: l";
            if (emptyCannonBtn.name.Equals("cannon5"))
                cannon[4] = "cannon 5: l";
            points -= 6;
        }
    }
    
    public void crewSelect()
    {
        if (crewBtn.image.enabled)
        {
            if (crewBtn.name.Equals("crew1"))
                crew[0] = "crew 1:";
            if (crewBtn.name.Equals("crew2"))
                crew[1] = "crew 2:";
            crewBtn.image.enabled = false;
            points += 20;
        }
        else
        {
            if (crewBtn.name.Equals("crew1"))
                crew[0] = "crew 1: t";
            if (crewBtn.name.Equals("crew2"))
                crew[1] = "crew 2: t";
            crewBtn.image.enabled = true;
            points -= 20;
        }
    }
    
    public void treasureSelect()
    {
        if (treasureBtn.image.enabled)
        {
            if (treasureBtn.name.Equals("treasure1"))
                treasure[0] = "treasure 1:";
            if (treasureBtn.name.Equals("treasure2"))
                treasure[1] = "treasure 2:";
            if (treasureBtn.name.Equals("treasure3"))
                treasure[2] = "treasure 3:";
            treasureBtn.image.enabled = false;
            points += 10;
        }
        else
        {
            if (treasureBtn.name.Equals("treasure1"))
                treasure[0] = "treasure 1: t";
            if (treasureBtn.name.Equals("treasure2"))
                treasure[1] = "treasure 2: t";
            if (treasureBtn.name.Equals("treasure3"))
                treasure[2] = "treasure 3: t";
            treasureBtn.image.enabled = true;
            points -= 10;
        }
    }

    public void damageSelect()
    {
        if (damageBtn.image.enabled)
        {
            if (damageBtn.name.Equals("damage1"))
                damage[0] = "damage 1:";
            if (damageBtn.name.Equals("damage2"))
                damage[1] = "damage 2:";
            damageBtn.image.enabled = false;
            points += 20;
        }
        else
        {
            if (damageBtn.name.Equals("damage1"))
                damage[0] = "damage 1: t";
            if (damageBtn.name.Equals("damage2"))
                damage[1] = "damage 2: t";
            damageBtn.image.enabled = true;
            points -= 20;
        }

    }
    
    public void save()
    {
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Player"+ GameObject.Find("shipNum").GetComponent<Text>().text+".txt", string.Join("\n", mast)+"\n"+
                                                                                                                                             string.Join("\n", cannon)+"\n"+
                                                                                                                                             string.Join("\n", crew)+"\n"+
                                                                                                                                             string.Join("\n", treasure)+"\n"+
                                                                                                                                             string.Join("\n", damage));
    }
}
