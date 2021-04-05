using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase_LockedNormal : DoorBase
{
	public DoorBase_LockedNormal(DoorType subtype, Node _node, int _UDLR)
	{
		init_ObjectBase(ObjectBaseType.DOOR, (int)subtype, _node);
		UDLR = _UDLR;
	}

	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);
		unlocked_doorType = (DoorType)(subtype - (int)DoorType.LOCKED_DOOR);
		is_collide = true;
	}


	public override bool death()
	{
		// locked door death
		connected_roommeta.room_locked = false;
		ObjectBase temp_base = gameManager.sub_managers.boardManager.create_object(ObjectBaseType.DOOR, (int)unlocked_doorType, node_now, UDLR);
		((DoorBase)temp_base).connected_roommeta = connected_roommeta;
		((DoorBase)temp_base).connected_node = connected_node;

		return base.death();
	}
}
