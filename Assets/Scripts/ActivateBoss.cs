using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBoss : MonoBehaviour
{
    public GameObject boss;
    Transform bossPos;
    void Awake()
    {
        boss.GetComponent<MonoBehaviour>().enabled = false;
        boss.GetComponent<Enemy>().enabled = false;
        bossPos=boss.transform;
        Invoke("GetInPos", 2f);
    }

    void GetInPos()
    {
        Debug.Log("Moved");
        boss.transform.position = bossPos.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            boss.GetComponent<MonoBehaviour>().enabled = true;
            boss.GetComponent<Enemy>().enabled = true;
        }
    }
}
