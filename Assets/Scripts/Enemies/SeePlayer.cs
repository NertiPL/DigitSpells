using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SeePlayer : MonoBehaviour
{
    public bool isInRange = false;

    private void Update()
    {
        transform.parent.GetComponent<Skeleton>().sees = isInRange;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" )
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }
}
