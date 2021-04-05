using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBodyPool : MonoBehaviour
{
    public List<BlockBox> BlockList = new List<BlockBox>();

    public GameObject spawn_Block_body(BlockType BlockType, Vector3 pos)
    {
        for (int i = 0; i < BlockList.Count; i++)
        {
            if (BlockType == BlockList[i].blockType)
            {
                
                GameObject body = Instantiate(BlockList[i].BlockObject, pos, transform.rotation) as GameObject;
                body.AddComponent<CoroutineBox>();
                GameManager.Instance.garbageBox.garbageList.Add(body);
                
                return body;
            }
        }
        Debug.Log("no such Block");
        return null;
    }
}
[System.Serializable]
public class BlockBox
{
    public BlockType blockType;
    public GameObject BlockObject;
}