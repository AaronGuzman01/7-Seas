using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_movement : MonoBehaviour
{
    private Transform attacher;
    public int height = 10;//max height of Box's movement
    public float yCenter = 0f;

    void Start()
    {
        attacher = this.transform.Find("Target");
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, yCenter 
            + Mathf.PingPong(Time.time * 2, height) - height / 2f, transform.position.z);
            //move on y axis only
                                                                                                                                                  //Box is moving with Mathf.PingPong (http://docs.unity3d.com/Documentation/ScriptReference/Mathf.PingPong.html)
    }
}
