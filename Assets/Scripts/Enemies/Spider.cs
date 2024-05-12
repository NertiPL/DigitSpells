using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spider : Enemy
{ 
    public bool isAlreadyWalking = false;

    public GameObject webPrefab;

    public override void CollideAnim()
    {
        useMeleeAnim = false;
        animator.Play("SpiderAttack");
    }

    public override void WalkAnim()
    {
        if (canMove && !isAlreadyWalking && sees)
        {
            animator.Play("SpiderWalk");
            isAlreadyWalking = true;
        }

        if (!sees)
        {
            animator.Play("SpiderIdle");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isInAttackRange && canCollide && !player.GetComponent<PlayerController>().isDashing && !attacked)
        {
            canMove = false;
            attacked = true;
            isAlreadyWalking = false;
            AttackRange();
        }
    }

    void AttackRange()
    {
        animator.Play("SpiderSpit");
    }

    public void MidAttack()
    {
        player.GetComponent<PlayerController>().GetHit(dmgOnCol);
    }

    public void MidSpit()
    {
        Instantiate(webPrefab, transform.position, Quaternion.identity, transform);
    }

    public void EndAttack()
    {
        Debug.Log("end");
        attacked = false;
        canMove = true;
    }

    public void EndSpit()
    {
        Debug.Log("endspit");
        attacked = false;
        canMove = true;
    }
}
