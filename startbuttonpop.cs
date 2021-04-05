using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startbuttonpop : MonoBehaviour
{
    public GameObject startbutton; 
    private void OnEnable()
    {
        startbutton.SetActive(true);
    }
}
