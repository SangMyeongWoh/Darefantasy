using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activetitle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject title;
    private void OnEnable()
    {
        title.SetActive(true);
    }
}
