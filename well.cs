using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class well : MonoBehaviour
{
    public GameObject BloodStain;
    public GameObject WellEffect;
    public GameObject WellEffect2;
    public GameObject Lock;
    bool BloodEffectDone;

    private void FixedUpdate()
    {
    }
    private void OnEnable()
    {
        if(BloodEffectDone) WellEffect.GetComponent<ParticleSystem>().Play();
    }

    public void BloodProcess()
    {
        if (!BloodEffectDone)
        {
            StartCoroutine(blood());
            BloodEffectDone = true;
        }
        
    }
    public void unlock()
    {
        if (Lock)
        {
            Lock.GetComponent<AudioSource>().Play();
            StartCoroutine(unlockprocess());
        }
    }
    
    IEnumerator unlockprocess()
    {
        for (int i = 0; i < 90; i++)
        {
            if (i < 30) Lock.transform.localScale = Vector3.Lerp(Lock.transform.localScale, new Vector3(0.3f, 1f, 1), 0.1f);
            else Lock.transform.localScale = Vector3.Lerp(Lock.transform.localScale, new Vector3(1f, 0.3f, 1), 0.1f);
            Lock.GetComponent<SpriteRenderer>().color = Color.Lerp(Lock.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0), 0.2f);
            yield return new WaitForSeconds(0.02f);
        }
    }
    IEnumerator blood()
    {
        GetComponent<AudioSource>().Play();
        WellEffect.GetComponent<ParticleSystem>().Play();
        WellEffect2.GetComponent<ParticleSystem>().Play();
        for (int i = 0; i < 90; i++)
        {
            BloodStain.GetComponent<SpriteRenderer>().color = Color.Lerp(BloodStain.GetComponent<SpriteRenderer>().color, new Color(1, 0, 0, 1), 0.03f);
            yield return new WaitForSeconds(0.02f);
        }
    }

}
