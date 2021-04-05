using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : ObjectBase
{
	public RoomMeta connected_roommeta;
	public Node connected_node;
	public DoorType doorType;
	public DoorType unlocked_doorType;

	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);
		doorType = (DoorType)subtype;
	}

	public override void set_UDLR(int _UDLR) {
	}

	public void get_connected_roommeta(RoomMeta roomMeta)
	{
		connected_roommeta = roomMeta;
	}

	public override void active()
	{
		base.active();
		if (unlocked_doorType != DoorType.NONE && node_now.roommeta.room_locked == false && connected_roommeta.room_locked == false)
		{
			already_open();
		}
	}

	public override bool hit(int[] damages)
	{
		return hit(damages[0]);
	}


	public void already_open()
	{
		is_collide = false;
		doorType = unlocked_doorType;
		unlocked_doorType = DoorType.NONE;

		Vector3 position = new Vector3(ObjectBody.transform.position.x, ObjectBody.transform.position.y);
		ObjectBody.SetActive(false);
		ObjectBody = gameManager.pools.objectBodyPool.get_body(doorType, position, UDLR);
	}


	public override bool interaction(ObjectBase objectBase)
	{
		if (objectBase is PlayerBase)
		{
			return true;
		//	GameManager.Instance.sub_managers.playerManager.room_process(connected_roommeta);
		}
		return false;
	}
}
