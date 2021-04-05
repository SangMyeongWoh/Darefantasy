using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUIDeath : MonoBehaviour
{
    public GameObject HeartUIITSelf;
    public GameObject Heart;
    public GameObject HeartEffect;
    public GameObject HeartEffect2;
    
    private void FixedUpdate()
    {
    }

    public void heartUI_death()
    {
        StartCoroutine(heartUI_death_coroutine());
    }

    IEnumerator heartUI_death_coroutine()
    {
        gameObject.GetComponentInParent<Animator>().enabled = false;
        HeartEffect.GetComponent<ParticleSystem>().Play();
        for (int i = 0; i < 120; i++)
        {
            HeartUIITSelf.transform.localScale = Vector3.Lerp(HeartUIITSelf.transform.localScale, new Vector3(2f, 2f, 1), 0.1f);
            
            if(i < 65)
            {
                if (i % 2 == 0)
                {
                    Heart.transform.localScale += new Vector3(0.03f, 0, 0);
                }
                else
                {
                    Heart.transform.localScale += new Vector3(-0.03f, 0, 0);
                }
                Heart.transform.localScale = Vector3.Lerp(Heart.transform.localScale, new Vector3(0.65f, 0.1f, 1), 0.01f);
                if (i == 64) HeartEffect.GetComponent<ParticleSystem>().Stop();

            } else if (i >= 65 && i < 70)
            {
                Heart.transform.localScale = Vector3.Lerp(Heart.transform.localScale, new Vector3(0.1f, 0.65f, 1), 0.8f);
            }
            else
            {
                if (i == 70) HeartEffect2.GetComponent<ParticleSystem>().Play();

                Heart.transform.localScale = Vector3.Lerp(Heart.transform.localScale, new Vector3(0.65f, 0f, 1), 0.8f);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
