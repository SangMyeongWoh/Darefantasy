using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIDeath : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }    
}
