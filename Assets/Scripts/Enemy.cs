using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public class Enemy : MonoBehaviour
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

    Vector3 targetDirection;
    Vector3 newDirection;

    GameObject player;

    bool canCollide = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameManager.instance.player.gameObject;
    }
    private void Update()
    {
        CheckDeath();
        targetDirection = player.transform.position - transform.position;
        if (!(Vector3.Distance(transform.position, player.transform.position) <= 1f))
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

        newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed*Time.deltaTime, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && canCollide && !player.GetComponent<PlayerController>().isDashing)
        {
            player.GetComponent<PlayerController>().GetHit(dmgOnCol);
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

    void CanCollideAgain()
    {
        canCollide = true;
    }

    void CheckDeath()
    {
        if (hp<=0)
        {
            Destroy(gameObject);
        }
    }



}
