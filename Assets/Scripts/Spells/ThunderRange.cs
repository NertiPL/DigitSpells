using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRange : MonoBehaviour
{
    public GameObject thunderPrefab;

    bool shot = false;

    public Spells spell;

    public int ammount;

    List<GameObject> alreadyHit;

    private void Start()
    {
        alreadyHit = new List<GameObject>();
        transform.position = GameManager.instance.player.transform.position;

        ammount = 1;
        if (spell.lvl >= 3)
        {
            ammount = 2;
        }

        if (spell.lvl >= 5)
        {
            ammount = 3;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !shot && !alreadyHit.Contains(other.gameObject))
        {
            alreadyHit.Add(other.gameObject);
            ammount--;
            if (ammount <= 0)
            {
                shot = true;
            }
            Debug.Log("aaa");
            Instantiate(thunderPrefab, other.transform.position + new Vector3(0f,8f,0f),Quaternion.identity,transform);
            Invoke("DestroySelf", 5f);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
