using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;
    void Start()
    {
        rb= GetComponent<Rigidbody>();
    }

    void Update()
    {

        Movement();
    }

    void Movement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity += transform.forward * 1 * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity += transform.forward * -1 * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity += transform.right * -1 * speed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity += transform.right * 1 * speed * Time.fixedDeltaTime;
        }



    }
}
