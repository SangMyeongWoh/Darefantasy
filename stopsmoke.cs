using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopsmoke : MonoBehaviour
{
    public GameObject smoke1;
    public GameObject smoke2;
    public GameObject explosionEffect;
    bool exeplositonDone;

    private void OnEnable()
    {
        smoke1.GetComponent<ParticleSystem>().Stop();
        smoke2.GetComponent<ParticleSystem>().Stop();
        if (!exeplositonDone)
        {
            explosionEffect.GetComponent<ParticleSystem>().Play();
            explosionEffect.GetComponent<AudioSource>().Play();
            exeplositonDone = true;
        }
        
    }
}
