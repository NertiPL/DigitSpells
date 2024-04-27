using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberInEqScript : MonoBehaviour
{
    public float value;

    private void Start()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = value.ToString();
    }
}
