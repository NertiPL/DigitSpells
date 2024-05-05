using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShoot : MonoBehaviour
{
    public float step;

    Animator animator;

    Vector3 startingPlayerPos;

    int i = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //transform.position = transform.parent.position;
        transform.parent = transform.parent.parent;
        startingPlayerPos = GameManager.instance.player.transform.position;
        animator.Play("WebShoot");
        step = Vector3.Distance(transform.position, startingPlayerPos)/40;

        InvokeRepeating("GoTowardsPlayer", 0f, 0.03f);
    }

    void GoTowardsPlayer()
    {
        if(i == 40)
        {
            CancelInvoke("GoTowardsPlayer");
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, startingPlayerPos, step);
        i++;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            GameManager.instance.player.GetComponent<PlayerController>().speed = GameManager.instance.player.GetComponent<PlayerController>().speed / 2;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            GetSpeedBack();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.player.GetComponent<PlayerController>().speed = GameManager.instance.player.GetComponent<PlayerController>().speed / 2;
            Invoke("GetSpeedBack", 4f);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void GetSpeedBack()
    {
        GameManager.instance.player.GetComponent<PlayerController>().speed = GameManager.instance.player.GetComponent<PlayerController>().ogSpeed;
        SelfDestroy();
    }

    public void EndAnim()
    {
        i = 40;
        Invoke("SelfDestroy", 8f);
    }
    void SelfDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
