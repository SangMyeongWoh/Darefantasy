using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase_Normal : TrapBase
{
	public TrapBase_Normal(TrapType subtype, Node _node)
	{
		init_ObjectBase(ObjectBaseType.TRAP, (int)subtype, _node);
        movable = false;
    }

    public override bool is_target(ObjectBase targetbase)
    {
        if (targetbase is PlayerBase || targetbase is MonsterBase) return true;

        return false;
    }


    public override int[] attack(ObjectBase targetBase)
    {
        //int target_life = targetBase.objectStatus.heart;
        int target_life = target_life_with_shield(targetBase);
        int damage = 0;
        int blue_damage = 0;
        //direction(targetBase);

        if (targetBase is MonsterBase || targetBase is PlayerBase)
        {
            damage = 1;
        }

        // anim 
        gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 0);
        gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 0);
        gameManager.sub_managers.animManager.anim_add(this, AnimType.ATTACK, UDLR, 0);
        return new int[2] { damage, blue_damage };
    }

    public override Node next_node()
    {
        //Node next_node = Tools.BFS.get_next_node(this, gameManager.playerBase);
        Node next_node = node_now;
        return next_node;
    }
    public override bool interaction(ObjectBase objectBase)
    {
        //objectBase.hit(attack(objectBase));

        return false;
    }

}
