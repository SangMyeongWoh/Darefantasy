using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase_Vendingmachine : BlockBase
{

	//ItemType first_item = ItemType.ITEM_NONE;
	//ItemType second_item = ItemType.ITEM_NONE;
	ItemType result_item = ItemType.ITEM_NONE;

    

	public BlockBase_Vendingmachine(BlockType subtype, Node _node)
	{
		init_ObjectBase(ObjectBaseType.BLOCK, (int)subtype, _node);
		objectStatus.heart = 2;
	}

	public override bool death()
	{        
		ObjectBase tempbase;
		tempbase = gameManager.sub_managers.boardManager.create_object(ObjectBaseType.ITEM, (int)result_item, node_now);
		gameManager.sub_managers.animManager.anim_add(tempbase, AnimType.LANDING, 0, _priority: 0);
        
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
	public void mix_items()
	{
        for(int i = 0; i < objectStatus.item_list.Count; i++)
        {
            if(objectStatus.item_list[0] == objectStatus.item_list[1])
            {
                try
                {
                    result_item = (ItemType)System.Enum.Parse(typeof(ItemType), objectStatus.item_list[0].ToString() + "_RED");
                }
                catch
                {
                    //if(Random.Range(0,10) > 0) result_item = objectStatus.item_list[Random.Range(0, objectStatus.item_list.Count)];
                    result_item = objectStatus.item_list[Random.Range(0, objectStatus.item_list.Count)];

                }
                
            }
            else if(objectStatus.item_list.Contains(ItemType.ITEM_SWORD) && objectStatus.item_list.Contains(ItemType.ITEM_HOLYGRAIL))
            {
                result_item = ItemType.ITEM_HOLYSWORD;
            }
            else
            {
                result_item = objectStatus.item_list[Random.Range(0, objectStatus.item_list.Count)];
            }
        }

        /*
		if (first_item == ItemType.ITEM_SWORD && second_item == ItemType.ITEM_SWORD) result_item = ItemType.ITEM_SWORD_RED;
		else if (first_item == ItemType.ITEM_SHIELD && second_item == ItemType.ITEM_SHIELD) result_item = ItemType.ITEM_SHIELD_RED;
		else if (first_item == ItemType.ITEM_HOLYGRAIL && second_item == ItemType.ITEM_HOLYGRAIL_RED) result_item = ItemType.ITEM_HOLYGRAIL_RED;
		else result_item = (ItemType)((int)Random.Range(1, 4));*/        
	}

    public void create_UI()
    {
        for (int i = 0; i < objectStatus.item_list.Count; i++)
        {
            //gameManager.pools.objectBodyPool.get_body((ItemUIType)(((ItemBase)_item).itemType), position);
            ItemUIList.Add(gameManager.pools.objectBodyPool.get_body((ItemUIType)(objectStatus.item_list[i]), node_now.position + new Vector3(0.8f, 0.35f * i + 0.1f, 0), false));
            ItemUIList[i].transform.localScale *= 0.35f;
            ItemUIList[i].GetComponent<ItemUI>().BackGround.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 1);
        }
    }

    public override bool hit(int[] damage)
	{
		Debug.Log("VVENDINGMACHINE : "+objectStatus.heart.ToString()+"  "+((ItemType)damage[0]).ToString());
		ItemType get_Item = (ItemType)damage[0];
        if (get_Item == ItemType.ITEM_NONE || get_Item == ItemType.ITEM_HEART) {
            
            return false;
        } 

        
        if (objectStatus.item_list.Count < 2)
        {            
            objectStatus.item_list.Add(get_Item);
            if(objectStatus.item_list.Count == 1)
                create_UI();
            Debug.Log(objectStatus.item_list.Count);
            if (objectStatus.item_list.Count > 1)
            {
                
                mix_items();
            }
            
        }
        
        

        /*
		if (first_item == ItemType.ITEM_NONE)
		{
			first_item = get_Item;
		}

		else if (second_item == ItemType.ITEM_NONE)
		{
			second_item = get_Item;
			mix_items();
		}
		else
		{
			throw new System.InvalidOperationException("VendingMachine ERROR while getting items");
		}
        */

        if (objectStatus.del_heart(1))
		{
			death();
			return true;
		}
		return false;
	}
}
