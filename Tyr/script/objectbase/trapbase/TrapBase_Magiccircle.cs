using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase_Magiccircle : TrapBase
{
	public TrapBase_Magiccircle(TrapType subtype, Node _node)
	{
		init_ObjectBase(ObjectBaseType.TRAP, (int)subtype, _node);
	}

	public override bool interaction(ObjectBase objectBase)
	{
		if (objectBase is PlayerBase)
		{
			if (gameManager.wellpoint == Constant.WELL_POINT)
			{
				//gameManager.stage_start(1);                  
				ObjectBody.GetComponent<MagicCircle>().stage_up();


			}
			else Debug.Log("need more well point");
		}

		return false;
	}


}
