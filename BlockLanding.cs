using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLanding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            gameObject.GetComponent<Animator>().SetInteger("MoveType", 77);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetInteger("MoveType", 777);
        }

    }
}
