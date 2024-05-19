using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float floatSpeed;
    bool goUp = true;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed));
        if(transform.position.y < startPos.y - 0.5f && !goUp)
        {
            goUp = true;
        }
        else if(transform.position.y > startPos.y+0.5f && goUp)
        {
            goUp = false;
        }

        if(goUp)
        {
            transform.position += new Vector3(0, floatSpeed, 0);
        }
        else
        {
            transform.position += new Vector3(0, -floatSpeed, 0);
        }
    }
}
