using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPools
{
    WellRoomPool wellRoomPool = new WellRoomPool();
    NormalRoomPool normalRoomPool = new NormalRoomPool();
    InitialRoomPool initialRoomPool = new InitialRoomPool();
    MagicCircleRoomPool magicCircleRoomPool = new MagicCircleRoomPool();
    WorkShopRoomPool workShopRoomPool = new WorkShopRoomPool();
   

    public RoomPools()
    {
        set_RoomPools();
    }

    public void set_RoomPools()
    {
        wellRoomPool.setpool(RoomType.WELL);
        normalRoomPool.setpool(RoomType.NORMAL); 
        initialRoomPool.setpool(RoomType.INITIAL);
        magicCircleRoomPool.setpool(RoomType.MAGICCIRCLE);
        workShopRoomPool.setpool(RoomType.WORKSHOP);
    }

    public List<NodeSetter> get_roomsetters(int stagelv, RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.INITIAL:
                return initialRoomPool.get_nodesetters(stagelv);                
            case RoomType.MAGICCIRCLE:
                return magicCircleRoomPool.get_nodesetters(stagelv);
            case RoomType.NORMAL:                
                return normalRoomPool.get_nodesetters(stagelv);
            case RoomType.WELL:
                return wellRoomPool.get_nodesetters(stagelv);
            case RoomType.WORKSHOP:
                return workShopRoomPool.get_nodesetters(stagelv);
            default:
                return new List<NodeSetter>();
        }
    }
    
}
