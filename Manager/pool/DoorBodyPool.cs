using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBodyPool : MonoBehaviour
{
    public List<DoorBox> DoorList = new List<DoorBox>();

    public GameObject spawn_door_body(DoorType DoorType, Vector3 pos, int UDLR)
    {
		if (DoorType > DoorType.LOCKED_DOOR) DoorType = DoorType.LOCKED_DOOR;
        for (int i = 0; i < DoorList.Count; i++)
        {
            if (DoorType == DoorList[i].doorType)
            {
                Vector3 rotation = new Vector3(0, 0, 0);
                if (UDLR == 1)
                {
                    rotation = new Vector3(0, 0, 180);
                } else if(UDLR == 2)
                {
                    rotation = new Vector3(0, 0, 90);
                } else if(UDLR == 3)
                {
                    rotation = new Vector3(0, 0, 270);
                }
                GameObject body = Instantiate(DoorList[i].DoorObject, pos, transform.rotation) as GameObject;
                body.transform.localEulerAngles = rotation;                
                GameManager.Instance.garbageBox.garbageList.Add(body);
                body.AddComponent<CoroutineBox>();
                return body;
            }
        }
        Debug.Log("no such Door");
        return null;
    }
}

[System.Serializable]
public class DoorBox
{
    public DoorType doorType;
    public GameObject DoorObject;
}