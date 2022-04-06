using UnityEngine;
using UnityEngine.UI;

public class Crew : MonoBehaviour
{
    static PlayerShip player;
    static Text crewText;
    static bool added = false;

    public static void AddCrew(PlayerShip playerInfo, Text crewText)
    {

        int num;

        player = playerInfo;
        Crew.crewText = crewText;

        if (player.GetCrew() > 0 && !player.HasBought())
        {
            while (!added)
            {
                num = Random.Range(0, 4);

                ProcessCrew(num);
            }

            player.SetCrewBought();
        }
        else if (player.HasBought())
        {
            crewText.text = "YOU CAN ONLY GET 1 CREW MEMBER PER TURN";
        }
        else
        {
            crewText.text = "YOU SHIP DOES NOT HAVE SPACE FOR A CREW MEMBER";
        }

        crewText.gameObject.SetActive(true);
    }
    static void ProcessCrew(int num)
    {
        switch (num)
        {
            case 0:
                AddPlayerBase();
                break;
            case 1:
                AddPlayerCompass();
                break;
            case 2:
                AddPlayerCombat();
                break;
            case 3:
                AddPlayerCannons();
                break;
        }
    }

    static void AddPlayerBase()
    {
        player.AddBase();
        player.RemoveCrew();
        crewText.text = "A NAVIGATOR HAS JOINED YOUR CREW! \n\n +2 NAVIGATE MOVEMENT";

        added = true;
    }

    static void AddPlayerCompass()
    {
        player.AddCompass();
        player.RemoveCrew();
        crewText.text = "A HELMSMAN HAS JOINED YOUR CREW! \n\n +2 COMPASS MOVEMENT";

        added = true;
    }

    static void AddPlayerCombat()
    {
        player.AddCombat();
        player.RemoveCrew();
        crewText.text = "A CANNONEER HAS JOINED YOUR CREW! \n\n +2 COMBAT POINTS";

        added = true;
    }

    static void AddPlayerCannons()
    {

        if (player.AddLRCannon())
        {
            player.RemoveCrew();
            crewText.text = "A QUARTERMASTER HAS JOINED YOUR CREW! \n\n +1 LONG RANGE CANNON";

            added = true;
        }
    }
}