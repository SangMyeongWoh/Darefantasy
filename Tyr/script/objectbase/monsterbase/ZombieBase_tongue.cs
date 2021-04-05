using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBase_tongue : MonsterBase
{
    bool range_attack = false;
    public ZombieBase_tongue(MonsterType subtype, Node _node)
    {
        init_ObjectBase(ObjectBaseType.MONSTER, (int)subtype, _node);

    }
    public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
    {

        base.init_ObjectBase(_objectBaseType, subtype, _node);
        movable = true;

    }
    public override Node next_node()
    {
        if (!range_attack)
        {
            Node next_node = Tools.BFS.get_next_node(this, gameManager.playerBase);
            range_attack = true;
            return next_node;
        }
        else
        {
            Node next_node = gameManager.playerBase.node_now;
            range_attack = false;
            return next_node;
        }
    }

    

    public override bool is_target(ObjectBase targetbase)
    {
        if (targetbase is PlayerBase) return true;
        return false;
    }

    public override void set_objectStatus()
    {
        // 공격력

        objectStatus.item_list.Add(ItemType.ITEM_SHIELD);
        objectStatus.item_list.Add(ItemType.ITEM_SWORD_RED);
        //objectStatus.item_list.Add(ItemType.ITEM_RED_SWORD);

        // 생명력 +1 
        objectStatus.heart++;

    }
    public override int[] attack(ObjectBase targetBase)
    {
        direction_onleLR(targetBase.node_now);
        attackanim();
        return base.attack(targetBase);
    }
    public void attackanim()
    {

    }
}
