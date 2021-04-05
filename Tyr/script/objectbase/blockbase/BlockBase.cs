using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : ObjectBase
{
    public BlockType blockType;

	public override void init_ObjectBase(ObjectBaseType _objectBaseType, int subtype, Node _node)
	{
		base.init_ObjectBase(_objectBaseType, subtype, _node);

		blockType = (BlockType)subtype;
		is_collide = true;
		objectStatus.heart = 1;
	}
    /*
	public override bool hit(int[] damages)
	{
        
		return false;
	}
    
	public override bool hit(int damage)
	{
        if (objectStatus.del_heart(damage))
		{            
			death();
			return true;
		}
		return false;
	}*/
}
