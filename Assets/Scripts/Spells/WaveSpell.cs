using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpell : MonoBehaviour
{
    public float speed = 2;
    public float timeOfMaxState = 2;
    public Spells spell;

    public Animator animator;

    public Rigidbody rb;
    private void Start()
    {
        timeOfMaxState = 2;
        rb = GetComponent<Rigidbody>();
        animator.Play("WaveRisingAnim");

        if (spell.lvl >= 3)
        {
            timeOfMaxState = 3;
        }
        if (spell.lvl >= 5)
        {
            timeOfMaxState = 4;
        }

        Invoke("WaveFall",timeOfMaxState);
    }

    private void Update()
    {
        rb.velocity += transform.forward * speed * Time.deltaTime;
        Debug.Log(rb.velocity);

    }

    void WaveFall()
    {
        animator.Play("WaveFallingAnim");
    }

    public void DestroyWave()
    {
        Destroy(gameObject);
    }

}
