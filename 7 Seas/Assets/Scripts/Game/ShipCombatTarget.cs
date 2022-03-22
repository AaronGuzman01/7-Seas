using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCombatTarget : MonoBehaviour
{
    public bool treasure;
    List<Vector3> positions = new List<Vector3>();
    int count = 0;
    public bool started = false;
    bool moving = false;
    Vector3 position;

    void Start()
    {
        float x;

        if (treasure)
        {
            x = 2f;
        }
        else
        {
            x = 1.5f;
        }

        positions.Add(new Vector3(x, 2, 0));
        positions.Add(new Vector3(x, 2, -2));
        positions.Add(new Vector3(x, 2, 2));
        positions.Add(new Vector3(x, 3.5f, 0));
        positions.Add(new Vector3(x, 3.5f, -2));
        positions.Add(new Vector3(x, 3.5f, 2));
    }

    void Update()
    {
        StartCoroutine(MoveTargetToRandomPosition());

        if (moving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, position, Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, position) < 0.001f)
            {
                moving = false;
                count = 0;
            }
        }
    }

    public void SetStart()
    {
        started = false;
    }

    public IEnumerator MoveTargetToRandomPosition()
    {
        if (started & !moving && count == 0)
        {
            count++;

            yield return new WaitForSeconds(3);

            int index = Random.Range(0, positions.Count);

            moving = true;
            position = positions[index];

            //transform.localPosition = new Vector3(1.5f, Random.Range(1f, 4f), Random.Range(-2f, 2f));
        }

        yield return null;
    }
}
