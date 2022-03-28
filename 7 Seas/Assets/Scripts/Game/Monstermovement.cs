using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//This script handled movement and enemy health because movement needs to be halted and an animation played every time the enemy takes damage


public class Monstermovement : MonoBehaviour
{
    public int moveTowardspeed = 10;
    public float height;
    public float attackHeight=-10;
    public bool isMoving;
    public int sideMovingspeed=3;
    private int timesMoved;//times the monster has moved to the left
    private int timesAttacked;//times the monster has used its attack
    private bool justHit;//check if monster was hit during in its current lane
    float x, z;//the x vector of the monsters position
    bool movingForward;
    bool movingBackward;
    bool movingLeft;
    bool movingRight;
    bool attacking;
    bool hiding;
    bool standing;
    bool hidden = false;
    public Animation anim;
    public float health;
    public float maxHealth;
    public Slider slider;
    public GameObject FX;

    private GameObject explosion;
    private ParticleSystem explosionEffect;
    private AudioSource explosionSFX;
    private bool died = false;

    //public bool istriggered;
    Collider m_Collider;
    public int cannonDmg;//amount of damage cannons do with each hit
    // Vector3 cameraInitialPosition;
    // public float shakeMagnetude = 0.05f, shakeTime = 0.5f;
    // public Camera mainCamera;

    private int maxAttacks = 0, attacks = 0;
    private int maxStands = 0, stands = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play("Disappear");
        movingForward = true;
        movingBackward = false;
        movingLeft = true;
        movingRight = true;
        timesMoved = 0;
        timesAttacked = 0;
        health = maxHealth;
        slider.value = 100;
        //istriggered = false;
        m_Collider = GetComponent<Collider>();
        justHit = false;
        PlayerPrefs.SetInt("DamageDoneMonster", 0);
        PlayerPrefs.SetString("MonsterStatus", "Alive");
        PlayerPrefs.Save();

        explosion = GameObject.Find("ScoreExplosion");
        explosionEffect = explosion.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        explosionSFX = explosion.transform.GetChild(0).gameObject.GetComponent<AudioSource>();

