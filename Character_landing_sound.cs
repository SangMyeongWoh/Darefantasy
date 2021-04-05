using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_landing_sound : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();
    }
}
