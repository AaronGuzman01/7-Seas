using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scoreUpdate : MonoBehaviour
{
    public static int absorb = 0;
    public Text MyText;
    public Text absorbText;
    public int score;
    int highestScore;
    public PointsManager manager;
    private bool hit;
    private GameObject explosion;
    private ParticleSystem explosionEffect;
    private AudioSource explosionSFX;
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        manager = GetComponent<PointsManager>();
        manager = FindObjectOfType<PointsManager>();
        this.hit = false;

        explosion = GameObject.Find("ScoreExplosion");
        explosionEffect = explosion.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        explosionSFX = explosion.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
  
    void OnTriggerEnter(Collider other)
    {
        /*
        if (PlayerPrefs.GetString("Enemy").Equals("Player") && other.gameObject.layer == 9) // Target layer is 9
        {
            ShipCombatTarget teleporter = other.gameObject.GetComponentInParent<ShipCombatTarget>();
            //teleporter.MoveTargetToRandomPosition();
        }
        if (PlayerPrefs.GetString("Enemy").Equals("Treasure") && other.gameObject.layer == 9)
        {
            ShipCombatTarget teleporter = other.gameObject.GetComponentInParent<ShipCombatTarget>();
            
        }
        */
        if (PlayerPrefs.GetString("Enemy").Equals("Player") && absorb > 0 && 
            (other.CompareTag("+1") || other.CompareTag("+2") || other.CompareTag("+3") || other.CompareTag("+4")))
        {
            absorb--;
            absorbText.text = "Damage Absorbed: " + other.tag.Substring(1);
            absorbText.gameObject.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Enemy").Equals("Treasure") && 
            (other.CompareTag("+1") || other.CompareTag("+2") || other.CompareTag("+3") || other.CompareTag("+4")))
        {
            if (PlayerPrefs.GetInt("TimesHit") >= 0)
            {
                PlayerPrefs.SetInt("TimesHit", PlayerPrefs.GetInt("TimesHit") + 1);
                PlayerPrefs.SetInt("ShipHits", PlayerPrefs.GetInt("ShipHits") - PlayerPrefs.GetInt("TimesHit"));

                if (PlayerPrefs.GetInt("ShipHits") < 0)
                {
                    PlayerPrefs.SetInt("ShipHits", 0);
                }
            }
        }
        else if (other.CompareTag("+1") && (this.hit == false))
        {
            explosion.transform.position = transform.position;
            explosionEffect.startSize = 1;
            explosionSFX.Play();
            explosionEffect.Stop();
            explosionEffect.Clear();
            explosionEffect.Play(); 
            this.hit = true;
            score = score + 1;
            manager.AddPoints(score);
            Debug.Log(score + " hit registered");
            PlayerPrefs.SetInt("Hit", 1);

            if (PlayerPrefs.GetString("Enemy").Equals("Monster"))
            {
                other.gameObject.GetComponentInParent<Monstermovement>().MonsterHit();
            }
        }
        else if (other.CompareTag("+2") && (this.hit == false))
        {
            explosion.transform.position = transform.position;
            explosionEffect.startSize = 3;
            explosionSFX.Play();
            explosionEffect.Stop();
            explosionEffect.Clear();
            explosionEffect.Play();
            this.hit = true;
            score = score + 2;
            manager.AddPoints(score);
            Debug.Log(score + " hit registered");
            PlayerPrefs.SetInt("Hit", 2);

            if (PlayerPrefs.GetString("Enemy").Equals("Monster"))
            {
                other.gameObject.GetComponentInParent<Monstermovement>().MonsterHit();
            }
        }
        else if (other.CompareTag("+3") && (this.hit == false))
        {
            explosion.transform.position = transform.position;
            explosionEffect.startSize = 9;
            explosionSFX.Play();
            explosionEffect.Stop();
            explosionEffect.Clear();
            explosionEffect.Play();
            this.hit = true;
            score = score + 3;
            manager.AddPoints(score);
            Debug.Log(score + " hit registered");
            PlayerPrefs.SetInt("Hit", 3);

            if (PlayerPrefs.GetString("Enemy").Equals("Monster"))
            {
                other.gameObject.GetComponentInParent<Monstermovement>().MonsterHit();
            }
        }
        else if (other.CompareTag("+4") && (this.hit == false))
        {
            explosion.transform.position = transform.position;
            explosionEffect.startSize = 15;
            explosionSFX.Play();
            explosionEffect.Stop();
            explosionEffect.Clear();
            explosionEffect.Play();
            this.hit = true;
            score = score + 4;
            manager.AddPoints(score);
            Debug.Log(score + " hit registered");
            PlayerPrefs.SetInt("Hit", 4);

            if (PlayerPrefs.GetString("Enemy").Equals("Monster"))
            {
                other.gameObject.GetComponentInParent<Monstermovement>().MonsterHit();
            }
        }
        else { Debug.Log(" No Tag"); }
        PlayerPrefs.Save();
    }

   
}
