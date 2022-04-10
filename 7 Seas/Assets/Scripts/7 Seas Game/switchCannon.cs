using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class switchCannon : MonoBehaviour
{
    //Setting up 3 cameras
    public Camera cameraOne;
    public Camera cannonCamOne;
    public Camera cameraTwo;
    public Camera cannonCamTwo;
    public Camera cameraThree;
    public Camera cannonCamThree;
    public Camera cameraFour;
    public Camera cannonCamFour;
    public Camera cameraFive;
    public Camera cannonCamFive;
    public Cannon_Firing cannonOne;
    public Cannon_Firing cannonTwo;
    public Cannon_Firing cannonThree;
    public Cannon_Firing cannonFour;
    public Cannon_Firing cannonFive;
    //bool hasStarted = false;
    public Button swapBtnRight;
    public Button swapBtnLeft;

    //current camera will be middle

    // int max num of cannons
    int numOfCannon = 5; //have this as a configured player pref
    int curNum = 0;

    //disables cannons 1 and 3 camera, activates cannon 2 camera
    void start()
    {
        cameraOne.enabled = true;
        cannonCamOne.enabled = true;
        cameraTwo.enabled = false;
        cannonCamTwo.enabled = false;
        cameraThree.enabled = false;
        cannonCamThree.enabled = false;
        cameraFour.enabled = false;
        cannonCamFour.enabled = false;
        cameraFive.enabled = false;
        cannonCamFive.enabled = false;
        //to be implemented later
        //cameraThree.enabled = false;
        //Debug.Log("start");
        //Button btnToSwapRight = swapBtnRight.GetComponent<Button>();
        //Button btnToSwapLeft = swapBtnLeft.GetComponent<Button>();
        //btnToSwapRight.onClick.AddListener(swapCameraRight);
        //btnToSwapLeft.onClick.AddListener(swapCameraLeft);
    }

    public void swapCameraOne()
    {
        curNum = 0;
        cameraOne.enabled = true;
        cannonCamOne.enabled = true;
        cannonOne.CannonSelected(true);

        cameraTwo.enabled = false;
        cannonCamTwo.enabled = false;
        cannonTwo.CannonSelected(false);

        cameraThree.enabled = false;
        cannonCamThree.enabled = false;
        cannonThree.CannonSelected(false);

        cameraFour.enabled = false;
        cannonCamFour.enabled = false;
        cannonFour.CannonSelected(false);

        cameraFive.enabled = false;
        cannonCamFive.enabled = false;
        cannonFive.CannonSelected(false);
    }


    //If we decide to add more cannons, we can make an array of cameras
    //to go thorugh, would be easier then n number of if statements
    public void swapCameraRight()
    {
        //move one cannon to right
        ++curNum;
        //if at limit then go back to the first cannon (cannon 0)
        int currentCannon = curNum % numOfCannon;

        //disable all cameras
        cameraOne.enabled = false;
        cannonCamOne.enabled = false;
        cannonOne.CannonSelected(false);

        cameraThree.enabled = false;
        cannonCamThree.enabled = false;
        cannonThree.CannonSelected(false);

        cameraFour.enabled = false;
        cannonCamFour.enabled = false;
        cannonFour.CannonSelected(false);
        //Debug.Log("CameraOne");

        cameraFive.enabled = false;
        cannonCamFive.enabled = false;
        cannonFive.CannonSelected(false);

        cameraTwo.enabled = false;
        cannonCamTwo.enabled = false;
        cannonTwo.CannonSelected(false);

        //switches based off of what num we get to now
        switch (currentCannon)
        {
            case 0:
                cameraOne.enabled = true;
                cannonCamOne.enabled = true;
                cannonOne.CannonSelected(true);
                break;
            case 1:
                cameraTwo.enabled = true;
                cannonCamTwo.enabled = true;
                cannonTwo.CannonSelected(true);
                break;
            case 2:
                cameraThree.enabled = true;
                cannonCamThree.enabled = true;
                cannonThree.CannonSelected(true);
                break;
            case 3:
                cameraFour.enabled = true;
                cannonCamFour.enabled = true;
                cannonFour.CannonSelected(true);
                break;
            case 4:
                cameraFive.enabled = true;
                cannonCamFive.enabled = true;
                cannonFive.CannonSelected(true);
                break;
        }
    }
    //exact same thing except for left
    public void swapCameraLeft()
    {
        //move one cannon to right
        --curNum;
        if (curNum < 0)
            curNum = numOfCannon - 1;
        //if at limit then go back to the first cannon (cannon 0)
        int currentCannon = curNum % numOfCannon;

        //disable all cameras
        cameraOne.enabled = false;
        cannonCamOne.enabled = false;
        cannonOne.CannonSelected(false);

        cameraThree.enabled = false;
        cannonCamThree.enabled = false;
        cannonThree.CannonSelected(false);

        cameraFour.enabled = false;
        cannonCamFour.enabled = false;
        cannonFour.CannonSelected(false);
        //Debug.Log("CameraOne");

        cameraFive.enabled = false;
        cannonCamFive.enabled = false;
        cannonFive.CannonSelected(false);

        cameraTwo.enabled = false;
        cannonCamTwo.enabled = false;
        cannonTwo.CannonSelected(false);

        //switches based off of what num we get to now
        switch (currentCannon)
        {
            case 0:
                cameraOne.enabled = true;
                cannonCamOne.enabled = true;
                cannonOne.CannonSelected(true);
                break;
            case 1:
                cameraTwo.enabled = true;
                cannonCamTwo.enabled = true;
                cannonTwo.CannonSelected(true);
                break;
            case 2:
                cameraThree.enabled = true;
                cannonCamThree.enabled = true;
                cannonThree.CannonSelected(true);
                break;
            case 3:
                cameraFour.enabled = true;
                cannonCamFour.enabled = true;
                cannonFour.CannonSelected(true);
                break;
            case 4:
                cameraFive.enabled = true;
                cannonCamFive.enabled = true;
                cannonFive.CannonSelected(true);
                break;
        }
    }

}