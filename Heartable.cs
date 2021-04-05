using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartable : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
