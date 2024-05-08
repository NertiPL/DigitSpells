using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusic : MonoBehaviour
{
    public AudioClip musicClip;
    public AudioSource musicSource;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="Player")
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }
}
