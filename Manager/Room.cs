using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject tile_list;
    public GameObject mapbase;

    public void setMapBase(StageType stageType)
    {
        mapbase.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.pools.ImagePool.get_mapimage(stageType);
    }
    public void setTiles(RoomMeta roomMeta, StageType stageType)
    {
        if (!roomMeta.instantDone)
        {
            roomMeta.room_thema_list.Clear();
            List<Sprite> tile_sprites = GameManager.Instance.pools.ImagePool.get_tile_imageList(stageType);
            List<int> numlist = new List<int>();
            for(int i = 0; i < tile_sprites.Count; i++)
            {  
                numlist.Add(i);
            }


            SpriteRenderer[] spriteRenderers = tile_list.GetComponentsInChildren<SpriteRenderer>();
            
            for(int i = 0; i < spriteRenderers.Length; i++)
            {
                int subnum = Random.Range(0, numlist.Count);
                int num = numlist[subnum];
                numlist.RemoveAt(subnum);

                spriteRenderers[i].sprite = tile_sprites[num];
                roomMeta.room_thema_list.Add(tile_sprites[num]);
            }
        }
        else
        {
            SpriteRenderer[] spriteRenderers = tile_list.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].sprite = roomMeta.room_thema_list[i];
            }
        }
    }
}

public class Node
{
    public Node[] UDLR_node = new Node[4];
	public Vector3 position = new Vector3();
	public int[] xy = new int[2];   
    public List<ObjectBase> object_here_list = new List<ObjectBase>();
	public bool is_extra_node = false;
	public RoomMeta roommeta;

	public Node(int i, int j, RoomMeta roomMeta)
	{
		xy[0] = i;
		xy[1] = j;

		position = get_position(i, j);

		roommeta = roomMeta;
	}

	public Node(Vector3 pos) { // it is for door/item node.?????

		position = pos;
		is_extra_node = true;
	}

	Vector3 get_position(int i, int j) {
		/* i j   02
		   00 01 02 03 04
		   10 11 12 13 14                     0,g
	   2-1 20 21 22 23 24  -3g,0 -2g,0  -g,0  0,0  g,0  2g,0 
		         32
				 42
				 52
		 */
		Vector3 position = new Vector3();
		position.x = j * Constant.GRID_SIZE - 2 * Constant.GRID_SIZE;
		position.y = 2 * Constant.GRID_SIZE - i * Constant.GRID_SIZE;

		return position;
	}
}
