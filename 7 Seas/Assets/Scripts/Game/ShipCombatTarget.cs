using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCombatTarget : MonoBehaviour
{
    public bool treasure;
    List<Vector3> positions = new List<Vector3>();
    int count = 0;
    public bool started = false;
    public bool monster = false;
    bool moving = false;
    Vector3 position;
    float time = 1;
    float sec = 3;

    void Start()
    {
        float x, y;

        if (treasure)
        {
            x = 2f;
            y = 0;
        }
        else
        {
            x = 1.5f;
            y = 1.5f;
        }

        if (monster)
        {
            time = 0.7f;
            sec = 5f;

            transform.localPosition = new Vector3(0, 1, y);

            positions.Add(new Vector3(0, 1, y));
            positions.Add(new Vector3(-1, 1, y));
            positions.Add(new Vector3(1, 1, y));
            positions.Add(new Vector3(-1, 1.5f, y));
            positions.Add(new Vector3(1, 1.5f, y));
            positions.Add(new Vector3(0, 1.5f, y));
        }
        else if (treasure)
        {
            transform.localPosition = new Vector3(x, x, y);

            positions.Add(new Vector3(x, x, 0));
            positions.Add(new Vector3(x, x, x));
            positions.Add(new Vector3(x, x, -x));
            positions.Add(new Vector3(x, 2 * x, x));
            positions.Add(new Vector3(x, 2 * x, -2));
            positions.Add(new Vector3(x, 2 * x, 0));
        }
        else
        {
            transform.localPosition = new Vector3(x, 2 - transform.parent.localPosition.y, 0);

            positions.Add(new Vector3(x, transform.localPosition.y, 0));
            positions.Add(new Vector3(x, transform.localPosition.y, -2));
            positions.Add(new Vector3(x, transform.localPosition.y, 2));
            positions.Add(new Vector3(x, transform.localPosition.y + 1.5f, 0));
            positions.Add(new Vector3(x, transform.localPosition.y + 3.5f, -2));
            positions.Add(new Vector3(x, transform.localPosition.y + 3.5f, 2));
        }
    }

    void Update()
    {
        StartCoroutine(MoveTargetToRandomPosition());

        if (moving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, position, Time.deltaTime * time);

            if (Vector3.Distance(transform.localPosition, position) < 0.001f)
            {
                moving = false;
                count = 0;
            }
        }
    }

    public IEnumerator MoveTargetToRandomPosition()
    {
        if (started & !moving && count == 0)
        {
            count++;

            yield return new WaitForSeconds(sec);

            int index = Random.Range(0, positions.Count);

            moving = true;
            position = positions[index];

            //transform.localPosition = new Vector3(1.5f, Random.Range(1f, 4f), Random.Range(-2f, 2f));
        }

        yield return null;
    }
}
