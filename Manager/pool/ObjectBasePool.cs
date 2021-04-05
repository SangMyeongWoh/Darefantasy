using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBasePool : MonoBehaviour
{
	public Dictionary<ObjectBaseType, List<ObjectBase>> object_base_dict = new Dictionary<ObjectBaseType, List<ObjectBase>>();

	private void Start()
	{
		foreach (ObjectBaseType objectBaseType in System.Enum.GetValues(typeof(ObjectBaseType))){
			
			if (objectBaseType == ObjectBaseType.CHARACTER) continue;

			object_base_dict[objectBaseType] = new List<ObjectBase>();
		}
	}

    public ObjectBase call_object_base(ObjectBaseType objectBaseType, int subType, Node node, int UDLR = 0)
	{
		ObjectBase tempbase = null;
		switch (objectBaseType)
		{
			case ObjectBaseType.CHARACTER:
				tempbase = new PlayerBase((MonsterType)subType, node);
				break;

			case ObjectBaseType.MONSTER:
				if ((MonsterType)subType == MonsterType.ZOMBIE)
					tempbase = new ZombieBase((MonsterType)subType, node);
				if ((MonsterType)subType == MonsterType.GHOST)
					tempbase = new GhostBase((MonsterType)subType, node);
                if ((MonsterType)subType == MonsterType.ZOMBIE_TONGUE)
                    tempbase = new ZombieBase_tongue((MonsterType)subType, node);
                if ((MonsterType)subType == MonsterType.GHOST_LV2)
                    tempbase = new GhostBase((MonsterType)subType, node, 2);
                break;

			case ObjectBaseType.ITEM:
				tempbase =  new ItemBase((ItemType)subType, node);
				break;
			case ObjectBaseType.DOOR:
				if ((DoorType)subType == DoorType.NORMAL)
					tempbase = new DoorBase_Normal((DoorType)subType, node, UDLR);
				if ((DoorType)subType == DoorType.UP_STAIR)
					tempbase = new DoorBase_Upstair((DoorType)subType, node, UDLR);
				if ((DoorType)subType == DoorType.DOWN_STAIR)
					tempbase = new DoorBase_Downstair((DoorType)subType, node, UDLR);

				if ((DoorType)subType == DoorType.LOCKED_DOOR_NORAML)
					tempbase = new DoorBase_LockedNormal((DoorType)subType, node, UDLR);
				if ((DoorType)subType == DoorType.LOCKED_DOOR_UP_STAIR)
					tempbase = new DoorBase_LockedUpstair((DoorType)subType, node, UDLR);
				if ((DoorType)subType == DoorType.LOCKED_DOOR_DOWN_STAIR)
					tempbase = new DoorBase_LockedDownstair((DoorType)subType, node, UDLR);
				
				break;

			case ObjectBaseType.BLOCK:
                if (!checkDoor(node)) break;
				if ((BlockType)subType == BlockType.WELL)
                {
                    tempbase = new BlockBase_Well((BlockType)subType, node);
                }
                if ((BlockType)subType == BlockType.BOX)
                    tempbase = new BlockBase_Box((BlockType)subType, node);
					
				if ((BlockType)subType == BlockType.CASE)
					tempbase = new BlockBase_Case((BlockType)subType, node);
                if ((BlockType)subType == BlockType.CASE_LOCKED)
                    tempbase = new BlockBase_Case_Locked((BlockType)subType, node);
                if ((BlockType)subType == BlockType.VENDINGMACHINE)
					tempbase = new BlockBase_Vendingmachine((BlockType)subType, node);
                if ((BlockType)subType == BlockType.WELL_LOCKED)
                    tempbase = new BlockBase_Well_Locked((BlockType)subType, node);
                break;

			case ObjectBaseType.TRAP:
				if ((TrapType)subType == TrapType.MAGICCIRCLE)
					tempbase =  new TrapBase_Magiccircle((TrapType)subType, node);
				if ((TrapType)subType == TrapType.NORMAL)
                {
                    tempbase = new TrapBase_Normal((TrapType)subType, node);
                }
                    
                if ((TrapType)subType == TrapType.NORMAL_TYPE2)
                {
                    tempbase = new TrapBase_Normal_type2((TrapType)subType, node, false);
                }
                if ((TrapType)subType == TrapType.NORMAL_TYPE3)
                {
                    tempbase = new TrapBase_Normal_type2((TrapType)subType, node, true);
                    
                }

                break;
			default:
				break;
		}
		return tempbase;

		/*
		if (objectBaseType == ObjectBaseType.CHARACTER) // player경우 pool 안쓰는경우.
		{
			return new PlayerBase((MonsterType)subType, node);
		}
		
		for (int i = 0; i < object_base_dict[objectBaseType].Count; i++)
		{
			if (!object_base_dict[objectBaseType][i].is_alive)
			{
				object_base_dict[objectBaseType][i].init_ObjectBase(objectBaseType, subType, node);
				return object_base_dict[objectBaseType][i];
			}
		}
		create_object_base(objectBaseType, subType, node, UDLR);
		return object_base_dict[objectBaseType][object_base_dict[objectBaseType].Count - 1];
		*/
	}

	/*
	public void create_object_base(ObjectBaseType objectBaseType, int subType, Node node, int UDLR = 0)
	{

		switch (objectBaseType)
		{
			case ObjectBaseType.MONSTER:

				object_base_dict[objectBaseType].Add(new MonsterBase((MonsterType)subType, node));
				break;
			case ObjectBaseType.ITEM:
				object_base_dict[objectBaseType].Add(new ItemBase((ItemType)subType, node));
				break;
			case ObjectBaseType.DOOR:
				object_base_dict[objectBaseType].Add(new DoorBase((DoorType)subType, node, UDLR));
				break;
				
			case ObjectBaseType.BLOCK:

				object_base_dict[objectBaseType].Add(new BlockBase((BlockType)subType, node));
				break;
			
			case ObjectBaseType.TRAP:

				object_base_dict[objectBaseType].Add(new TrapBase((TrapType)subType, node));
				break;
			default:
				Debug.Log("ADD OBJECTPOOL DICTIONARY");
				break;
				

		}
	}
	*/
    bool checkDoor(Node node)
    {
        if(node.roommeta.UDLR_door[0] != null)
        {
            if(node.position == new Vector3(0, 3, 0) ||
               node.position == new Vector3(0, 1.5f, 0) ||
               node.position == new Vector3(1.5f, 3f, 0) ||
               node.position == new Vector3(-1.5f, 3f, 0))
            {
                return false;
            }            
        }
        if (node.roommeta.UDLR_door[1] != null)
        {
            if (node.position == new Vector3(0, -3, 0) ||
               node.position == new Vector3(0, -1.5f, 0) ||
               node.position == new Vector3(1.5f, -3f, 0) ||
               node.position == new Vector3(-1.5f, -3f, 0))
            {
                return false;
            }
        }
        if (node.roommeta.UDLR_door[2] != null)
        {
            if (node.position == new Vector3(-3, 0, 0) ||
               node.position == new Vector3(-1.5f, 0f, 0) ||
               node.position == new Vector3(-3f, -1.5f, 0) ||
               node.position == new Vector3(-3f, 1.5f, 0))
            {
                return false;
            }
        }
        if (node.roommeta.UDLR_door[3] != null)
        {
            if (node.position == new Vector3(3, 0, 0) ||
               node.position == new Vector3(1.5f, 0f, 0) ||
               node.position == new Vector3(3f, -1.5f, 0) ||
               node.position == new Vector3(3f, 1.5f, 0))
            {
                return false;
            }
        }
        
        return true;
    }
	public void recall_object_base_list()
    {
		foreach (var key in object_base_dict.Keys)
		{
			foreach (var object_base in object_base_dict[key]) {
				//objectbase recall

			}
		}

	}
	
}
