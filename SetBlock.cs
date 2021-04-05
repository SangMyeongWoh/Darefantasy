using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBlock : MonoBehaviour
{
    public List<Sprite> Block_sprite_list = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        set_block_sprite();
    }

    void set_block_sprite()
    {
        int num = Random.Range(0, Block_sprite_list.Count);
        gameObject.GetComponent<SpriteRenderer>().sprite = Block_sprite_list[num];
    }
}
