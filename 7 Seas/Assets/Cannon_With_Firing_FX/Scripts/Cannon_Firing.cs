using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cannon_Firing : MonoBehaviour
{
    public GameObject CannonObject;
    public AnimationClip CannonFireAnim;
    public ParticleSystem CannonMuzzleFlash;
    public Light MuzzleFlashLight;
    public ParticleSystem SparkParticles;
    public ParticleSystem SmokeParticles;
    public GameObject FuseObject;
    public GameObject FuseCentre;
    public ParticleSystem FuseSmokeParticles;
    public GameObject FuseSmokeParticlesNode;
    public Light FuseLight;
    public AudioSource CannonFireAudio;
    public AudioSource BurningFuseAudio;

    private Renderer fuseObjectRenderer;

    private float offset = 0;
    private float fuselightintensity = 0.6f;
    private float explodeset = 0;
    private float explodehalt = 0;
    private float cannonfired = 0;
    private float fadeStart = 3;
    private float fadeEnd = 0;
    private float fadeTime = 1;
    private float t = 0.0f;
    private float fuseLit = 0;

    //OUR VARIABLES
    public Camera currentCamera;
    public int speed;
    public float friction;
    public float lerpSpeed;
    float xDegrees;
    float yDegrees;
    Quaternion fromRotation;
    Quaternion toRotation;
    Camera camera;
    public bool isSelected;
    //float lastSliderXVal;
    //float lastSliderYVal;

    public GameObject cannonBall;
    Rigidbody cannonBallRB;
    public Transform shotPos;
    public float firePower;
    public float powerScale;
    public Slider sliderX;
    public Slider sliderY;
    public Button fireBtn;
    public Button reloadBtn;
    //public float timer = 46;
    //public string NewLevel = "EndScreen";
    //public Camera cannonballFollowingCamera;
    public int cannonNumber;
    public int shotsLeft;
    public float[,] cannonArray = new float[15, 15]; // 2d array of the cannon numbers of size 15 on both
    bool canReload;
    //ineffcient way to set up short/long range cannons - fix w/ making a public global array of all cannons
    // Replace this with a arrayprefs2 later
    private int[] shortLongRange = new int[15] { 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0 };
    public Text rangeType;
    public Text cannonShots;
    public Text absorb;
    private int[] cannonType =  new int[5];

    void Start()
    {
        /*
        if (CannonMinigame.currShip == 1)
        {
            cannonType = CannonMinigame.shipsInfo[0].GetCannons();
        }
        else
        {
            cannonType = CannonMinigame.shipsInfo[1].GetCannons();
        }*/

        cannonType = new int[] { 1, 1, 1, 2, 2 };

        MuzzleFlashLight.intensity = 0;
        fuseObjectRenderer = FuseObject.GetComponent<Renderer>();

        //int gamesPlayed = PlayerPrefs.GetInt("ConsecutiveGamesPlayed");

        //PlayerPrefs.SetInt("ConsecutiveGamesPlayed", gamesPlayed + 1);
        //PlayerPrefs.Save();

        //Debug.Log(PlayerPrefs.GetInt("ConsecutiveGamesPlayed"));

        //OUR CODE
        camera = Camera.main;

        //lastSliderXVal = 0.5f;
        //lastSliderYVal = 0.5f;

        Button btnToFire = fireBtn.GetComponent<Button>();
        btnToFire.onClick.AddListener(FuseStart);

        //Button btnToReload = reloadBtn.GetComponent<Button>();
        //btnToReload.onClick.AddListener(Reload);
        canReload = false;

        //StartCoroutine(LoadLevelAfterTime(timer));
        for (int i = 0; i < 15; ++i)
        {
            cannonArray[i, 0] = .5f;
            cannonArray[i, 1] = .5f;
        }
        //Debug.Log("Cannon number is " + cannonNumber);

        if (isSelected)
        {
            if (cannonType[cannonNumber] == 1)
            {
                rangeType.GetComponent<Text>().text = "Short range cannon";
                cannonShots.GetComponent<Text>().text = "Cannon Shots: " + shotsLeft;
            }
            else if (cannonType[cannonNumber] == 2)
            {
                rangeType.GetComponent<Text>().text = "Long range cannon";
                cannonShots.GetComponent<Text>().text = "Cannon Shots: " + shotsLeft;
            }
            else
            {
                rangeType.GetComponent<Text>().text = "INACTIVE";
                cannonShots.GetComponent<Text>().text = "Cannon Shots: 0";
                shotsLeft = 0;
            }
        }
    }


    void Update()
    {
        //OUR CODE
        /*
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.tag == "Cannon")
            {
                if (Input.GetMouseButton(0))
                {
                    xDegrees -= Input.GetAxis("Mouse Y") * speed * friction;
                    yDegrees += Input.GetAxis("Mouse X") * speed * friction;
                    fromRotation = transform.rotation;
                    toRotation = Quaternion.Euler(0, yDegrees, xDegrees);
                    transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);

                }
            }
        }
        */

        //if statement to activate/deactive cannon based on if the camera is active
        //if (currentCamera == Camera.main)
        //{

        //Stops the cannon from moving
        if (isSelected)
        {
            transform.localEulerAngles = new Vector3(0, (sliderX.value - 0.5f) * speed,
                                            -1 * (sliderY.value - 0.5f) * speed / 2);
            // saves current angle in a array
            cannonArray[cannonNumber, 0] = sliderX.value;
            cannonArray[cannonNumber, 1] = sliderY.value;

            if (cannonType[cannonNumber] == 1)
            {
                rangeType.GetComponent<Text>().text = "Short range cannon";
                cannonShots.GetComponent<Text>().text = "Cannon Shots: " + shotsLeft;
            }
            else if (cannonType[cannonNumber] == 2)
            {
                rangeType.GetComponent<Text>().text = "Long range cannon";
                cannonShots.GetComponent<Text>().text = "Cannon Shots: " + shotsLeft;
            }
            else
            {
                rangeType.GetComponent<Text>().text = "INACTIVE";
                cannonShots.GetComponent<Text>().text = "Cannon Shots: 0";
                shotsLeft = 0;
            }
        }

        FuseSmokeParticlesNode.transform.position = new Vector3(FuseCentre.transform.position.x, FuseCentre.transform.position.y, FuseCentre.transform.position.z);

        fuselightintensity = (Random.Range(0.2f, 0.4f));
        FuseLight.intensity = fuselightintensity;


        if (explodeset == 1)
        {
            FireCannon();
        }
        // }
    }

    public void CannonSelected(bool selected)
    {
        // loads current val from array
        if (selected)
        {
            //Debug.Log(cannonNumber);
            //loading old slider values
            sliderX.value = cannonArray[cannonNumber, 0];
            sliderY.value = cannonArray[cannonNumber, 1];
            cannonArray[cannonNumber, 0] = sliderX.value;
            cannonArray[cannonNumber, 1] = sliderY.value;
            isSelected = true;
        }
        else
        {
            //Debug.Log("saving: " + cannonNumber);
            isSelected = false;
            //saving old slider values
            //cannonArray[cannonNumber, 0] = sliderX.value;
           // cannonArray[cannonNumber, 1] = sliderY.value;
        } 
    }

    void FuseStart()
    {
        //if statement to activate/deactive cannon based on if the camera is active
        if (currentCamera == Camera.main && shotsLeft > 0)
        {
            if (cannonfired != 1)
            {
                absorb.gameObject.SetActive(false);

                if (fuseLit != 1)
                {
                    offset = 0;
                    StartCoroutine("Fuse");
                    BurningFuseAudio.Play();
                    FuseCentre.SetActive(true);
                    FuseSmokeParticlesNode.SetActive(true);
                    explodehalt = 0;
                    explodeset = 0;
                }

            }
        }

    }

    public void Reload()
    {
        // if statement for the 5s delay to see if its good to reload
        //Debug.Log(canReload);
        //if (canReload)
        //{
            explodehalt = 1;
            explodeset = 0;
            offset = 0.5f;
            FuseObject.SetActive(true);
            FuseCentre.SetActive(false);
            fuselightintensity = 0;
            // FuseObject.GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(0,0));

            //Vector2 offsetVector = new Vector2 (0, 0);
            if (fuseObjectRenderer != null) fuseObjectRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, 0));

            SparkParticles.Clear();
            FuseSmokeParticlesNode.SetActive(false);
            BurningFuseAudio.Stop();
            MuzzleFlashLight.intensity = 0;
            cannonfired = 0;
            fuseLit = 0;
       //}
    }

    IEnumerator Fuse()
    {
        while (offset < 0.43f)
        {
            offset += (Time.deltaTime * 0.11f);
            // FuseObject.GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(offset,0));

            fuseObjectRenderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));

            fuseLit = 1;
            yield return 0;
        }

        if (explodehalt != 1)
        {
            explodeset = 1;
        }

        offset = 0;
    }

    //IEnumerator LoadLevelAfterTime(float timer)
    //{
    //    yield return new WaitForSeconds(timer);
    //    if (PlayerPrefs.GetInt("ConsecutiveGamesPlayed") >= 10)
    //    {
    //        GameObject UnityAdsManager = GameObject.Find("UnityAdsManager");
    //        UnityAdsManager AdsScript = UnityAdsManager.GetComponent<UnityAdsManager>();
    //        AdsScript.PlayInterstitialAd();
    //        AudioListener.volume = 0f;
    //        PlayerPrefs.SetInt("ConsecutiveGamesPlayed", 0);
    //        PlayerPrefs.Save();
    //    }
    //    SceneManager.LoadScene(NewLevel);
    //}

    void FireCannon()
    {
        FuseCentre.SetActive(false);
        FuseSmokeParticles.Stop();
        fuselightintensity = 0;
        explodeset = 0;
        CannonMuzzleFlash.Play();
        SparkParticles.Play();
        SmokeParticles.Play();
        CannonFireAudio.Play();
        StartCoroutine("FadeLight");
        // CannonObject.transform.GetComponent<Animation>().GetComponent.<Animation>().Play("CannonFireAnim");

        CannonObject.GetComponent<Animation>().Play();

        cannonfired = 1;

        //OUR CODE
        if (cannonType[cannonNumber] == 1)
        {
            //Debug.Log("1)Cannon number " + cannonNumber);
            //Debug.Log("1)Short range: true");
            GameObject cannonBallCopy = Instantiate(cannonBall, shotPos.position, shotPos.rotation) as GameObject;

            cannonBallCopy.GetComponent<scoreUpdate>().absorbText = absorb;

            cannonBallRB = cannonBallCopy.GetComponent<Rigidbody>();
            cannonBallRB.AddForce(shotPos.transform.forward * (firePower * powerScale));

            shotsLeft--;
        }
        else if (cannonType[cannonNumber] == 2)
        {
            //Debug.Log("1)Cannon number " + cannonNumber);
            //Debug.Log("1)Short range: true");
            GameObject cannonBallCopy = Instantiate(cannonBall, shotPos.position, shotPos.rotation) as GameObject;

            cannonBallCopy.GetComponent<scoreUpdate>().absorbText = absorb;

            cannonBallRB = cannonBallCopy.GetComponent<Rigidbody>();
            cannonBallRB.AddForce(shotPos.transform.forward * firePower);

            shotsLeft--;
        }

        //cannonballFollowingCamera.enabled = true;
        //cannonballFollowingCamera.transform.Translate(cannonBallCopy.transform.position);

        //cannonballFollowingCamera.enabled = true;
        //cannonballFollowingCamera.transform.Translate(cannonBallCopy.transform.position);

        if (cannonType[cannonNumber] != 0)
        {
            cannonShots.GetComponent<Text>().text = "Cannon Shots: " + shotsLeft;
        }
    }

    IEnumerator FadeLight()
    {
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            MuzzleFlashLight.intensity = Mathf.Lerp(fadeStart, fadeEnd, t / fadeTime);
            yield return 0;
        }

        t = 0;
    }
    /*
    // allows you to call reloaddelay from a button
    void reloadDelayWrapper()
    {
        Debug.Log("test");
        ReloadDelay();
    }

    //reload delay of 5s
    IEnumerator ReloadDelay()
    {
        Debug.Log("test");
        canReload = false;
        yield return new WaitForSeconds(5f);
        canReload = true;
        Reload();
    }
    */



}