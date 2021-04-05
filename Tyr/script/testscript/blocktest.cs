using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocktest : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.GetComponent<Animator>().SetInteger("MoveType", 7);

        }
    }
}
