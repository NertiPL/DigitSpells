using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : MonoBehaviour
{
    public GameObject boss;
    void Awake()
    {
        boss.GetComponent<MonoBehaviour>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boss.GetComponent<MonoBehaviour>().enabled = true;
        }
    }
}
