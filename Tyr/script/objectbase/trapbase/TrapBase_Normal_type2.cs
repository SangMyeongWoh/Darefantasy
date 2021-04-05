﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase_Normal_type2 : TrapBase
{
    
    bool attackReady = false;
    public GameObject subSprite;
    public TrapBase_Normal_type2(TrapType subtype, Node _node, bool _attackReady)
    {        
        init_ObjectBase(ObjectBaseType.TRAP, (int)subtype, _node);
        movable = false;
        attackReady = _attackReady;
        
    }

    public override bool is_target(ObjectBase targetbase)
    {
        bool thereisenemy = false;
        foreach(var _base in targetbase.node_now.object_here_list)
        {
            if(_base is PlayerBase || _base is MonsterBase)
            {
                thereisenemy = true;
            }
        }
        if (attackReady)
        {
            if (thereisenemy)
            {
                if (targetbase is PlayerBase || targetbase is MonsterBase)
                {
                    return true;
                }
                else return false;
                
            }
            else return true;
            
        }
        else
        {
            return false;
        }
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
            gameManager.sub_managers.animManager.anim_add(this, AnimType.HIGHLIGHT, UDLR, 0);
            gameManager.sub_managers.animManager.anim_add(targetBase, AnimType.HIGHLIGHT, UDLR, 0);
            damage = 1;
        }
        
        gameManager.sub_managers.animManager.anim_add(this, AnimType.ATTACK, UDLR, 0);
        return new int[2] { damage, blue_damage };
    }

    public override Node next_node()
    {        
        //Node next_node = Tools.BFS.get_next_node(this, gameManager.playerBase);
        if (!attackReady){
            ObjectBody.GetComponent<Trap_normal_attackready>().activeattack();            
            attackReady = true;
        } else {
            ObjectBody.GetComponent<Trap_normal_attackready>().unactiveattack();
            attackReady = false;
        } 
        Node next_node = node_now;
        return next_node;
    }
    public override bool interaction(ObjectBase objectBase)
    {
        //objectBase.hit(attack(objectBase));

        return false;
    }

}
