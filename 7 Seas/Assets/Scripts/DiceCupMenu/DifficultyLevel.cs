//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class DifficultyLevel : MonoBehaviour {

//    public Sprite swabieOnSprite;
//    public Sprite swabieOffSprite;
//    public Sprite seamanOnSprite;
//    public Sprite seamanOffSprite;
//    public Sprite captainOnSprite;
//    public Sprite captainOffSprite;
//    public bool swabieOn = false;
//    public bool seamanOn = false;
//    public bool captainOn = false;

//    // Use this for initialization
//    void Start () {

//        string str = "Seaman";
//        if (str == "Swabie")
//        {
//            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOnSprite;
//        }
//        else if (str == "Seaman")
//        {
//            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOnSprite;
//        }
//        else if (str == "Captain")
//        {
//            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOnSprite;
//        }
//    }
	
//    //swaps dice levels to edit them
//    public void LoadDiceLevel()
//    {
//        if(this.name == "Swabie")
//        {
//            ES2.Save(this.name, "SelectedCup");
//            DiceCupMain.swabie = true;
//            DiceCupMain.seaman = false;
//            DiceCupMain.captain = false;
//            swabieOn = true;
//            seamanOn = false;
//            captainOn = false;
//            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOnSprite;
//            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOffSprite;
//            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOffSprite;
//        }
//        else if (this.name == "Seaman")
//        {
//            ES2.Save(this.name, "SelectedCup");
//            DiceCupMain.swabie = false;
//            DiceCupMain.seaman = true;
//            DiceCupMain.captain = false;
//            swabieOn = false;
//            seamanOn = true;
//            captainOn = false;
//            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOffSprite;
//            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOnSprite;
//            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOffSprite;
//        }
//        else if (this.name == "Captain")
//        {
//            ES2.Save(this.name, "SelectedCup");
//            DiceCupMain.swabie = false;
//            DiceCupMain.seaman = false;
//            DiceCupMain.captain = true;
//            swabieOn = false;
//            seamanOn = false;
//            captainOn = true;
//            GameObject.Find("Swabie").GetComponent<Button>().image.overrideSprite = swabieOffSprite;
//            GameObject.Find("Seaman").GetComponent<Button>().image.overrideSprite = seamanOffSprite;
//            GameObject.Find("Captain").GetComponent<Button>().image.overrideSprite = captainOnSprite;
//        }
//    }
//}
