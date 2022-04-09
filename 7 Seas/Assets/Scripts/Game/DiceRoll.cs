using System.Collections;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public Animator[] cups;
    public GameObject[] die;
    public AudioSource diceAudio;
    public AudioClip[] clips;

    GameObject[] currDie;
    int cupIndex;
    int currPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currDie = new GameObject[5];
        string cupStr = PlayerPrefs.GetString("Cup");
        currPlayer = MapLoad.playerNum;

        if (cupStr.Equals("Captain"))
        {
            cupIndex = 2;
        }
        else if (cupStr.Equals("Seaman"))
        {
            cupIndex = 1;
        }
        else
        {
            cupIndex = 0;
        }
    }

    void Update()
    {
        if (CameraController.cups && !MapLoad.isRolling)
        {
            cups[cupIndex].Play("Cup", -1, 0f);

            cups[cupIndex].gameObject.SetActive(true);

            CameraController.cups = false;

            MapLoad.isRolling = true;

            StartCoroutine(ProcessDice());
            StartCoroutine(DiceSounds());
        }

        if (currPlayer != MapLoad.playerNum)
        {
            cups[cupIndex].gameObject.SetActive(false);

            currPlayer = MapLoad.playerNum;
        }
    }

    IEnumerator DiceSounds()
    {
        yield return new WaitForSeconds(1.2f);
        diceAudio.PlayOneShot(clips[0]);
        yield return new WaitForSeconds(2.22f);
        diceAudio.PlayOneShot(clips[1], 0.3f);
        yield return new WaitForSeconds(1.18f);
        diceAudio.PlayOneShot(clips[1], 0.3f);
        yield return new WaitForSeconds(1.18f);
        diceAudio.PlayOneShot(clips[1], 0.3f);
        yield return new WaitForSeconds(1.18f);
        diceAudio.PlayOneShot(clips[1], 0.3f);
        yield return new WaitForSeconds(1.18f);
        diceAudio.PlayOneShot(clips[1], 0.3f);
    }

    IEnumerator ProcessDice()
    {
        Transform transform;
        GameObject currDice;
        int count = 0;

        yield return new WaitForSeconds(3.5f);

        foreach (GameObject dice in die)
        {
            currDice = Instantiate(dice, dice.transform.parent);

            transform = currDice.transform;

            currDice.SetActive(true);

            currDie[count] = currDice;

            count++;

            yield return new WaitForSeconds(1.2f);

            Destroy(currDice);
        }

        yield return new WaitForSeconds(2);
    }
}
