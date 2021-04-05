using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ObjectBase
{
    public ItemType itemType;
	public ItemBase(ItemType subtype, Node _node)
	{
		
		init_ObjectBase(ObjectBaseType.ITEM, (int)subtype, _node);
		//objectStatus
	}

	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);
		is_collide = false;
		itemType = (ItemType)subtype;
	}
    public override bool death()
    {
        if (ObjectBody.GetComponent<ObjectBody>())
        {
            ObjectBody.GetComponent<ObjectBody>().DeathTrigger = true;
            ObjectBody.GetComponent<ObjectBody>().RoomNowForDeathEffect = gameManager.roomMeta_now;
        }
        else
        {
            Debug.Log("there is no objectbody script");
        }
        for (int i = 0; i < ItemUIList.Count; i++)
        {
            gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(ItemUIList[i], AnimType.DEATH, 0);
        }
        for (int i = 0; i < heartUIList.Count; i++)
        {
            if (heartUIList[i])
                gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(heartUIList[i], AnimType.DEATH, 0);
        }
        for (int i = 0; i < blueheartUIList.Count; i++)
        {
            if (blueheartUIList[i])
                gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(blueheartUIList[i], AnimType.DEATH, 0);
        }

        gameManager.roomMeta_now.room_stain_list.Add(ObjectBody);
        gameManager.sub_managers.animManager.anim_add(this, AnimType.DEATHWITHOUTDELAY, 0, 1);
        node_now.object_here_list.Remove(this);
        is_alive = false;

        return true;
    }
    public override bool interaction(ObjectBase objectBase)
	{
		if (objectBase is PlayerBase)
		{
			if(((PlayerBase)objectBase).get_item(this)) // getitem 성공시 true;
			{
				death();
                // + 아이템 사라지기
                return true;
			}

		}
		return false;
	}
	
}
