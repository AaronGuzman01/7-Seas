using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera main;
    [SerializeField] private CinemachineVirtualCamera[] playerDefault;
    [SerializeField] private CinemachineVirtualCamera[] playerOverhead;
    [SerializeField] private CinemachineVirtualCamera diceAndCups;

    public static int index;
    private static bool overhead = true;
    public static bool cups = false;
    static int playerNum;
    Vector3 currPos;

    static bool[] activePlayers;
    int[] players = new int[8];
    int playerIndex = 0;
    private int maxPlayers;

    public float touchSensitivity = 10f;
    public float minZoom = 30f;
    public float maxZoom = 145f;
    public float zoomSpeed = 0.05f;

    private float navCount;
    private int dice;

    //GUI
    private bool GUI = false;
    public GameObject buttons;
    private bool left = false;
    private bool right = false;

    //CAMERA DUMMY
    public GameObject dummy;
    public GameObject[] ship;
    public GameObject ghostDummy;

    //Camera Tilt
    private int xTilt = 0;

    //Camera Position Display
    public Text[] positionTexts;
    Vector3Int cameraPos;
    public GameObject PanDirection;
    public GameObject TiltDirection;
    private Vector3 pan;
    private Vector3 tilt;

    void Start()
    {
        int count = 0;

        activePlayers = MapLoad.activePlayers;
        
        for (int i = 0; i < 8; i++)
        {
            if (activePlayers[i] == true)
            {
                players[count] = i;
                count++;
            }
        }
        
        playerNum = players[playerIndex];

        playerOverhead[playerNum].Priority = 1;

        currPos = main.transform.position;

        CinemachineCore.GetInputAxis = this.GetCustomAxis;

        ResetDummy();
        
        maxPlayers = MapLoad.maxPlayers;
    }

    //Mostly Switching between cameras
    void Update()
    {
        if (playerNum != MapLoad.playerNum)
        {
            playerDefault[playerNum].Priority = 0;
            playerOverhead[playerNum].Priority = 0;

            playerNum = MapLoad.playerNum;

            playerDefault[playerNum].Priority = 0;
            playerOverhead[playerNum].Priority = 1;
        }

        currPos = main.transform.position;

        if (overhead == false)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel"));      //Zooming with mouse scrollwheel
        }

        //GUI
        if (GUI == true && overhead == true)
        {
            buttons.SetActive(true);
        }
        else
        {
            buttons.SetActive(false);
        }

        ghostDummy.transform.rotation = dummy.transform.rotation;

    }

    public void ChangeView()
    {
        if (overhead)
        {
            playerDefault[playerNum].Priority = 1;
            playerOverhead[playerNum].Priority = 0;

            overhead = false;
        }
        else
        {
            playerDefault[playerNum].Priority = 0;
            playerOverhead[playerNum].Priority = 1;

            overhead = true;

            DetermineZoom();
        }
    }

    public void SetDefaultView()
    {
        playerDefault[playerNum].Priority = 1;
        playerOverhead[playerNum].Priority = 0;

        overhead = false;
    }

    public void SetOverheadView()
    {
        ResetDummy();

        playerDefault[playerNum].Priority = 0;
        playerOverhead[playerNum].Priority = 1;

        overhead = true;

        DetermineZoom();
    }

    public void SetDiceAndCups()
    {
        StartCoroutine(DiceAndCups());
    }

    public IEnumerator DiceAndCups()
    {
        diceAndCups.Priority = 2;

        yield return new WaitUntil(() => currPos == diceAndCups.transform.position);

        cups = true;

        yield return new WaitForSeconds(10);

        MapLoad.isRolling = false;
        MapLoad.diceSet = true;

        diceAndCups.Priority = 0;

        GUI = true;

        navCount = MapLoad.diceVals[0];

        DetermineZoom();
    }

    //Manages Camera Movement
    public float GetCustomAxis(string axisName)
    {
        if (overhead == false)   //Touch Mode
        {
            if (axisName == "Custom")
            {
                if (Input.touchCount == 2)          //Two fingers for pinch zooming
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    float difference = currentMagnitude - prevMagnitude;

                    Zoom(difference * zoomSpeed);
                }
                else if (Input.touchCount > 0)       //Mobile Touch
                {
                    return Input.touches[0].deltaPosition.x / touchSensitivity;

                }
                else if (Input.GetMouseButton(0))    //Mouse Click
                {
                    return UnityEngine.Input.GetAxis("Mouse X");
                }
            }
        }
       
        return UnityEngine.Input.GetAxis(axisName);
    }

    public void LeftButton()
    {
        if (overhead == true)
        {
            playerOverhead[playerNum].m_LookAt = null;

            ghostDummy.transform.Translate(-12.0f, 0.0f, 0.0f, Space.World);

            if (ghostDummy.transform.position.x > -403 && ghostDummy.transform.position.x < 547 && ghostDummy.transform.position.z > -559 && ghostDummy.transform.position.z < 391)
            {
                if (dummy.transform.position.x - ship[playerNum].transform.position.x > -120)
                {
                    dummy.transform.Translate(-12.0f, 0.0f, 0.0f, Space.World);

                    cameraPos.x -= 1;
                    positionTexts[0].text = (cameraPos.x + 34).ToString();
                }
            }

            ghostDummy.transform.position = dummy.transform.position;    
        }
    }


    public void RightButton()
    {
        if (overhead == true)
        {
            playerOverhead[playerNum].m_LookAt = null;

            ghostDummy.transform.Translate(12.0f, 0.0f, 0.0f, Space.World);

            if (ghostDummy.transform.position.x > -403 && ghostDummy.transform.position.x < 547 && ghostDummy.transform.position.z > -559 && ghostDummy.transform.position.z < 391)
            {
                if (dummy.transform.position.x - ship[playerNum].transform.position.x < 120)
                {
                    dummy.transform.Translate(12.0f, 0.0f, 0.0f, Space.World);

                    cameraPos.x += 1;
                    positionTexts[0].text = (cameraPos.x + 34).ToString();
                }
            }

            ghostDummy.transform.position = dummy.transform.position;
        }
    }

    public void UpButton()
    {
        if (overhead == true)
        { 
            playerOverhead[playerNum].m_LookAt = null;

            ghostDummy.transform.Translate(0.0f, 0.0f, 12.0f, Space.World);

            if (ghostDummy.transform.position.x > -403 && ghostDummy.transform.position.x < 547 && ghostDummy.transform.position.z > -559 && ghostDummy.transform.position.z < 391)
            {
                if (dummy.transform.position.z - ship[playerNum].transform.position.z < 120)
                {
                    dummy.transform.Translate(0.0f, 0.0f, 12.0f, Space.World);

                    cameraPos.y += 1;
                    positionTexts[1].text = ((cameraPos.y - 32) * -1).ToString();
                }
            }

            ghostDummy.transform.position = dummy.transform.position;
        }
    }

    public void DownButton()
    {
        if (overhead == true)
        {
            playerOverhead[playerNum].m_LookAt = null;

            ghostDummy.transform.Translate(0.0f, 0.0f, -12.0f, Space.World);

            if (ghostDummy.transform.position.x > -403 && ghostDummy.transform.position.x < 547 && ghostDummy.transform.position.z > -559 && ghostDummy.transform.position.z < 391)
            {
                if (dummy.transform.position.z - ship[playerNum].transform.position.z > -120)
                {
                    dummy.transform.Translate(0.0f, 0.0f, -12.0f, Space.World);

                    cameraPos.y -= 1;
                    positionTexts[1].text = ((cameraPos.y - 32) * -1).ToString();
                }
            }

            ghostDummy.transform.position = dummy.transform.position;
        }

    }

    public void ZoomIn()
    {
        Zoom(1.0f);
    }

    public void ZoomOut()
    {
        Zoom(-1.0f);
    }

    public void PedestalUp()
    {
        if (dummy.transform.position.y - ship[playerNum].transform.position.y < 60)
        {
            dummy.transform.Translate(0.0f, 12.0f, 0.0f, Space.World);
            
            cameraPos.z += 1;
            positionTexts[2].text = cameraPos.z.ToString();
        }
    }

    public void PedestalDown()
    {
        if (dummy.transform.position.y - ship[playerNum].transform.position.y > 13)
        {
            dummy.transform.Translate(0.0f, -12.0f, 0.0f, Space.World);
            
            cameraPos.z -= 1;
            positionTexts[2].text = cameraPos.z.ToString();
        }
    }

    public void PanLeft()
    {
        playerOverhead[playerNum].m_LookAt = dummy.transform;

        dummy.transform.Rotate(0.0f, 10.0f, 0.0f, Space.World);

        //Update Pan Direction Display
        PanDirection.transform.Rotate(0.0f, 0.0f, -10.0f);

    }

    public void PanRight()
    {
        playerOverhead[playerNum].m_LookAt = dummy.transform;

        dummy.transform.Rotate(0.0f, -10.0f, 0.0f, Space.World);

        //Update Pan Direction Display
        PanDirection.transform.Rotate(0.0f, 0.0f, 10.0f);
    }

    public void TiltDown()
    {
        playerOverhead[playerNum].m_LookAt = dummy.transform;

        if (xTilt < 0)
        {
            dummy.transform.Rotate(10.0f, 0.0f, 0.0f, Space.Self);
            xTilt += 10;
            TiltDirection.transform.Rotate(0.0f, 0.0f, 20.0f);
        }

    }

    public void TiltUp()
    {
        playerOverhead[playerNum].m_LookAt = dummy.transform;

        if (xTilt > -90)
        {
            dummy.transform.Rotate(-10.0f, 0.0f, 0.0f, Space.Self);
            xTilt -= 10;
            TiltDirection.transform.Rotate(0.0f, 0.0f, -20.0f);
        }
    }

    //Same-Player Dummy Reset
    public void ResetDummy()
    {
        playerOverhead[playerNum].m_LookAt = dummy.transform;

        dummy.transform.position = ship[playerNum].transform.position;
        dummy.transform.Translate(0.0f, 12.0f, 0.0f, Space.World);

        dummy.transform.rotation = ship[playerNum].transform.rotation;
        playerOverhead[playerNum].m_Lens.FieldOfView = 60;
        xTilt = 0;

        //Update Position Text
        cameraPos = MapLoad.shipInfo[index].GetCurrentPosition();
        positionTexts[0].text = (cameraPos.x + 34).ToString();
        positionTexts[1].text = ((cameraPos.y - 32) * -1).ToString();
        positionTexts[2].text = cameraPos.z.ToString();

        ResetCompass();
    }

    //Reset Dummy to Next Player
    public void UpdateDummy()
    {
        playerOverhead[playerNum].m_LookAt = dummy.transform;
        int temp = playerNum;

        if (playerIndex < maxPlayers - 1)
        {
            //Update Player Number
            playerIndex++;
            playerNum = players[playerIndex];

            //Change Camera
            playerOverhead[temp].Priority = 0;
            playerOverhead[playerNum].Priority = 1;
            playerOverhead[playerNum].m_LookAt = dummy.transform;

            //Set Dummy Position
            dummy.transform.rotation = ship[playerNum].transform.rotation;
            dummy.transform.position = ship[playerNum].transform.position;
            dummy.transform.Translate(0.0f, 12.0f, 0.0f, Space.World);
            playerOverhead[playerNum].m_Lens.FieldOfView = 60;
        }
        else if (playerIndex == maxPlayers - 1)
        {
            //Update Player Number
            playerIndex = 0;
            playerNum = players[0];

            //Change Camera
            playerOverhead[temp].Priority = 0;
            playerOverhead[playerNum].Priority = 1;
            playerOverhead[playerNum].m_LookAt = dummy.transform;
            
            //Set Dummy Position
            dummy.transform.rotation = ship[playerNum].transform.rotation;
            dummy.transform.position = ship[playerNum].transform.position;
            dummy.transform.Translate(0.0f, 12.0f, 0.0f, Space.World);
            playerOverhead[playerNum].m_Lens.FieldOfView = 60;
        }

        //Disable GUI and Reset Tilt
        GUI = false;
        xTilt = 0;

        //Update Position Text
        cameraPos = MapLoad.shipInfo[index].GetCurrentPosition();

        positionTexts[0].text = (cameraPos.x + 34).ToString();
        positionTexts[1].text = ((cameraPos.y - 32) * -1).ToString();
        positionTexts[2].text = cameraPos.z.ToString();

        ResetCompass();
    }

    public void ResetCompass()
    {
        pan = ship[playerNum].transform.rotation.eulerAngles;
        PanDirection.transform.rotation = Quaternion.identity;
        PanDirection.transform.Rotate(0.0f, 0.0f, -pan.y);

        TiltDirection.transform.rotation = Quaternion.identity;
    }

    //Camera zooming function
    void Zoom(float increment)
    {
        playerOverhead[playerNum].m_LookAt = dummy.transform;

        if (overhead)
        {
            playerOverhead[playerNum].m_Lens.FieldOfView = Mathf.Clamp(playerOverhead[playerNum].m_Lens.FieldOfView - (increment * 10), minZoom, maxZoom);
        }
        else
        {
            playerDefault[playerNum].m_Lens.FieldOfView = Mathf.Clamp(playerDefault[playerNum].m_Lens.FieldOfView - (increment * 10), minZoom, maxZoom);
        }

        Debug.Log("Zoom = " + playerOverhead[playerNum].m_Lens.FieldOfView);
    }

    //Dice-Based Zooming
    public void DetermineZoom()
    {
        float diceAmount = (float)MapLoad.diceVals[1];

        if (MapLoad.diceVals[0] <= 0)
        {
            dice = 1;
        }

        if (MapLoad.diceIndex == 0 && navCount > 0)             //Nav Dice
        {
            Zoom(0.0f);

            navCount--;

            if (navCount <= 0)
            {
                dice = 1;
            }
        }
        else if (dice == 1)                                 //Compass Dice
        {
            if (MapLoad.diceVals[1] <= 0)
            {
                Zoom(0.0f);
            }
            else
            {
                Zoom(-diceAmount * 1.5f);
            }
            
            dice = 2;
        }
        else                                                //Other Dice
        {
            Zoom(0.0f);
        }
    }
}
