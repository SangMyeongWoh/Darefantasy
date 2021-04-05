using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBodyPool : MonoBehaviour
{
    public List<TrapBox> TrapList = new List<TrapBox>();

    public GameObject spawn_Trap_body(TrapType TrapType, Vector3 pos)
    {
        for (int i = 0; i < TrapList.Count; i++)
        {
            if (TrapType == TrapList[i].trapType)
            {
                GameObject body = Instantiate(TrapList[i].TrapObject, pos, transform.rotation) as GameObject;
                GameManager.Instance.garbageBox.garbageList.Add(body);
                body.AddComponent<CoroutineBox>();
                
                return body;
            }
        }
        Debug.Log("no such Trap");
        return null;
    }
}

[System.Serializable]
public class TrapBox
{
    public TrapType trapType;
    public GameObject TrapObject;
}