        FX.transform.position = new Vector3(this.transform.position.x, 1010, this.transform.position.z);
    }

    public void StartAnimation()
    {
        anim.Play("Appear");
    }

    // Update is called once per frame
    void Update()
    {
        if(health<=0 && !died)
        {
            Image fill = slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, 0);

            died = true;

            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            anim.Play("Death");
            PlayerPrefs.SetString("MonsterStatus", "Dead");
            PlayerPrefs.Save();
            StartCoroutine(Wait());
        }/*
        else if (timesMoved >= 5) {
            SceneManager.UnloadSceneAsync("Cannon");
            SceneManager.LoadScene("CannonResults", LoadSceneMode.Additive);
        }
        */

        slider.value = health;
        x = transform.position.x;
        z = transform.position.z;

        if (isMoving)
        {
            //move monster towards ship
            if (movingForward)
            {
                if (!anim.isPlaying)
                {
                    transform.Translate(0, 0, moveTowardspeed * Time.deltaTime);
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);

                    if (x >= -5)
                    {
                        movingForward = false;
                    }
                }

            }
            else if (movingBackward)
            {
                if (!anim.isPlaying)
                {
                    transform.Translate(0, 0, -moveTowardspeed * Time.deltaTime);
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);

                    if (x <= -10)
                    {
                        movingBackward = false;
                    }
                }
            }
            else if (movingLeft)
            {
                if (!anim.isPlaying)
                {
                    transform.Translate((moveTowardspeed / 3) * Time.deltaTime, 0, 0);
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);

                    if (z <= 15)
                    {
                        movingLeft = false;
                    }
                }
            }
            else if (movingRight)
            {
                if (!anim.isPlaying)
                {
                    transform.Translate((-moveTowardspeed / 3) * Time.deltaTime, 0, 0);
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);

                    if (z >= 35)
                    {
                        movingRight = false;
                    }
                }
            }
            else if (attacking)
            {
                if (maxAttacks == 0)
                {
                    maxAttacks = Random.Range(1, 3);
                }
                else
                {
                    if (!anim.IsPlaying("Attack1"))
                    {
                        anim.Play("Attack1");

                        attacks++;

                        if (attacks >= maxAttacks)
                        {
                            attacks = 0;
                            maxAttacks = 0;

                            attacking = false;
                        }
                    }
                }
            }
            else if (hiding)
            {
                if (!hidden)
                {
                    anim.Play("Disappear");

                    hidden = true;
                }
                else
                {
                    if (!anim.isPlaying)
                    {
                        float posX = 15 + (5 * Random.Range(0, 5));

                        transform.position = new Vector3(transform.position.x, height, posX);

                        anim.Play("Appear");

                        hiding = false;
                        hidden = false;
                    }
                }
            }
            else if (standing)
            {
                if (maxStands == 0)
                {
                    maxStands = Random.Range(1, 6);
                }
                else
                {
                    if (!anim.IsPlaying("Stand"))
                    {
                        anim.Play("Stand");

                        stands++;

                        if (stands >= maxStands)
                        {
                            stands = 0;
                            maxStands = 0;

                            standing = false;
                        }
                    }
                }
            }
            else 
            {
                /*
                if (timesAttacked == timesMoved)
                {
                    if (justHit==false)
                    { anim.Play("Attack1");
                        // ShakeIt(); caused lag
                        //PlayersManager.Opponent1.Health -= 5;
                        PlayerPrefs.SetInt("DamageDoneMonster", PlayerPrefs.GetInt("DamageDoneMonster") + 5);
                        PlayerPrefs.Save();
                    }
                    else
                    {
                        anim.Play("Stand");
                    }
                    anim.CrossFadeQueued("Disappear");
                    //m_Collider.enabled = false;
                    timesAttacked++;
                }
                if ((!anim.IsPlaying("Attack1")) && (!anim.IsPlaying("Disappear")) && !anim.IsPlaying("Stand"))
                {
                    anim.CrossFadeQueued("Appear");
                    m_Collider.enabled = true;
                    transform.position = new Vector3(0, attackHeight, transform.position.z + 5); ;//used to moved monster in front of next cannon
                    justHit = false;
                    anim.CrossFadeQueued("Stand");
                    timesMoved++;
                }
                */

                GenerateRandomMovement();
            }

            if (FX)
            {
                FX.transform.position = new Vector3(this.transform.position.x, 1010, this.transform.position.z);
            }
        }
    }

    void GenerateRandomMovement()
    {
        if (!anim.isPlaying && !died)
        {
            int num = Random.Range(0, 7);

            switch (num)
            {
                case 0:
                    movingForward = true;
                    break;
                case 1:
                    movingBackward = true;
                    break;
                case 2:
                    movingLeft = true;
                    break;
                case 3:
                    movingRight = true;
                    break;
                case 4:
                    attacking = true;
                    break;
                case 5:
                    standing = true;
                    break;
                default:
                    hiding = true;
                    break;
            }
        }
    }

    public void MonsterHit()
    {
        int val = PlayerPrefs.GetInt("Hit");

        if (val > 0)
        {
            anim.Play("Hit1");
            health = health - (val * 5 * cannonDmg);
            justHit = true;

            explosion.transform.position = transform.position;
            explosionEffect.startSize = 9;
            explosionSFX.Play();
            explosionEffect.Stop();
            explosionEffect.Clear();
            explosionEffect.Play();

            movingForward = false;
            movingBackward = false;
            movingLeft = false;
            movingRight = false;
            hiding = false;
            standing = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(6);
        SceneManager.UnloadSceneAsync("Cannon");
        transform.position = new Vector3(0, transform.position.y, 0);
        SceneManager.LoadScene("CannonResults", LoadSceneMode.Additive);
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannonball") && hit)
        {
            anim.Play("Hit1");
            health = health - (multiplier * cannonDmg);
            justHit=true;

            explosion.transform.position = transform.position;
            explosionEffect.startSize = 9;
            explosionSFX.Play();
            explosionEffect.Stop();
            explosionEffect.Clear();
            explosionEffect.Play();

            movingForward = false;
            movingBackward = false;
            movingLeft = false;
            movingRight = false;
            hiding = false;
            standing = false;
        }
            //istriggered = true;

    }
    */

    /*public void ShakeIt()
    {
        cameraInitialPosition = mainCamera.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }

    void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        float cameraShakingOffsetY = Random.value * shakeMagnetude * 2 - shakeMagnetude;
        Vector3 cameraIntermadiatePosition = mainCamera.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.y += cameraShakingOffsetY;
        mainCamera.transform.position = cameraIntermadiatePosition;
    }

    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        mainCamera.transform.position = cameraInitialPosition;
    }
    */
}
