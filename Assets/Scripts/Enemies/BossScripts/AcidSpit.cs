using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AcidSpit : MonoBehaviour
{
    GameObject warningArea;
    GameObject acidArea;

    public float dmg;

    bool warningAreaVisable = true;
    bool canDmg = true;

    private void Start()
    {
        transform.parent = transform.parent.parent;
        transform.position = GameManager.instance.player.transform.position;
        transform.rotation = GameManager.instance.player.transform.rotation;
        warningArea = transform.GetChild(0).gameObject;
        acidArea = transform.GetChild(1).gameObject;

        Invoke("ChangeToAcid", 2f);
        InvokeRepeating("BlinkingWarning", 0, 0.2f);

    }

    void BlinkingWarning()
    {
        warningAreaVisable = !warningAreaVisable;
        warningArea.SetActive(warningAreaVisable);
    }

    void ChangeToAcid()
    {
        CancelInvoke("BlinkingWarning");
        acidArea.SetActive(true);
        warningArea.SetActive(false);
        Invoke("SelfDestroy", 3f);
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && canDmg)
        {
            other.gameObject.GetComponentInParent<PlayerController>().GetHit(dmg);
            canDmg = false;
            Invoke("CanDmgAgain", 0.2f);
        }
    }

    void CanDmgAgain()
    {
        canDmg = true;
    }
}
