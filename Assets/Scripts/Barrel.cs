using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spell")
        {
            DestroyBarrel();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag =="Spell")
        {
            DestroyBarrel();
        }
    }

    void DestroyBarrel()
    {
        GiveGems();
        Destroy(gameObject);
    }

    void GiveGems()
    {
        for (int i = 0; i < Random.Range(0, 3); i++)
        {
            GameManager.instance.numbersEq.Add(Random.Range(0, 21));
            GameManager.instance.UpdateEqNums();
        }
    }
}
