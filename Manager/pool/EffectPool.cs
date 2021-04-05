using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{ 

    public List<EffectBox> effectBoxes = new List<EffectBox>();
    Dictionary<ItemType, List<GameObject>> _effectpool = new Dictionary<ItemType, List<GameObject>>();

    private void Start()
    {
        setPool();
    }

    public void callEffect(ItemType itemType, Node targetnode, int LR)
    {
        List<GameObject> effectlist = _effectpool[itemType];
        if (effectlist == null) return;
        for(int i = 0; i < effectlist.Count; i++)
        {
            if (!effectlist[i].activeSelf)
            {
                effectlist[i].SetActive(true);
                effectlist[i].transform.position = targetnode.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f),0);
                if (LR != 0) effectlist[i].transform.localScale = new Vector3(-1, 1, 1);
                else effectlist[i].transform.localScale = new Vector3(1, 1, 1);
                return;
            }
        }
        GameObject temp;
        effectlist.Add(temp = Instantiate(effectlist[0], targetnode.position, transform.rotation) as GameObject);
        if (LR != 0) temp.transform.localScale = new Vector3(-1, 1, 1);
        else temp.transform.localScale = new Vector3(1, 1, 1);

    }

    void setPool()
    {
        for(int i = 0; i < effectBoxes.Count; i++)
        {            
            _effectpool.Add(effectBoxes[i].itemType, new List<GameObject>());
            _effectpool[effectBoxes[i].itemType].Add(Instantiate(effectBoxes[i].Effect, new Vector3(0,300,0), transform.rotation) as GameObject);
            _effectpool[effectBoxes[i].itemType][0].SetActive(false);
        }
    }

    public bool findavailable(ItemType itemType)
    {
        if (_effectpool.ContainsKey(itemType)) return true;
        else return false;
    }
    
}
[System.Serializable]
public class EffectBox
{
    public ItemType itemType;
    public GameObject Effect;
}
