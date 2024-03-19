using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int howMany;
    public Spells spell;
    public float speed;
    public float dmg;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        rb.velocity += transform.forward * 1 * speed * Time.fixedDeltaTime;
    }

}
