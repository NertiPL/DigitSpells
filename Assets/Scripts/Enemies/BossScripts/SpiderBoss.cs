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

    private void Start()
    {
        hpMax = gameObject.GetComponent<BossEnemy>().hp;
        SetOffAttacks();
    }

    void SetOffAttacks()
    {
        if (!isPhase2)
        {
            InvokeRepeating("ChooseAttack", 0f, 3f);
        }
        else
        {
            InvokeRepeating("ChooseAttack", 0f, 1.5f);
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
            isPhase2=true;
            CancelInvoke("ChooseAttack");
            SetOffAttacks();
        }
    }

    public void ChooseAttack()
    {
        int randomAttack =Random.Range(0, 4);
        /* 0 - Magic Balls Around Him
        * 1 - Area Damage
        * 2 - Run Leaving Cobweb
        * 3 - Shoot Cobweb 
        */ 
        StartAttack(randomAttack);
    }

    void StartAttack(int id)
    {
        Instantiate(Attacks1stPhasePrefabs[id],transform.position, Quaternion.identity,transform); 
    }
}
