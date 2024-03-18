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
    Fish
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
        if (collision.gameObject.tag == "Player" && canCollide)
        {
            player.GetComponent<PlayerController>().GetHit(dmgOnCol);
            canCollide = false;
            Invoke("CanCollideAgain", 2f);
        }
    }

    void CanCollideAgain()
    {
        canCollide = true;
    }

}
