using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBodyPool : MonoBehaviour
{
    public GameObject DeathEffect;
    public List<MonsterBox> monsterList = new List<MonsterBox>();
    public GameObject spawn_monster_body(MonsterType monsterType, Vector3 pos)
    {
        for(int i = 0; i < monsterList.Count; i++)
        {
            if(monsterType == monsterList[i].monsterType)
            {
                GameObject body = Instantiate(monsterList[i].monsterObject, pos, transform.rotation) as GameObject;
                if(monsterType != MonsterType.CHARACTER)
                    GameManager.Instance.garbageBox.garbageList.Add(body);
                body.AddComponent<CoroutineBox>();
                return body;
            }
        }
        Debug.Log("no such monster");
        return null;
    }
}
[System.Serializable]
public class MonsterBox
{
    public MonsterType monsterType;
    public GameObject monsterObject;
}
