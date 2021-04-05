using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartProcess : MonoBehaviour
{
    public GameObject title1;
    public GameObject title2;
    public GameObject titlewithmenu;
    public GameObject sounds;
    // Start is called before the first frame update
    bool startanim;
    SpriteRenderer[] spriteRenderers;
    ParticleSystem[] particleSystems;

    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.anyKeyDown)
        {
            if (!startanim)
            {
                title1.GetComponent<Animator>().SetBool("start", true);
                startanim = true;
            }
            else
            {
                effect1.SetActive(false);
                effect2.SetActive(false);
                effect3.SetActive(false);
                try
                {
                    Camera.main.transform.GetComponent<camerastart>().stopCoroutine();
                }
                catch
                {
                    
                }
                title1.GetComponent<Animator>().enabled = false;
                title2.GetComponent<Animator>().enabled = false;
                spriteRenderers = title1.GetComponentsInChildren<SpriteRenderer>();
                particleSystems = title1.GetComponentsInChildren<ParticleSystem>();
                for(int i = 0; i < spriteRenderers.Length; i++)
                {
                    spriteRenderers[i].sprite = null;
                }
                spriteRenderers = title2.GetComponentsInChildren<SpriteRenderer>();
                particleSystems = title2.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    spriteRenderers[i].sprite = null;
                }
                sounds.SetActive(false);
                titlewithmenu.SetActive(true);
            }
            

        }

    }
}
