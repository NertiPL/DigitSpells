using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLvlStation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0f;
            foreach(Transform child in GameManager.instance.canvas.transform)
            {
                child.gameObject.SetActive(false);
            }
            GameManager.instance.SpellLvlUpStation.SetActive(true);

            GameManager.instance.player.transform.position -= GameManager.instance.player.transform.forward;
            GameManager.instance.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
