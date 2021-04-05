using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> ghost_skulls = new List<Sprite>();
    public List<Sprite> ghost_faces = new List<Sprite>();

    public GameObject ghost_skull;
    public GameObject ghost_face;

    void Start()
    {
        setGhost();
    }
    void setGhost()
    {
        ghost_skull.GetComponent<SpriteRenderer>().sprite = ghost_skulls[Random.Range(0, ghost_skulls.Count)];
        ghost_face.GetComponent<SpriteRenderer>().sprite = ghost_faces[Random.Range(0, ghost_skulls.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
