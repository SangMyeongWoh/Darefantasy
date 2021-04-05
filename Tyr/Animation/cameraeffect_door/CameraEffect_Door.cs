using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect_Door : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        gameObject.GetComponent<Animator>().SetBool("stageUp", false);
        gameObject.GetComponent<CameraEffect_Door>().enabled = false;
    }
    
}
