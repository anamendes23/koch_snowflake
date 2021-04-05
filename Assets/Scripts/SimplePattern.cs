using System;
using System.IO;
using UnityEngine;

public class SimplePattern : MonoBehaviour
{
    Vector3 pos;
    float step;
    // Start is called before the first frame update
    void Start()
    {
        step = 50 * Time.fixedDeltaTime;
        pos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float diff = Vector3.Distance(transform.position, pos);
        Debug.Log(diff + " speed: " + step);
        if (diff > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
        }
        else
        {
            pos.x *= 2;
            pos.y *= 2;
            Debug.Log(pos.x + " " + pos.y);
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
        }
    }
}
