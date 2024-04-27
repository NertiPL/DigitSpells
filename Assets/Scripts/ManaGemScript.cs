using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaGemScript : MonoBehaviour
{
    float number;

    private void Start()
    {
        Invoke("DelObject", 15f);
    }
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0.25f, 0f));
    }

    void DelRB()
    {
        Destroy(GetComponent<Rigidbody>());
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Collider>().excludeLayers = 0;
    }

    void DelObject()
    {
        Destroy(gameObject);
    }

    public void GiveNum(float num)
    {
        number = num;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Invoke("DelRB", 1.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.instance.numbersEq.Add(number);
            GameManager.instance.UpdateEqNums();
            Destroy(gameObject);
        }
    }
}
