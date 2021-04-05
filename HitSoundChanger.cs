using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundChanger : MonoBehaviour
{
    public List<AudioClip> hit_sound_list = new List<AudioClip>();

    private void OnEnable()
    {
        GetComponent<AudioSource>().clip = hit_sound_list[Random.Range(0, hit_sound_list.Count)];
        GetComponent<HitSoundChanger>().enabled = false;
    }
}
