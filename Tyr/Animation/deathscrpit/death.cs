using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
    // Start is called before the first frame update
    
    protected virtual void Start()
    {
        spawndeath();
    }

    public void spawndeath()
    {
        GameObject death = Instantiate(GameManager.Instance.pools.objectBodyPool.bodyPools.MonsterBodyPool.DeathEffect, transform.position, transform.rotation) as GameObject;
        GameManager.Instance.garbageBox.garbageList.Add(death);
        GameManager.Instance.roomMeta_now.room_stain_list.Add(death);
        if(GetComponentInParent<ObjectBody>())
        {
            GetComponentInParent<ObjectBody>().DeathEffectSpawnDone = true;
        }       
    }
    
    
}
