using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Earthquake : MonoBehaviour
{
    public float intensity;
    public float duration;

    public float dmg;

    public Spells spell;

    public CinemachineVirtualCamera virtualCam;
    public CinemachineBasicMultiChannelPerlin perlinNoise;

    private void Start()
    {
        transform.eulerAngles = new Vector3(-90,0,0);
        virtualCam = GameManager.instance.player.transform.parent.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        ShakeCamera();
    }

    public void ShakeCamera()
    {
        perlinNoise.m_AmplitudeGain = intensity;
        Invoke("ResetIntesity", duration);
    }

    

    void ResetIntesity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemy>().DealDmg(spell.spellType,dmg);
        }
    }
}
