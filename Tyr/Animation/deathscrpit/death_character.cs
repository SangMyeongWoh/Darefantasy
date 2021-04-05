using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death_character : death
{
    public GameObject Head;
    public GameObject LArm;
    public GameObject RArm;
    public GameObject Muffler;
    public GameObject HeadGear;

    public GameObject Origin_head;
    public GameObject Origin_LArm;
    public GameObject Origin_RArm;
    public GameObject Origin_Muffler;
    public GameObject Origin_HeadGear;

    protected override void Start()
    {
        base.Start();
        Head.SetActive(true);

        LArm.GetComponentInChildren<SpriteRenderer>().sprite = Origin_LArm.GetComponent<SpriteRenderer>().sprite;
        LArm.SetActive(true);

        RArm.GetComponentInChildren<SpriteRenderer>().sprite = Origin_RArm.GetComponent<SpriteRenderer>().sprite;
        RArm.SetActive(true);

        Muffler.GetComponentInChildren<SpriteRenderer>().sprite = Origin_Muffler.GetComponent<SpriteRenderer>().sprite;
        Muffler.SetActive(true);

        HeadGear.GetComponentInChildren<SpriteRenderer>().sprite = Origin_HeadGear.GetComponent<SpriteRenderer>().sprite;
        HeadGear.SetActive(true);
    }
    
}
