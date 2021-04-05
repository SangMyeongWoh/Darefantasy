using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase_Case : BlockBase
{
	public BlockBase_Case(BlockType subtype, Node _node)
	{
		init_ObjectBase(ObjectBaseType.BLOCK, (int)subtype, _node);
	}

	public override bool death()
	{
		ObjectBase tempbase;
		int rand_item_idx = Random.Range(0, 6);
		if (rand_item_idx == 0)
        {
            tempbase = gameManager.sub_managers.boardManager.create_object(ObjectBaseType.ITEM, (int)ItemType.ITEM_KEY, node_now);
            gameManager.sub_managers.animManager.anim_add(tempbase, AnimType.LANDING, 0, _priority: 0);
        }
        else if (rand_item_idx == 1)
        {
            tempbase = gameManager.sub_managers.boardManager.create_object(ObjectBaseType.ITEM, (int)ItemType.ITEM_HEART, node_now);
            gameManager.sub_managers.animManager.anim_add(tempbase, AnimType.LANDING, 0, _priority: 0);
        }
        else if(rand_item_idx == 2)
        {
            tempbase = gameManager.sub_managers.boardManager.create_object(ObjectBaseType.ITEM, (int)ItemType.ITEM_SWORD, node_now);
            gameManager.sub_managers.animManager.anim_add(tempbase, AnimType.LANDING, 0, _priority: 0);
        }
        else if (rand_item_idx == 3)
        {
            tempbase = gameManager.sub_managers.boardManager.create_object(ObjectBaseType.ITEM, (int)ItemType.ITEM_SHIELD, node_now);
            gameManager.sub_managers.animManager.anim_add(tempbase, AnimType.LANDING, 0, _priority: 0);
        }



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




}
