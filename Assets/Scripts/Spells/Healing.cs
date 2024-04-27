using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public Spells spell;
    public float healing;
    public Animator animator;

    void Start()
    {
        animator.Play("HealAnim");
    }

    public void OnEndAnim()
    {
        GameManager.instance.player.GetComponent<PlayerController>().hp += healing;
        Destroy(gameObject);
    }
}
