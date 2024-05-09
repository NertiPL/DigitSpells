using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusic : MonoBehaviour
{
    public AudioClip musicClip;
    AudioSource musicSource;

    private void Start()
    {
        musicSource = GameManager.instance.music;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="Player")
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }
}
