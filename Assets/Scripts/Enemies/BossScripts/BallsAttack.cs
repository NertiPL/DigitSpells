using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class BallsAttack : MonoBehaviour
{
    public float speed;
    public float dmg;
    bool go = false;

    Rigidbody rb;
    private void Start()
    {
        if(transform.parent.parent!=null)
        {
            transform.parent.parent = transform.parent.parent.parent;
        }
        transform.parent.eulerAngles = new Vector3(0f, Random.Range(0, 361), 0f);
        if (speed == 0)
        {
            speed = transform.parent.GetChild(0).GetComponent<BallsAttack>().speed;
        }
        if (dmg == 0)
        {
            dmg = transform.parent.GetChild(0).GetComponent<BallsAttack>().dmg;
        }
        rb = GetComponent<Rigidbody>();        
        Invoke("Go", 0.5f);
        Invoke("SelfDestroy", 5f);
    }

    private void Update()
    {
        if (go)
        {
            rb.velocity += transform.forward * 1 * speed * Time.fixedDeltaTime;
        }
    }
    void Go()
    {
        go=true;
    }

    void SelfDestroy()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerController>().GetHit(dmg);
            Destroy(gameObject);
        }
    }
}
