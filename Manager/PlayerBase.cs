using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : ObjectBase
{
	public MonsterType monsterType;
	List<Node> item_node = new List<Node>();

	public PlayerBase(MonsterType subtype, Node _node)
	{
		init_ObjectBase(ObjectBaseType.CHARACTER, (int)subtype, _node);
        movable = true;
		//objectStatus
	}
    public override bool is_target(ObjectBase targetbase)
    {
        if (targetbase is PlayerBase) return false;

        return targetbase.is_collide;
    }
    public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);

		is_collide = true;
		//objectStatus.heart = Constant.DEFAULT_PLAYER_HEART;
		monsterType = (MonsterType)subtype;
		turn_type = Turn_Type.Player_turn;

    }
	public override bool death()
	{
		Camera.main.GetComponent<CameraManager>().run_camera_anim(CameraAnimType.CHARACTER_DEATH);
		return base.death();
	}


	public override void move(Node next)
	{
		direction(next);

		if (node_now == next) gameManager.sub_managers.animManager.anim_add(this, AnimType.LANDING, 0, 0);
		else gameManager.sub_managers.animManager.anim_add(this, AnimType.MOVE, UDLR, 0);

		node_change(next);
	}

	public override void set_objectStatus()
	{
		for (int i = 0; i < Constant.DEFAULT_PLAYER_HEART; i++)
		{
			get_bag();
			get_default_heart();
		}
	}

	public void get_bag()
	{

		Vector3 position = new Vector3(Constant.ITEM_UI_X, Constant.ITEM_UI_Y + 1 * 10, 0.0f);
		gameManager.pools.objectBodyPool.get_body(ItemUIType.ITEM_BAG_UI, position, true);
		objectStatus.item_list.Add(ItemType.ITEM_BAG);
	}

	public void get_bag_heart()
	{

		Vector3 position = new Vector3(Constant.ITEM_UI_X + 1f, Constant.ITEM_UI_Y + 1 * 10, 0.0f);
		gameManager.pools.objectBodyPool.get_body(ItemUIType.ITEM_BAG_HEART_UI, position, true);
		objectStatus.heart_list.Add(ItemType.ITEM_BAG);
	}

	public void get_default_heart()
	{
		objectStatus.add_heart(1);

		Vector3 position = new Vector3(Constant.ITEM_UI_X + 1f, Constant.ITEM_UI_Y + 1 * 10, 0.0f);
		gameManager.pools.objectBodyPool.get_body(ItemUIType.ITEM_HEART_UI, position, true);
		objectStatus.heart_list.Add(ItemType.ITEM_HEART);
	}

	public void get_itemUI(ObjectBase _item)
	{
		//this.node_now.object_here_list.Remove(_item);
		if (((ItemBase)_item).itemType == ItemType.ITEM_HEART || ((ItemBase)_item).itemType == ItemType.ITEM_BLUEHEART)
		{
			Vector3 position = new Vector3(Constant.ITEM_UI_X + 1f, Constant.ITEM_UI_Y + 1 * 10, 0.0f);
			gameManager.pools.objectBodyPool.get_body((ItemUIType)(((ItemBase)_item).itemType), position, true);
			//_item.ObjectBody = GameManager.Instance.pools.objectBodyPool.get_body((ItemUIType)((ItemBase)_item).itemType, item_node[item_node.Count - 1].position);
			objectStatus.heart_list.Add(((ItemBase)_item).itemType);
			if (((ItemBase)_item).itemType == ItemType.ITEM_HEART)
			{
				objectStatus.heart++;
			}
			else if (((ItemBase)_item).itemType == ItemType.ITEM_BLUEHEART)
			{
				objectStatus.blueheart++;
			}
		}
		else
		{
			Vector3 position = new Vector3(Constant.ITEM_UI_X, Constant.ITEM_UI_Y + 1 * 10, 0.0f);
			gameManager.pools.objectBodyPool.get_body((ItemUIType)(((ItemBase)_item).itemType), position, true);
			//_item.ObjectBody = GameManager.Instance.pools.objectBodyPool.get_body((ItemUIType)((ItemBase)_item).itemType, item_node[item_node.Count - 1].position);
			objectStatus.item_list.Add(((ItemBase)_item).itemType);
		}
	}
	public bool get_item(ObjectBase _item) /// 여기부터 시작. 아이템 노드에서 삭제시 true 필요. 즉 get item 성공시 true
	{
		int out_item_idx;
		if (((ItemBase)_item).itemType == ItemType.ITEM_HEART || ((ItemBase)_item).itemType == ItemType.ITEM_BLUEHEART)
		{
			//objectStatus.add_heart(1);

			out_item_idx = check_bag_heart();
			if (out_item_idx == -1) return false; // 실패


			drop_heart(out_item_idx); // 현재자리에 아이템 생성.
			get_itemUI(_item); // 먹은 아이템 ui 생성
            
			return true;
		}

		/*
		if (((ItemBase)_item).itemType == ItemType.ITEM_BAG) // heart bag 추가필요
		{
			get_bag();
			return true;
		}
		*/

		if (((ItemBase)_item).itemType == ItemType.ITEM_BAG)
		{
			get_bag();
			return true;
		}

        if (((ItemBase)_item).itemType == ItemType.ITEM_BAG_HERAT)
        {
            get_bag_heart();
            return true;
        }

        out_item_idx = check_bag();
		if (out_item_idx == -1) return false; // 실패


		drop_item(out_item_idx); // 현재자리에 아이템 생성.
		get_itemUI(_item); // 먹은 아이템 ui 생성
        



        return true;
	}

	public int check_bag()
	{
		int out_item_idx = -1;
		for (int i = 0; i < objectStatus.item_list.Count; i++)
		{

			//if (objectStatus.item_list[i] == ItemType.ITEM_RED_SHIELD) continue; 드랍 안되는거 저주의방패 추후 업데이트 예정
			//if (objectStatus.item_list[i] == ItemType.ITEM_RED_SWORD) continue; 드랍 안되는거 저주의칼 추후 업데이트 예정
			if (out_item_idx == -1 && objectStatus.item_list[i] != ItemType.ITEM_BAG)
			{
				out_item_idx = i;
				continue;
			}
			if (objectStatus.item_list[i] == ItemType.ITEM_BAG)
			{
                out_item_idx = i;
				break;
			}
		}

		return out_item_idx;
	}

	public int check_bag_heart()
	{
		int out_item_idx = -1;
		for (int i = 0; i < objectStatus.heart_list.Count; i++)
		{

			//if (objectStatus.item_list[i] == ItemType.ITEM_RED_SHIELD) continue; 드랍 안되는거 저주의방패 추후 업데이트 예정
			//if (objectStatus.item_list[i] == ItemType.ITEM_RED_SWORD) continue; 드랍 안되는거 저주의칼 추후 업데이트 예정
			if (out_item_idx == -1 && objectStatus.heart_list[i] != ItemType.ITEM_BAG)
			{
				out_item_idx = i;
				continue;
			}

			if (objectStatus.heart_list[i] == ItemType.ITEM_BAG)
			{

				out_item_idx = i;
				break;
			}
		}

		return out_item_idx;
	}

	public override void drop_heart(int item_idx)
	{
		// 현위치에 item drop
		if (objectStatus.heart_list[item_idx] != ItemType.ITEM_BAG) // 가방이 아니면.
		{
			//item base 생성 및 아이템 랜딩 애님 재생
			ObjectBase _item = gameManager.pools.objectBasePool.call_object_base(ObjectBaseType.ITEM, (int)objectStatus.heart_list[item_idx], this.node_now);
			_item.ObjectBody = gameManager.pools.objectBodyPool.get_body(objectStatus.heart_list[item_idx], _item.node_now.position);
			gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(_item.ObjectBody, AnimType.LANDING, 0);

			//
			this.node_now.object_here_list.Add(_item);
		}

		if (objectStatus.heart_list[item_idx] == ItemType.ITEM_HEART)
		{
			objectStatus.heart--;
		}
		else if (objectStatus.heart_list[item_idx] == ItemType.ITEM_BLUEHEART)
		{
			objectStatus.blueheart--;
		}
		gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(gameManager.sub_managers.uIManager.heartUIList[item_idx], AnimType.DEATH, 0);
		gameManager.sub_managers.uIManager.heartUIList.RemoveAt(item_idx);
		objectStatus.heart_list.RemoveAt(item_idx);
	}

	public override void drop_item(int item_idx)
	{
        // 현위치에 item drop
		if (objectStatus.item_list[item_idx] != ItemType.ITEM_BAG) // 가방이 아니면.
		{
			//item base 생성 및 아이템 랜딩 애님 재생
			ObjectBase _item = gameManager.pools.objectBasePool.call_object_base(ObjectBaseType.ITEM, (int)objectStatus.item_list[item_idx], this.node_now);
			_item.ObjectBody = gameManager.pools.objectBodyPool.get_body(objectStatus.item_list[item_idx], _item.node_now.position);
			gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(_item.ObjectBody, AnimType.LANDING, 0);

			//
			this.node_now.object_here_list.Add(_item);
		}
        
        
		gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(gameManager.sub_managers.uIManager.itemUIList[item_idx], AnimType.DEATH, 0);
		gameManager.sub_managers.uIManager.itemUIList.RemoveAt(item_idx);
		objectStatus.item_list.RemoveAt(item_idx);

        
        //for (int i = 0; i < objectStatus.item_list.Count; i++) Debug.Log(objectStatus.item_list[i]);
	}

	public override void arrange_used_times()
	{
		for (int i = 0, len = objectStatus.item_list.Count; i < len; i++)
		{
			if (objectStatus.item_list[i] == ItemType.ITEM_NONE)
			{
				objectStatus.item_list.RemoveAt(i);
				gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(gameManager.sub_managers.uIManager.itemUIList[i], AnimType.DEATH, 0);
				gameManager.sub_managers.uIManager.itemUIList.RemoveAt(i);
                

                i--;
				len--;
				get_bag();
			}
		}
	}

	public override void arrange_del_hearts()
	{
		for (int i = 0, len = objectStatus.heart_list.Count; i < len; i++)
		{
			if (objectStatus.heart_list[i] == ItemType.ITEM_NONE)
			{
				objectStatus.heart_list.RemoveAt(i);
				gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(gameManager.sub_managers.uIManager.heartUIList[i], AnimType.DEATH, 0);
				gameManager.sub_managers.uIManager.heartUIList.RemoveAt(i);

				i--;
				len--;
				get_bag_heart();
			}
		}
	}


	public override bool hit(int[] damages)
	{
		if (damages[0] + damages[1] == 0) return false;

		int hurt = damages[0] - use_items_ntimes(ItemType.ITEM_SHIELD, damages[0]);
		int blue_hurt = damages[1];

		del_heart_ntimes(ItemType.ITEM_HEART, hurt);
		del_heart_ntimes(ItemType.ITEM_BLUEHEART, blue_hurt);        
        set_itemEffect();
        gameManager.sub_managers.animManager.anim_add(this, AnimType.ITEMEFFECT, UDLR, 0, this);
        if (objectStatus.del_heart_and_blueheart(new int[2] { hurt, blue_hurt })) // 죽음.
		{
			death();
			return true;
		}


		if (hurt == 0)
		{
			gameManager.sub_managers.animManager.anim_add(this, AnimType.HIT_WITHOUTBLOOD, UDLR, 1);
		}
		else
		{
			gameManager.sub_managers.animManager.anim_add(this, AnimType.HIT_WITHBLOOD, UDLR, 1);

		}
		return true;

	}
	public override int[] attack(ObjectBase targetBase)
	{
		//int target_life = targetBase.objectStatus.heart;
		int target_life = target_life_with_shield(targetBase);
		int damage = 0;
		int blue_damage = 0;

		direction(targetBase);

		if (targetBase is MonsterBase)
		{
			damage = use_items_ntimes(ItemType.ITEM_SWORD, target_life);
			blue_damage = use_items_ntimes(ItemType.ITEM_HOLYGRAIL, targetBase.objectStatus.blueheart);            
            set_itemEffect();

			if (damage != 0 || blue_damage != 0)
			{                
				gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 0);
				gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 0);
                gameManager.sub_managers.animManager.anim_add(this, AnimType.ITEMEFFECT, UDLR, 0, targetBase);
            }
		}
		else if (targetBase is BlockBase && targetBase.objectStatus.heart > 0)
		{
			if (((BlockBase)targetBase).blockType == BlockType.WELL)
			{
				int[] _damage = { 1, 0 };
				this.hit(_damage);
                //targetBase.hit(_damage);
                
				gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 0);
				gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 0);
                //gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.ITEMEFFECT, UDLR, 0);
            }
			else if (((BlockBase)targetBase).blockType == BlockType.VENDINGMACHINE)
			{

				damage = use_items_ntimes(ItemType.ITEM_NONE, 1);
				//gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 0);
				//gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 0);
			}
			else if (((BlockBase)targetBase).blockType == BlockType.CASE)
            {
                damage = 1;//use_items_ntimes(ItemType.ITEM_KEY, target_life); // CASE
            }
            else if (((BlockBase)targetBase).blockType == BlockType.CASE_LOCKED)
            {
                damage = use_items_ntimes(ItemType.ITEM_KEY, target_life); // CASE_Locked
                gameManager.sub_managers.animManager.anim_add(this, AnimType.ITEMEFFECT, UDLR, 0, this);
            }
            else if (((BlockBase)targetBase).blockType == BlockType.WELL_LOCKED)
            {
                if (((BlockBase_Well_Locked)targetBase).isLocked) {
                    damage = use_items_ntimes(ItemType.ITEM_KEY, target_life); // CASE_Locked
                    gameManager.sub_managers.animManager.anim_add(this, AnimType.ITEMEFFECT, UDLR, 0, this);
                } 
                else
                {
                    int[] _damage = { 1, 0 };
                    this.hit(_damage);
                    //targetBase.hit(_damage);

                    gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 0);
                    gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 0);
                }

            }

        }
		else if (targetBase is DoorBase)
		{
			damage = use_items_ntimes(ItemType.ITEM_KEY, target_life);
            gameManager.sub_managers.animManager.anim_add(this, AnimType.ITEMEFFECT, UDLR, 0, this);
        }

		// anim 

		if (damage + blue_damage == 0)
		{
			gameManager.sub_managers.animManager.anim_add(this, AnimType.CANNOTATTACK, UDLR, 0);
		}
		else
			gameManager.sub_managers.animManager.anim_add(this, AnimType.ATTACK, UDLR, 0);
		return new int[2] { damage, blue_damage };

	}

    public override void use_item(int item_idx)
    {  
        
        base.use_item(item_idx);
    }
    public override void set_itemEffect() 
    {        
        for (int i = 0; i < used_item_List.Count; i++)
        {
            if(!gameManager.pools.effectPool.findavailable(used_item_List[i]))
            {
                used_item_List.RemoveAt(i);
                i--;
            }
        }
    }



}
