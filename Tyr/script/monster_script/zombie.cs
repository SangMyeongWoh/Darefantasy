using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie : MonoBehaviour
{
    public List<Sprite> LArms = new List<Sprite>();
    public List<Sprite> RArms = new List<Sprite>();
    public List<Sprite> faces = new List<Sprite>();
    public List<Sprite> hairs = new List<Sprite>();
    public List<AudioClip> attack_sount_list = new List<AudioClip>();
    // Start is called before the first frame update
    public GameObject LArm;
    public GameObject RArm;
    public GameObject Face;
    public GameObject Hair;

    void Start()
    {
        setZombie();
    }
    void setZombie()
    {
        LArm.GetComponent<SpriteRenderer>().sprite = LArms[Random.Range(0, LArms.Count)];
        RArm.GetComponent<SpriteRenderer>().sprite = RArms[Random.Range(0, RArms.Count)];
        try
        {
            Face.GetComponent<SpriteRenderer>().sprite = faces[Random.Range(0, faces.Count)];
            Hair.GetComponent<SpriteRenderer>().sprite = hairs[Random.Range(0, hairs.Count)];
        }
        catch
        {

        }
        
        
    }
    public void setAttackSound()
    {
        GetComponent<AudioSource>().clip = attack_sount_list[Random.Range(0, attack_sount_list.Count)];
    }

}
