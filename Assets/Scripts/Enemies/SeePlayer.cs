using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SeePlayer : MonoBehaviour
{
    public bool isInRangeOfSeeing = false;

    private void Update()
    {
        transform.parent.GetComponent<Skeleton>().sees = isInRangeOfSeeing;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" )
        {
            isInRangeOfSeeing = true;
        }
    }
}
