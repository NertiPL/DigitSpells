using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public float speed;
    public float dmg;

    Rigidbody rb;

    bool followPlayer = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 6f);
        Invoke("StopFollow", 2f);

        followPlayer = true;
    }
    private void Update()
    {
        if (followPlayer)
        {
            var targetDirection = GameManager.instance.player.transform.position - transform.position;
            var step = speed * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, step);
            var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            rb.velocity += transform.forward * 1 * speed * Time.fixedDeltaTime;
        }
    }

    void StopFollow()
    {
        followPlayer = false;
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
