using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum Class
{
    Mage,
    Warrior,
    Spider,
    Goblin,
    Rabbit,
    Witcher,
    Sorcerer,
    Healer,
    Dragon,
    Fish,
    Slime,
    Wolf,
    Skeleton,
    TrainingDummy,
    Zombie
}

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 2;
    public string name;
    public string description;
    public int lvl;
    public float hp;
    public Class enemyClass;
    public float dmgOnCol;
    public List<SpellType> weakSpellType;
    public List<SpellType> strongSpellType;

    public Vector3 targetDirection;
    public Vector3 newDirection;

    public GameObject player;

    public bool canCollide = true;

    public Animator animator;

    public float dmg;

    public bool canMove=true;
    public bool sees = false;
    public bool isInAttackRange;

    public bool useMeleeAnim=false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameManager.instance.player.gameObject;
    }
    private void Update()
    {
        if (useMeleeAnim)
        {
            CollideAnim();
        }
        WalkAnim();
        CheckDeath();
        targetDirection = player.transform.position - transform.position;
        if (!(Vector3.Distance(transform.position, player.transform.position) <= 1f) && canMove && sees)
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

        if (sees)
        {
            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && canCollide && !player.GetComponent<PlayerController>().isDashing)
        {
            useMeleeAnim = true;
            canCollide = false;
            Invoke("CanCollideAgain", 2f);
        }
    }

    public void DealDmg(SpellType spellType, float dmg)
    {
        if(weakSpellType.Contains(spellType) && weakSpellType.Count>0)
        {
            hp -= dmg/2;
        }
        else if(strongSpellType.Contains(spellType) && strongSpellType.Count > 0)
        {
            hp -= dmg * 2;
        }
        else
        {
            hp -= dmg;
        }
    }

    public void CanCollideAgain()
    {
        canCollide = true;
    }

    public void CheckDeath()
    {
        if (hp<=0)
        {
            Destroy(gameObject);
        }
    }

    public abstract void WalkAnim();
    public abstract void CollideAnim();



}
