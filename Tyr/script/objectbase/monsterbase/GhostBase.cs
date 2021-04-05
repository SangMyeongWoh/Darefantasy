using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBase : MonsterBase
{
	public GhostBase(MonsterType subtype, Node _node, int turn=1)
	{
		init_ObjectBase(ObjectBaseType.MONSTER, (int)subtype, _node);
        has_turns = turn;
	}
	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{

		base.init_ObjectBase(_objectBaseType, subtype, _node);        
        movable = true;

    }
    public override Node next_node()
    {
        Node next_node = Tools.BFS.get_next_node(this, gameManager.playerBase);

        return next_node;
    }
    public override bool is_target(ObjectBase targetbase)
    {
        if (targetbase is PlayerBase) return true;

        return false;
    }

    public override void set_objectStatus()
	{
		// 공격력
		
		objectStatus.item_list.Add(ItemType.ITEM_HOLYGRAIL_RED);
        objectStatus.item_list.Add(ItemType.ITEM_SWORD_RED);
        //objectStatus.item_list.Add(ItemType.ITEM_RED_SWORD);

        // 생명력 +1 
        objectStatus.blueheart++;

    }
	public override int[] attack(ObjectBase targetBase)
	{
        //ObjectBody.GetComponentInChildren<zombie>().setAttackSound();
        direction_onleLR(targetBase.node_now);
        return base.attack(targetBase);
	}

}
