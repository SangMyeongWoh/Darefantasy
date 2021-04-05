using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : ObjectBase
{
    public MonsterType monsterType;
    
	
	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);
		is_collide = true;
		monsterType = (MonsterType)subtype;
		has_turns = 1;
		turn_type = Turn_Type.Enemy_turn;
		create_UI();
	}
	public override bool hit(int[] damages) // 맞았으면 true 아님 false
	{
		if (damages[0] + damages[1] == 0) return false;

		int hurt = damages[0] - use_items_ntimes(ItemType.ITEM_SHIELD, damages[0]);
		int blue_hurt = damages[1];

		int temphurt = hurt;
		int heartindex = heartUIList.Count;
		for (int i = temphurt; i > 0; i--)
		{
			if (heartindex > 0)
			{
				gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(heartUIList[heartindex - 1], AnimType.DEATH, 0);
				heartUIList.RemoveAt(heartindex - 1);
				heartindex--;
			}
		}


		int tempbluehurt = blue_hurt;
		int blueheartindex = blueheartUIList.Count;
		for (int i = tempbluehurt; i > 0; i--)
		{
			if (blueheartindex > 0)
			{
				gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(blueheartUIList[blueheartindex - 1], AnimType.DEATH, 0);
				blueheartUIList.RemoveAt(blueheartindex - 1);
				blueheartindex--;
			}
		}

		if (objectStatus.del_heart_and_blueheart(new int[] { hurt, blue_hurt })) // 죽음.
		{
			death();
			return true;
		}

		//gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 1);

		gameManager.sub_managers.animManager.anim_add(this, AnimType.HIT, UDLR, 1);
		
		return true;
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
		for (int i = 0; i < objectStatus.heart; i++)
		{
			heartUIList.Add(gameManager.pools.objectBodyPool.get_body(ItemUIType.ITEM_HEART_UI, node_now.position + new Vector3(0.45f, 0.35f * i + 0.1f, 0), false));
			heartUIList[i].transform.localScale *= 0.35f;
		}
		for (int i = 0; i < objectStatus.blueheart; i++)
		{
			blueheartUIList.Add(gameManager.pools.objectBodyPool.get_body(ItemUIType.ITEM_BLUEHEART_UI, node_now.position + new Vector3(0.45f, 0.35f * (objectStatus.heart + i) + 0.1f, 0), false));
			blueheartUIList[i].transform.localScale *= 0.35f;
		}
	}
	
    
	public void arrange_UI() ////// 상명형 help
	{        
        gameManager.StartCoroutine(_arrange_UI());        
    }

    IEnumerator _arrange_UI()
    {
        
        for(int j = 0; j < 20; j++)
        {
            for (int i = 0; i < ItemUIList.Count; i++)
            {
                try
                {
                    ItemUIList[i].transform.position = Vector3.Lerp(ItemUIList[i].transform.position, node_now.position + new Vector3(0.8f, 0.35f * i + 0.1f, 0), 0.2f);
                }
                catch
                {                    
                    break;
                }
                
            }
            for (int i = 0; i < heartUIList.Count; i++)
            {
                try
                {
                    heartUIList[i].transform.position = Vector3.Lerp(heartUIList[i].transform.position, node_now.position + new Vector3(0.45f, 0.35f * i + 0.1f, 0), 0.2f);
                }
                catch
                {
                    break;
                }
                
            }
            for (int i = 0; i < blueheartUIList.Count; i++)
            {
                try
                {
                    blueheartUIList[i].transform.position = Vector3.Lerp(blueheartUIList[i].transform.position, node_now.position + new Vector3(0.45f, 0.35f * (heartUIList.Count + i) + 0.1f, 0), 0.2f);
                }
                catch
                {
                    break;
                }
                
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public override void set_objectStatus()
    {
    }

    

	public override bool death()
	{
		gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, 0, 1);
        if(Random.Range(0,30) == 0)
        {
            for(int i = 0; i <objectStatus.item_list.Count; i++)
            {
                gameManager.sub_managers.boardManager.create_object(ObjectBaseType.ITEM, (int)objectStatus.item_list[i], node_now);
            }
        }
        else
        {
            if(Random.Range(0,4) == 0)
                gameManager.sub_managers.boardManager.create_object(ObjectBaseType.ITEM, 0, node_now);
        }
        return base.death();        
	}
	public int get_priority()
	{
		return 1;
	}
    public override void use_item(int item_idx)
    {
        base.use_item(item_idx);
        if (!objectStatus.item_list[item_idx].ToString().Contains("RED"))
        {
            gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(ItemUIList[item_idx], AnimType.DEATH, 0);
            //call shield break effect.
            ItemUIList.RemoveAt(item_idx);
        }
            
        arrange_UI();		
    }

}
