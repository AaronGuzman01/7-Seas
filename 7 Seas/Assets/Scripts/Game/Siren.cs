using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siren : MonoBehaviour
{
    public ParticleSystem sirenFX;
    List<PlayerShip> players;
    Vector3Int sirenPosition;
    int left;
    int upper;
    int right;
    int lower;

    IEnumerator WaitForSiren()
    {
        sirenFX.gameObject.SetActive(true);
        sirenFX.Play();

        yield return new WaitForSeconds(10);

        sirenFX.gameObject.SetActive(false);
    }

    public void CheckPlayersHit()
    {
        StartCoroutine(WaitForSiren());

        int count = 0;

        foreach (PlayerShip player in players)
        {
            Vector3Int playerPos = player.GetCurrentPosition();

            if (playerPos.x > left && playerPos.x < right && playerPos.y < upper && playerPos.y > lower)
            {
                if (!MapLoad.playersHit.Contains(count))
                {
                    MapLoad.playersHit.Add(count);
                }
            }

            count++;
        }
    }

    public void SetPlayers(List<PlayerShip> players)
    {
        this.players = players;
    }

    public void SetBounds(int left, int upper, int right, int lower)
    {
        this.left = left;
        this.upper = upper;
        this.right = right;
        this.lower = lower;
    }

    public void SetPosition(Vector3Int pos)
    {
        sirenPosition = pos;
    }
}
