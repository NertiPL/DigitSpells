using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    bool isMage;
    bool isInRange;


    public GameObject rangeMage;
    public GameObject rangeMeele;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameManager.instance.player.gameObject;
        /* switch(Random.Range(0, 2))
         {
             case 0:
                 isMage = false;
                 break;
             case 1:
                 isMage = true;
                 break;
             default:
                 isMage = false;
                 break;
         }*/

        isMage = false;

        if (isMage)
        {
            rangeMage.SetActive(true);
            rangeMeele.SetActive(false);
        }
        else
        {
            rangeMage.SetActive(false);
            rangeMeele.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && canCollide && !player.GetComponent<PlayerController>().isDashing)
        {
            if (isMage)
            {
                AttackRange();
            }
            else
            {
                AttackMeele();
            }
        }
    }

    void AttackRange()
    {
        player.GetComponent<PlayerController>().GetHit(dmg);
        
    }

    void AttackMeele()
    {
        Debug.Log("attacked");
        animator.Play("SkellyAttackAnim");
        player.GetComponent<PlayerController>().GetHit(dmg);
    }
}
