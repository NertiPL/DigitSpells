using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSlide : MonoBehaviour
{
    Vector3 bossPos;
    Transform bossTrans;

    public float diference;
    public float diferenceTemp;

    Rigidbody rb;

    public float dashPower;

    private void Start()
    {
        bossTrans = transform.parent.parent;
        bossPos = bossTrans.position;
        rb = bossTrans.GetComponent<Rigidbody>();
        transform.parent.parent = transform.parent.parent.parent;

        transform.parent.rotation = bossTrans.rotation;

        rb.AddForce(bossTrans.forward * dashPower, ForceMode.Impulse);//rb.velocity

        Invoke("CheckDiference", 2f);
        Invoke("SelfDestroy", 10f);
    }

    void SelfDestroy()
    {
        Destroy(transform.parent.gameObject);
    }

    void CheckDiference()
    {
        diference = bossPos.x - bossTrans.position.x;

        if (diference < 0)
        {
            diference *= -1;
        }

        diferenceTemp = bossPos.z - bossTrans.position.z;

        if (diferenceTemp < 0)
        {
            diferenceTemp *= -1;
        }

        diference += diferenceTemp;

        gameObject.GetComponent<SpriteRenderer>().size += new Vector2(0,diference-15);
        gameObject.GetComponent<BoxCollider>().size += new Vector3(0,diference-15,0);
        transform.parent.position = new Vector3((bossPos.x + bossTrans.position.x) / 2, bossTrans.position.y, (bossPos.z + bossTrans.position.z) / 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.player.GetComponent<PlayerController>().rb.velocity = Vector3.zero;
            GameManager.instance.player.GetComponent<PlayerController>().speed = GameManager.instance.player.GetComponent<PlayerController>().speed / 2;
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            GameManager.instance.player.GetComponent<PlayerController>().speed = GameManager.instance.player.GetComponent<PlayerController>().ogSpeed;
    }

}
