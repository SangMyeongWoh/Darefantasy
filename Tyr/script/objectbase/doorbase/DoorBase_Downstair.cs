using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase_Downstair : DoorBase
{
	public DoorBase_Downstair(DoorType subtype, Node _node, int _UDLR)
	{
		init_ObjectBase(ObjectBaseType.DOOR, (int)subtype, _node);
		UDLR = _UDLR;
	}

	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);
		unlocked_doorType = DoorType.NONE;
		is_collide = false;
	}



}
