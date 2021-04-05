using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectBodyPool : MonoBehaviour
{
    public delegate GameObject spawn_body(BlockType BlockType, Vector3 pos);
    public ObjectBodyPool()
    {
        
    }
    private void Start()
    {
        bodyPools = new BodyPools();
        bodyPools.itemBodyPool = gameObject.GetComponentInChildren<ItemBodyPool>();
        bodyPools.MonsterBodyPool = gameObject.GetComponentInChildren<MonsterBodyPool>();
        bodyPools.doorBodyPool = gameObject.GetComponentInChildren<DoorBodyPool>();
        bodyPools.blockBodyPool = gameObject.GetComponentInChildren<BlockBodyPool>();
        bodyPools.TrapBodyPool = gameObject.GetComponentInChildren<TrapBodyPool>();
    }

    public BodyPools bodyPools;

    #region get_Body

    public GameObject get_body(BlockType blockType, Vector3 pos) //block
    {
        return bodyPools.blockBodyPool.spawn_Block_body(blockType, pos);
    }

    public GameObject get_body(DoorType doorType, Vector3 pos, int UDLR) //door
    {
        return bodyPools.doorBodyPool.spawn_door_body(doorType, pos, UDLR);
    }

    public GameObject get_body(ItemType itemType, Vector3 pos) //item
    {
        return bodyPools.itemBodyPool.spawn_item_body(itemType, pos);
    }

    public GameObject get_body(ItemUIType itemUIType, Vector3 pos, bool forCharacter) //itemUI
    {
        return bodyPools.itemBodyPool.spawn_item_UI_body(itemUIType, pos, forCharacter);
    }

    public GameObject get_body(MonsterType monsterType, Vector3 pos) //itemUI
    {
        return bodyPools.MonsterBodyPool.spawn_monster_body(monsterType, pos);
    }

    public GameObject get_body(TrapType trapType, Vector3 pos) //trap
    {
        return bodyPools.TrapBodyPool.spawn_Trap_body(trapType, pos);
    }

    #endregion

}
public class BodyPools
{
    public BlockBodyPool blockBodyPool;
    public DoorBodyPool doorBodyPool;
    public ItemBodyPool itemBodyPool;
    public MonsterBodyPool MonsterBodyPool;
    public TrapBodyPool TrapBodyPool;
}