using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;

    bool wasOpened = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.Play("ChestOpenUp");

            if (!wasOpened)
            {
                GiveGems();
            }
            wasOpened = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.Play("ChestCloseUp");
        }
    }

    void GiveGems()
    {
        for(int i = 0; i <Random.Range(0, 5); i++)
        {
            GameManager.instance.numbersEq.Add(Random.Range(0, 21));
            GameManager.instance.UpdateEqNums();
        }
    }
}
