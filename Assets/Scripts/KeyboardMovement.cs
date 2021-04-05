using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour
{
    public int velocity;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.localPosition;
        pos.y += Input.GetAxis("Vertical") * Time.deltaTime * velocity;
        pos.x += Input.GetAxis("Horizontal") * Time.deltaTime * velocity;
        transform.localPosition = pos;
    }
}
