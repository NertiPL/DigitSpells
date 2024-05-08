using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Spells spell;
    public float speed;
    public float dmg;
    int pierceAmount = 0;

    Rigidbody rb;

    private void Start()
    {
        dmg = spell.LvlChanges(dmg);
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);

        if (spell.lvl >=3)
        {
            pierceAmount = 1;
        }
        if (spell.lvl >=5)
        {
            pierceAmount = 2;
        }
    }
    private void Update()
    {
        rb.velocity += transform.forward * 1 * speed * Time.fixedDeltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && pierceAmount<=0)
        {
            other.gameObject.GetComponentInParent<Enemy>().DealDmg(spell.spellType, spell.LvlChanges(dmg));
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Enemy" && pierceAmount > 0)
        {
            other.gameObject.GetComponentInParent<Enemy>().DealDmg(spell.spellType, spell.LvlChanges(dmg));
            pierceAmount--;
        }
    }

}
