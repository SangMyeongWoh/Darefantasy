using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public List<Sprite> meatloaf_image_list = new List<Sprite>();
    public List<GameObject> meatloaf_list = new List<GameObject>();
    public List<Sprite> deathspritelist = new List<Sprite>();
    public List<AudioClip> audioClips = new List<AudioClip>();

    public GameObject spriteObject;

    int countsaver = 0;
    bool animstart;

    private void Start()
    {
        GetComponent<AudioSource>().clip = audioClips[Random.Range(0, audioClips.Count)];
        GetComponent<AudioSource>().Play();
        StartCoroutine(death());
    }

    void set_meatloaf()
    {
        for(int i = 0; i < meatloaf_list.Count; i++)
        {
            if (Random.Range(0, 3) > 0)
            {
                int imagenum = Random.Range(0, meatloaf_image_list.Count);
                meatloaf_list[i].GetComponentInChildren<SpriteRenderer>().sprite = meatloaf_image_list[imagenum];
                meatloaf_image_list.RemoveAt(imagenum);
                meatloaf_list[i].SetActive(true);
            }
                
        }
    }

    IEnumerator death()
    {
        animstart = true;
        if(countsaver == 0)
        {
            set_meatloaf();
        }
        for (int i = countsaver; i < deathspritelist.Count; i++)
        {
            spriteObject.GetComponent<SpriteRenderer>().sprite = deathspritelist[i];
            countsaver = i;
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnEnable()
    {
        if (animstart) StartCoroutine(death());
    }
}
