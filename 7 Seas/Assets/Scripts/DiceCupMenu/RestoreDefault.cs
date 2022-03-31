//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class RestoreDefault : MonoBehaviour
//{
//    public Text tarShipFrance;
//    public Text tarShipEngland;
//    public Text tarShipHolland;
//    public Text tarShipSpain;
//    public Text tarShipItaly;
//    public Text tarShipPortugal;
//    public Text tarShipGhost;

//    public Text moveNum1;
//    public Text moveNum2;
//    public Text moveNum3;
//    public Text moveNum4;
//    public Text moveNum5;
//    public Text moveNum6;
//    public Text moveNumGhost;

//    public Text windNum0;
//    public Text windNum1;
//    public Text windNum2;
//    public Text windNum3;
//    public Text windNum4;
//    public Text windNumFog;
//    public Text windNumGhost;

//    public Text resK1CB0;
//    public Text resK1CB2;
//    public Text resK2CB0;
//    public Text resK2CB1;
//    public Text resK4CB3;
//    public Text resGhost;
//    public Text resK0CB1;
//    public Text resK2CB2;
//    public Text resK0CB3;
//    public Text resK3CB0;
//    public Text resK3CB2;
//    public Text resRats;

//    public Text parrotAmber;
//    public Text parrotBlue;
//    public Text parrotBrown;
//    public Text parrotGreen;
//    public Text parrotOrange;
//    public Text parrotPurple;
//    public Text parrotRed;
//    public Text parrotWhite;
//    public Text parrotGhost;

//    public string difficulty;
//    public SaveAndLoadDice saveAndLoadDice;

//    private void Start()
//    {
//        //get diff level
//        difficulty = saveAndLoadDice.cupSelected;
//    }

//    public void SetDefault()
//    {
//        difficulty = saveAndLoadDice.cupSelected;

//        //set default values based on level
//        if (difficulty == "Swabie")
//        {
//            SetSwabie();
//        }
//        else if (difficulty == "Seaman")
//        {
//            SetSeaMan();
//        }
//        else if (difficulty == "Captain")
//        {
//            SetCaptain();
//        }

//        saveAndLoadDice.Save();
//    }

//    //set the swabie default dice
//    public void SetSwabie()
//    {
//        tarShipFrance.text = "10";
//        tarShipEngland.text = "10";
//        tarShipHolland.text = "10";
//        tarShipSpain.text = "10";
//        tarShipItaly.text = "10";
//        tarShipPortugal.text = "10";
//        tarShipGhost.text = "1";

//        moveNum1.text = "1";
//        moveNum2.text = "1";
//        moveNum3.text = "1";
//        moveNum4.text = "1";
//        moveNum5.text = "1";
//        moveNum6.text = "1";
//        moveNumGhost.text = "1";

//        windNum0.text = "1";
//        windNum1.text = "1";
//        windNum2.text = "1";
//        windNum3.text = "1";
//        windNum4.text = "1";
//        windNumFog.text = "1";
//        windNumGhost.text = "1";

//        resK1CB0.text = "1";
//        resK1CB2.text = "1";
//        resK2CB0.text = "1";
//        resK2CB1.text = "1";
//        resK4CB3.text = "1";
//        resGhost.text = "1";
//        resK0CB1.text = "1";
//        resK2CB2.text = "1";
//        resK0CB3.text = "1";
//        resK3CB0.text = "1";
//        resK3CB2.text = "1";
//        resRats.text = "1";

//        parrotAmber.text = "1";
//        parrotBlue.text = "1";
//        parrotBrown.text = "1";
//        parrotGreen.text = "1";
//        parrotOrange.text = "1";
//        parrotPurple.text = "1";
//        parrotRed.text = "1";
//        parrotWhite.text = "1";
//        parrotGhost.text = "1";

//    }

//    //set the seaman default dice
//    public void SetSeaMan()
//    {
//        tarShipFrance.text = "10";
//        tarShipEngland.text = "10";
//        tarShipHolland.text = "10";
//        tarShipSpain.text = "10";
//        tarShipItaly.text = "10";
//        tarShipPortugal.text = "10";
//        tarShipGhost.text = "1";

//        moveNum1.text = "1";
//        moveNum2.text = "1";
//        moveNum3.text = "1";
//        moveNum4.text = "1";
//        moveNum5.text = "1";
//        moveNum6.text = "1";
//        moveNumGhost.text = "1";

//        windNum0.text = "1";
//        windNum1.text = "1";
//        windNum2.text = "1";
//        windNum3.text = "1";
//        windNum4.text = "1";
//        windNumFog.text = "1";
//        windNumGhost.text = "1";

//        resK1CB0.text = "1";
//        resK1CB2.text = "1";
//        resK2CB0.text = "1";
//        resK2CB1.text = "1";
//        resK4CB3.text = "1";
//        resGhost.text = "1";
//        resK0CB1.text = "1";
//        resK2CB2.text = "1";
//        resK0CB3.text = "1";
//        resK3CB0.text = "1";
//        resK3CB2.text = "1";
//        resRats.text = "1";

//        parrotAmber.text = "1";
//        parrotBlue.text = "1";
//        parrotBrown.text = "1";
//        parrotGreen.text = "1";
//        parrotOrange.text = "1";
//        parrotPurple.text = "1";
//        parrotRed.text = "1";
//        parrotWhite.text = "1";
//        parrotGhost.text = "1";

//    }

//    //set the captain default dice
//    public void SetCaptain()
//    {
//        tarShipFrance.text = "10";
//        tarShipEngland.text = "10";
//        tarShipHolland.text = "10";
//        tarShipSpain.text = "10";
//        tarShipItaly.text = "10";
//        tarShipPortugal.text = "10";
//        tarShipGhost.text = "1";

//        moveNum1.text = "1";
//        moveNum2.text = "1";
//        moveNum3.text = "1";
//        moveNum4.text = "1";
//        moveNum5.text = "1";
//        moveNum6.text = "1";
//        moveNumGhost.text = "1";

//        windNum0.text = "1";
//        windNum1.text = "1";
//        windNum2.text = "1";
//        windNum3.text = "1";
//        windNum4.text = "1";
//        windNumFog.text = "1";
//        windNumGhost.text = "1";

//        resK1CB0.text = "1";
//        resK1CB2.text = "1";
//        resK2CB0.text = "1";
//        resK2CB1.text = "1";
//        resK4CB3.text = "1";
//        resGhost.text = "1";
//        resK0CB1.text = "1";
//        resK2CB2.text = "1";
//        resK0CB3.text = "1";
//        resK3CB0.text = "1";
//        resK3CB2.text = "1";
//        resRats.text = "1";

//        parrotAmber.text = "1";
//        parrotBlue.text = "1";
//        parrotBrown.text = "1";
//        parrotGreen.text = "1";
//        parrotOrange.text = "1";
//        parrotPurple.text = "1";
//        parrotRed.text = "1";
//        parrotWhite.text = "1";
//        parrotGhost.text = "1";

//    }



//}
