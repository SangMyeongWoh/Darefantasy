using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase_Box : BlockBase
{
	public BlockBase_Box(BlockType subtype, Node _node)
	{
		init_ObjectBase(ObjectBaseType.BLOCK, (int)subtype, _node);
	}
}
