using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : ObjectBase
{
    public TrapType trapType;
	
	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);
		trapType = (TrapType)subtype;
        turn_type = Turn_Type.Object_turn;
        has_turns = 1;
		is_collide = false;
        
	}


    public override int[] attack(ObjectBase targetBase)
	{
		//int target_life = targetBase.objectStatus.heart;
		int target_life = target_life_with_shield(targetBase);
		int damage = 0;
		int blue_damage = 0;

		direction(targetBase);

		if (targetBase is MonsterBase || targetBase is PlayerBase)
		{
			damage = 1;
		}

		// anim 
		gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 1);
		gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 1);
		gameManager.sub_managers.animManager.anim_add(this, AnimType.ATTACK, UDLR, 1);
		return new int[2] { damage, blue_damage };
	}
    
    public override bool interaction(ObjectBase objectBase)
	{
		return false;
	}

}
