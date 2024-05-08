using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public Spells spell;
    public float healing;
    public Animator animator;


    private void Awake()
    {
        transform.position = GameManager.instance.player.transform.position;
        transform.eulerAngles = new Vector3(90f, 0f, 0f);
    }
    void Start()
    {
        healing = spell.LvlChanges(healing);
        animator.Play("HealAnim");
    }

    public void OnEndAnim()
    {
        GameManager.instance.player.GetComponent<PlayerController>().hp += healing;
        Destroy(gameObject);
    }
}
