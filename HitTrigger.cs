using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<Animator>().SetInteger("MoveType", 0);
        gameObject.GetComponent<HitTrigger>().enabled = false;
    }
}
