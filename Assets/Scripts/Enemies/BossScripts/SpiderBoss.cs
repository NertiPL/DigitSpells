using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    public float rotationSpeed;
    bool isPhase2=false;

    public List<GameObject> Attacks1stPhasePrefabs;

    public Animator animator;

    float hp;
    float hpMax;

    bool isAttacking;

    private void Start()
    {
        hpMax = gameObject.GetComponent<BossEnemy>().hp;
        SetOffAttacks();
    }

    public void SetOffAttacks()
    {
        if (!isPhase2)
        {
            InvokeRepeating("ChooseAttack", 0f, 3f);
        }
        else
        {
            ChooseAttack();
        }
    }

    private void Update()
    {
       
        hp = gameObject.GetComponent<BossEnemy>().hp;
        var targetDirection = GameManager.instance.player.transform.position - transform.position;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (hp <= hpMax / 2 && !isPhase2)
        {
            isAttacking = false;
            isPhase2 =true;
            CancelInvoke("ChooseAttack");
            animator.Play("ChangeState");
        }

        
    }

    public void ChooseAttack()
    {
        if (!isAttacking)
        {
            int randomAttack = Random.Range(0, 4);
            /* 0 - Magic Balls Around Him
            * 1 - Area Damage
            * 2 - Run Leaving Cobweb
            * 3 - Shoot Cobweb 
            */
            StartAttack(randomAttack);
        }
    }

    void StartAttack(int id)
    {
        Instantiate(Attacks1stPhasePrefabs[id],transform.position, Quaternion.identity,transform);
        if (!isPhase2)
        {
            isAttacking = true;
            switch (id)
            {
                case 0:
                    animator.Play("Dance");
                    break;

                case 1:
                    animator.Play("Attack1");
                    break;

                case 2:
                    animator.Play("Walk");
                    //to swap
                    break;

                default:
                    animator.Play("Attack2");
                    break;

            }
        }
        else
        {
            switch (id)
            {
                case 0:
                    animator.Play("DanceState");
                    //to swap
                    break;

                case 1:
                    animator.Play("Attack1State");
                    break;

                case 2:
                    animator.Play("WalkState");
                    //to swap
                    break;

                default:
                    animator.Play("Attack2State");
                    break;

            }
        }

    }
    public void EndAnim()
    {
        isAttacking=false;
    }
}
