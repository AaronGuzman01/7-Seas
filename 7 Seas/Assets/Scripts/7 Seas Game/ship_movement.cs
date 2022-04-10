using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ship_movement : MonoBehaviour
{
    public static int movespeed = 1;
    public float height;
    public bool isMoving;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.Translate(0, 0, movespeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
    }
}
