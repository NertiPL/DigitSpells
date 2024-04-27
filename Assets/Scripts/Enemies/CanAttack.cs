using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttack : MonoBehaviour
{ 

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && canCollide && !player.GetComponent<PlayerController>().isDashing && !attacked)
        {
            canMove = false;
            attacked = true;
            isAlreadyWalking = false;
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
}
