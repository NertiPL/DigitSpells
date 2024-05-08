using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike : MonoBehaviour
{
    public Spells spell;
    public Animator animator;
    public float dmg;

    bool dealtDmg=false;

    private void Start()
    {
        dmg = spell.LvlChanges(dmg);
    }

    public void EndAnim()
    {
        animator.Play("ThunderDown");
        Invoke("DestroySelf", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !dealtDmg)
        {
            dealtDmg = true;
            if (other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().DealDmg(spell.spellType, dmg);
            }
            else if (other.transform.parent.GetComponent<Enemy>()!=null)
            {
                other.transform.parent.GetComponent<Enemy>().DealDmg(spell.spellType, dmg);
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
