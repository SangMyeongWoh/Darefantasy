using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playsound_once : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<playsound_once>().enabled = false;
    }
}
