using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricField : MonoBehaviour
{
    public float duration;

    public float dmg;

    public Spells spell;

    private void Start()
    {
        duration = 2;
        transform.eulerAngles = new Vector3(-90, 0, 0);
        transform.parent= GameManager.instance.player.gameObject.transform;
        transform.position = Vector3.zero;

        if (spell.lvl >= 3)
        {
            duration = 4;
        }
        else if(spell.lvl >= 5)
        {
            duration = 6;
        }
        Invoke("DestroyField", duration);
    }

    void DestroyField()
    { 
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().DealDmg(spell.spellType, dmg);
        }
    }
}
