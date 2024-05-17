using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float speed;
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed));
    }
}
