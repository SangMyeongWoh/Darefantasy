using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MovingManager
{
    // Start is called before the first frame update

    public override void initManager(AnimManager _animManager)
    {
        thisManagerType = Turn_Type.Object_turn;
        base.initManager(_animManager);
    }

    public override void process()
    {
        if (running || !gameManager.object_turn || gameManager.anim_playing) return;
        running = true;
        base.process();
    }
    public override void search_here(ObjectBase objectBase)
    {
        // monster는 search 안함.
    }
    public override bool interaction(ObjectBase objectBase, ObjectBase targetBase)
    {
        // monster는 interface안함.
        return false;
    }
    

    public override void process_continue()
    {
        object_turn();
        base.process_continue();
    }
    public override void process_done()
    {
        player_turn();
        base.process_done();
    }


    public override Node get_next_node(ObjectBase objectBase)
    {

        Node next_node = objectBase.next_node();

        return next_node;
    }
}
