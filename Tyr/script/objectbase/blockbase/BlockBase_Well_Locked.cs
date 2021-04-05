﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase_Well_Locked : BlockBase
{
    public bool isLocked = true;
    public BlockBase_Well_Locked(BlockType subtype, Node _node)
    {
        init_ObjectBase(ObjectBaseType.BLOCK, (int)subtype, _node);
        roomMeta_now = gameManager.roomMeta_now;
    }
    public void wellOn()
    {
        if (!roomMeta_now.WellON)
        {
            ObjectBody.GetComponent<well>().BloodProcess();
            roomMeta_now.WellON = true;
            gameManager.wellpoint += 1;
        }

    }
    public override bool hit(int[] damages)
    {
        death();
        return true;
    }
    public override bool death()
    {
        if (isLocked) open();
        else wellOn(); 

        return false;
    }
    void open()
    {
        ObjectBody.GetComponent<well>().unlock();
        isLocked = false;
    }
}
