using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class Skeleton : Enemy
{
    GameObject staff;
    public bool isMage=false;
    bool attacked=false;
    bool isAlreadyWalking=false;


    public GameObject MageSkellyPrefab;
    public GameObject MageSkellySpell;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameManager.instance.player.gameObject;

        isMage = true;


        /*if (!isMage)
        {
            switch (Random.Range(0, 2))
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
            }

            if (isMage)
            {
                var newSkelly=Instantiate(MageSkellyPrefab, transform.position, transform.rotation);
                newSkelly.GetComponent<Skeleton>().isMage = true;
                Destroy(gameObject);
            }
        } */

        if (isMage)
        {
            staff = transform.GetChild(3).gameObject;
        }

    }

    public override void WalkAnim()
    {
        if (canMove && !isAlreadyWalking && sees)
        {
            animator.Play("SkellyWalk");
            isAlreadyWalking = true;
        }

        if (!sees)
        {
            animator.Play("SkellyIdle");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isInAttackRange && canCollide && !player.GetComponent<PlayerController>().isDashing && !attacked)
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

    void AttackRange()
    {
        animator.Play("SkellyAttack");      
    }


    void AttackMeele()
    {
        Debug.Log("attacked");
        switch (Random.Range(0, 2))
        {
            case 0:
                animator.Play("SkellyAttack1");
                break;
            default:
                animator.Play("SkellyAttack2");
                break;
        }
        
    }

    public void EndAttack()
    {
        attacked = false;
        player.GetComponent<PlayerController>().GetHit(dmg);
        canMove = true;
    }

    public void EndThrowSpell()
    {
        attacked = false;
        canMove = true;
    }

    public void MidSpellAnim()
    {
        Instantiate(MageSkellySpell,staff.transform.GetChild(0).position,transform.rotation);
    }
}
