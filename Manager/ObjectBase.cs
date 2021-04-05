using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectBase
{
    //public int location; // node location..
    public Node node_now; // 이걸로 location 대체 
                          //public Vector3 pos_now;  // 이것도 필요없음.
    public bool is_alive;
    //public ObjectBase this;
    public ObjectStatus objectStatus;
    public RoomMeta roomMeta_now; // 이거 setting 안되고있음.
    public GameObject ObjectBody;
	public GameManager gameManager;
    public ObjectBaseType objectBase_type;
    public bool is_collide;
	public int UDLR;
	public int has_turns;
	public bool can_splash;
	public Turn_Type turn_type;
	public int delay_turn;
    public ObjectBase targeted_base;
    public List<GameObject> ItemUIList = new List<GameObject>();
	public List<GameObject> heartUIList = new List<GameObject>();
	public List<GameObject> blueheartUIList = new List<GameObject>();
    public bool movable;
    public List<ItemType> used_item_List = new List<ItemType>();
    //ObjectBase생성자에 대한 문법 추가 필요.
    //생성자의 상속이 어떻게되는지 기억이 안나서 생각이 필요함
    //ObjectBase를 상속받아서 플레이어나 기타 몬스터를 생성함.
    //ObjectBase는 monobehavior없으므로 게임오브젝트를 직접 생성하면 안됨.
    //이것도 new키워드로 붙여야함.
    //블럭처럼 instance가 잇고 거기에 필요한 애니메이션 몸통은 별도로 잇는것.
    //이렇게해야 별도의 몬스터 리스트를 업데이트 해주고 애니메이션은 그걸 따라가는것.


    public virtual void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
    {
		gameManager = GameManager.Instance;
        objectBase_type = _objectBaseType;
        objectStatus = new ObjectStatus(_objectBaseType);
        node_now = _node;
        //this = this;
        is_alive = true;
		has_turns = 1;
		can_splash = false;
		turn_type = Turn_Type.None;
		delay_turn = 1;
		targeted_base = null;
        set_objectStatus();
	}

    public void unactive()
    {
        ObjectBody.SetActive(false);
    }
    public virtual void active()
    {
        ObjectBody.transform.position = node_now.position;
        ObjectBody.SetActive(true);
    }
	public virtual void set_UDLR(int _UDLR)
	{
		UDLR = _UDLR;
	}
	public virtual void move(Node next)
	{
		direction(next);

		if (node_now == next) gameManager.sub_managers.animManager.anim_add(this, AnimType.LANDING, 0, 0);
		else gameManager.sub_managers.animManager.anim_add(this, AnimType.MOVE, UDLR, 0);

		node_change(next);
	}

	public void node_change(Node next)
    {
		node_now.object_here_list.Remove(this);
        node_now = next;
        node_now.object_here_list.Add(this);
    }
    public virtual void set_objectStatus()
    {
		objectStatus.heart++;
	}
    public virtual bool is_target(ObjectBase targetbase)
    {
        return false;
    }
    public virtual Node next_node()
	{
		return null;
	}

    public virtual bool interaction(ObjectBase objectBase) // 겹쳐있을때 발생. 수동object에서 발동
    {
        //objectbase의 타입이 몬스터일경우 attack,
        //objectbase의 타입이 아이템일경우 이동및 획득
        //objectbase의 타입이 obstacle일경우 아무일도 안일어남
        //objectbase의 타입이 함정일 경우 떨어짐
        return false;
    }

	public virtual bool hit(int damage) // 맞았으면 true 아님 false
	{
		if (damage == 0) return false;

		int hurt = damage - use_items_ntimes(ItemType.ITEM_SHIELD, damage);
		if (objectStatus.del_heart(hurt)) // 죽음.
		{
			death();
			return true;
		}

		gameManager.sub_managers.animManager.anim_add(this, AnimType.HIT, UDLR, 1);

		return true;
	}

	public virtual bool hit(int[] damages) // 맞았으면 true 아님 false
    {
		if (damages[0] + damages[1] == 0) return false;

		int hurt = damages[0] - use_items_ntimes(ItemType.ITEM_SHIELD, damages[0]);
		int blue_hurt = damages[1];

		if (objectStatus.del_heart_and_blueheart(new int[2] {hurt ,blue_hurt})) // 죽음.
        {
            death();
            return true;
        }        
        
        gameManager.sub_managers.animManager.anim_add(this, AnimType.HIT, UDLR, 1);

        return true;
    }

    public virtual int[] attack(ObjectBase targetBase)
    {
        //int target_life = targetBase.objectStatus.heart;
        
        int target_life = target_life_with_shield(targetBase);
        int damage = 0;
		int blue_damage = 0;
        if (targetBase is MonsterBase || targetBase is PlayerBase)
        {
            damage = use_items_ntimes(ItemType.ITEM_SWORD, target_life);
			blue_damage = use_items_ntimes(ItemType.ITEM_HOLYGRAIL, targetBase.objectStatus.blueheart);
            
        }
        else if (targetBase is DoorBase)
        {
            damage = use_items_ntimes(ItemType.ITEM_KEY, target_life);
        }
        
        // anim 
        gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 0);
        gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 0);
        gameManager.sub_managers.animManager.anim_add(this, AnimType.ATTACK, UDLR, 0);
		return new int[2] { damage, blue_damage };
    }

    public int target_life_with_shield(ObjectBase targetBase)
    {
        int target_life = targetBase.objectStatus.heart;

        for (int i = 0; i < targetBase.objectStatus.item_list.Count; i++)
        {
            if (targetBase.objectStatus.item_list[i] == ItemType.ITEM_SHIELD ||
                targetBase.objectStatus.item_list[i] == ItemType.ITEM_SHIELD_RED)
                target_life++;
        }

        return target_life;
    }

    public virtual int use_items_ntimes(ItemType itemtype, int max_times)
    {
        //만약 itemtype이 shield이면 이펙트 발생시키면됨.
        int used_times = 0;
        for (int i = 0; i < objectStatus.item_list.Count; i++)
        {
            if (used_times >= max_times) break;

            
            if (objectStatus.item_list[i].ToString().Contains(itemtype.ToString()))
            {
                used_times++;
                use_item(i);
            }

            if(itemtype == ItemType.ITEM_NONE)//for vendingmachine
            {
                if (objectStatus.item_list[i] != ItemType.ITEM_BAG && objectStatus.item_list[i] != ItemType.ITEM_NONE)
                {
                    used_times = (int)objectStatus.item_list[i];
                    use_item_even_RED(i);
                }
            }
        }

        arrange_used_times();
        return used_times; // 사용된 횟수 or 사용한 아이템 index(vending machine)
    }

    public virtual void arrange_used_times()
    {
        for (int i = 0, len = objectStatus.item_list.Count; i < len; i++)
        {
            if (objectStatus.item_list[i] == ItemType.ITEM_NONE)
            {
                objectStatus.item_list.RemoveAt(i);
                i--;
                len--;
            }
        }
	}

	public virtual int del_heart_ntimes(ItemType itemtype, int max_times)
	{
		int deleted_times = 0;
		for (int i = 0; i < objectStatus.heart_list.Count; i++)
		{
			if (deleted_times == max_times) break;
			switch (itemtype)
			{
				case ItemType.ITEM_HEART:
					if (objectStatus.heart_list[i] == ItemType.ITEM_HEART)
					{
						deleted_times++;
						del_heart(i);
					}
					break;

				case ItemType.ITEM_BLUEHEART:
					if (objectStatus.heart_list[i] == ItemType.ITEM_BLUEHEART)
					{
						deleted_times++;
						del_heart(i);
					}
					break;
					
				default:
					Debug.Log("what do you use item?");
					break;
			}
		}

		arrange_del_hearts();
		return deleted_times; // 사용된 횟수
	}
	
	public virtual void arrange_del_hearts()
	{
		for (int i = 0, len = objectStatus.heart_list.Count; i < len; i++)
		{
			if (objectStatus.heart_list[i] == ItemType.ITEM_NONE)
			{
				objectStatus.heart_list.RemoveAt(i);
				i--;
				len--;
			}
		}
	}

	public virtual void use_item(int item_idx)
	{
        used_item_List.Add(objectStatus.item_list[item_idx]);
        if (!objectStatus.item_list[item_idx].ToString().Contains("RED"))
		    objectStatus.item_list[item_idx] = ItemType.ITEM_NONE;
	}
    public void use_item_even_RED(int item_idx)
    {
        objectStatus.item_list[item_idx] = ItemType.ITEM_NONE;
    }

	public virtual void del_heart(int heart_idx)
	{
		//objectStatus.item_list.RemoveAt(item_idx);
		objectStatus.heart_list[heart_idx] = ItemType.ITEM_NONE;
	}

	public virtual void drop_heart(int item_idx)
	{
		objectStatus.heart_list.RemoveAt(item_idx);
	}


	public virtual void drop_item(int item_idx)
    {
        objectStatus.item_list.RemoveAt(item_idx);
    }

    public virtual bool death()
	{
		if (this.ObjectBody.GetComponent<ObjectBody>())
        {
            this.ObjectBody.GetComponent<ObjectBody>().DeathTrigger = true;
            this.ObjectBody.GetComponent<ObjectBody>().RoomNowForDeathEffect = gameManager.roomMeta_now;
        }
        else
        {
            Debug.Log("there is no objectbody script");
        }
        for(int i = 0; i < ItemUIList.Count; i++)
        {
            gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(ItemUIList[i], AnimType.DEATH, 0);
        }
        for(int i = 0; i < heartUIList.Count; i++)
        {
            if(heartUIList[i])
                gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(heartUIList[i], AnimType.DEATH, 0);
        }
		for (int i = 0; i < blueheartUIList.Count; i++)
		{
			if (blueheartUIList[i])
				gameManager.sub_managers.animManager.animCoroutinePool.run_coroutine_withoutbase(blueheartUIList[i], AnimType.DEATH, 0);
		}

		gameManager.roomMeta_now.room_stain_list.Add(this.ObjectBody);
		gameManager.sub_managers.animManager.anim_add(this, AnimType.DEATH, 0, 1);
		node_now.object_here_list.Remove(this); 
		is_alive = false;

		return true;

		//GameManager.Instance.sub_managers.animManager.anim_add(this, AnimType.DEATH, 0, _priority: 0);


		// unactive(); 이거 여기서 하면 안됨. 애님매니저 실행후 해야됨.
	}

	public virtual void UI_process()
    {

    }
    public virtual void set_itemEffect()
    {
        used_item_List.Clear();
        
    }

    public void direction(ObjectBase to_objectBase)
	{
		Node from_node = node_now;
		Node to_node = to_objectBase.node_now;
		if (from_node.xy[0] > to_node.xy[0])
		{
			this.set_UDLR(0);
			to_objectBase.set_UDLR(1);
		}
		else if (from_node.xy[0] < to_node.xy[0])
		{
			this.set_UDLR(1);
			to_objectBase.set_UDLR(0);
		}
		else if (from_node.xy[1] > to_node.xy[1])
		{
			this.set_UDLR(2);
			to_objectBase.set_UDLR(3);
		}
		else if (from_node.xy[1] < to_node.xy[1])
		{
			this.set_UDLR(3);
			to_objectBase.set_UDLR(2);
		}
	}

	public void direction(Node to_node)
	{
		Node from_node = node_now;

		if (from_node.xy[0] > to_node.xy[0])
		{
			this.set_UDLR(0);
		}
		else if (from_node.xy[0] < to_node.xy[0])
		{
			this.set_UDLR(1);
		}
		else if (from_node.xy[1] > to_node.xy[1])
		{
			this.set_UDLR(2);
		}
		else if (from_node.xy[1] < to_node.xy[1])
		{
			this.set_UDLR(3);
		}
		
	}
    public void direction_onleLR(Node targetnode)
    {
        if (node_now.position.x > targetnode.position.x)
        {
            UDLR = 2;
        }
        else
        {
            UDLR = 3;
        }
    }

}

public class ObjectStatus
{
	public ObjectStatus(ObjectBaseType objectBaseType)
	{
		thisType = objectBaseType;
	}
	public ObjectBaseType thisType;
	
	// public List<ObjectBase> item_list = new List<ObjectBase>();
	public List<ItemType> item_list = new List<ItemType>();
	public List<ItemType> heart_list = new List<ItemType>();
	public int heart = 0;
	public int blueheart = 0;

	public void add_heart(int n)
	{

		heart += n;
	}

	public void add_blueheart(int n)
	{

		blueheart += n;
	}

	public bool del_heart_and_blueheart(int[] ns)
	{

		heart -= ns[0];
		blueheart -= ns[1];

		if (heart + blueheart == 0)
		{
			return true; // 죽으면 true
		}

		else return false;
	}


	public bool del_heart(int n)
	{

		heart -= n;

		if (heart + blueheart == 0)
		{
			return true; // 죽으면 true
		}

		else return false;
	}

	public bool del_blueheart(int n)
	{

		blueheart -= n;

		if (heart + blueheart == 0)
		{
			return true; // 죽으면 true
		}

		else return false;
	}

}