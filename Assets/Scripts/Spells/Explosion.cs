using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Spells spell;
    public GameObject explosion;

    public CinemachineVirtualCamera virtualCam;
    public CinemachineBasicMultiChannelPerlin perlinNoise;
    public float duration;
    public float intensity;

    GameObject expl;


    public float dmg;
    private void Start()
    {
        dmg = spell.LvlChanges(dmg);
        virtualCam = GameManager.instance.player.transform.parent.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Invoke("SetExplosion", 1);
        Invoke("CameraShake", 1);
        //CameraShake();
    }

    void SetExplosion()
    {
        expl=Instantiate(explosion, GameManager.instance.player.transform.GetChild(0).position + GameManager.instance.player.transform.GetChild(0).forward*2, Quaternion.identity, GameManager.instance.player.transform.GetChild(0));
    }

    void CameraShake()
    {
        perlinNoise.m_AmplitudeGain = intensity;
        Invoke("ResetIntesity", duration);
    }

    void ResetIntesity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
        Destroy(expl);
        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().DealDmg(spell.spellType, dmg);
            }
            else if (other.transform.parent.GetComponent<Enemy>() != null)
            {
                other.transform.parent.GetComponent<Enemy>().DealDmg(spell.spellType, dmg);
            }
        }
    }

}
