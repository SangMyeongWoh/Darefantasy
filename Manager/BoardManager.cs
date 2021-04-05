using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	// 여기에 objectbase pool, room 들어가야 할듯.

	public GameManager gameManager;
    public GameObject key_nav;
	public Room room;
	public Pools pools;

	public GameObject[] playerPrefab;
	public GameObject[] monsterPrefab;

	public GameObject[] itemPrefab;


	Dictionary<int, int> door_tag = new Dictionary<int, int>
	{
		{ -1, 1}, {0, 0}, {1, 2},
	};

	private void Start()
	{
		gameManager = GameManager.Instance;
		room = gameManager.GetComponentInChildren<Room>();
		pools = gameManager.pools;
        
	}

	public void room_transition(RoomMeta next_room)
	{
		resetRoomMeta(gameManager.roomMeta_now);
        gameManager.roomMeta_now = next_room;
        setRoom(next_room);		      
    }
	public void get_player_turn()
	{
		gameManager.player_turn = true;
	}
	
	
	Node RandomNode(RoomMeta roomMeta, bool Itemduplicate=true)
	{
        int random_x, random_y;
        for(int i = 0; i < 10; i++)
        {
            random_x = Random.Range(0, Constant.MAX_INDEX);
            random_y = Random.Range(0, Constant.MAX_INDEX);
            if(roomMeta.node_list[random_x][random_y].object_here_list.Count == 0)
            {
                return roomMeta.node_list[random_x][random_y];
            }
            else
            {
                bool isOk = true;
                for(int j = 0; j < roomMeta.node_list[random_x][random_y].object_here_list.Count; j++)
                {
                    if (Itemduplicate)
                    {
                        if (roomMeta.node_list[random_x][random_y].object_here_list[j].objectBase_type == ObjectBaseType.BLOCK ||
                        roomMeta.node_list[random_x][random_y].object_here_list[j].objectBase_type == ObjectBaseType.MONSTER)
                        {
                            isOk = false;
                            break;
                        }
                    }
                    else
                    {
                        if (roomMeta.node_list[random_x][random_y].object_here_list[j].objectBase_type == ObjectBaseType.BLOCK ||
                        roomMeta.node_list[random_x][random_y].object_here_list[j].objectBase_type == ObjectBaseType.MONSTER ||
                        roomMeta.node_list[random_x][random_y].object_here_list[j].objectBase_type == ObjectBaseType.ITEM)
                        {
                            isOk = false;
                            break;
                        }
                    }
                    
                }
                if(isOk) return roomMeta.node_list[random_x][random_y];

            }
        }
        return null;
        
	}

	public ObjectBase init_player()
	{
		Node randomNode = RandomNode(gameManager.roomMeta_now);
        Vector3 randomPosition = randomNode.position;
		ObjectBase objectbase = create_object(ObjectBaseType.CHARACTER, 0, randomNode);
		
		return objectbase;
	}
	public ObjectBase create_object(ObjectBaseType objectbaseType, int subType, Node node, int UDLR = 0) {
		
		Vector3 _position = node.position;
		ObjectBase objectbase = pools.objectBasePool.call_object_base(objectbaseType, subType, node, UDLR); /// subtype 저으이필요.
        if (objectbase == null) return null;
		switch (objectbaseType)
		{
			case ObjectBaseType.CHARACTER:
				objectbase.ObjectBody = gameManager.pools.objectBodyPool.get_body(MonsterType.CHARACTER, _position);
                objectbase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", 7);
				break;

			case ObjectBaseType.MONSTER:
				objectbase.ObjectBody = gameManager.pools.objectBodyPool.get_body((MonsterType)subType, _position);
                for(int i = 0; i < objectbase.ItemUIList.Count; i++)
                {
                    objectbase.ItemUIList[i].transform.parent = objectbase.ObjectBody.transform;
				}
				for (int i = 0; i < objectbase.heartUIList.Count; i++)
				{
					objectbase.heartUIList[i].transform.parent = objectbase.ObjectBody.transform;
				}
				for (int i = 0; i < objectbase.blueheartUIList.Count; i++)
				{
					objectbase.blueheartUIList[i].transform.parent = objectbase.ObjectBody.transform;
				}
				break;
	
			case ObjectBaseType.ITEM:
				objectbase.ObjectBody = gameManager.pools.objectBodyPool.get_body((ItemType)subType, _position);
				break;

			case ObjectBaseType.DOOR:

				objectbase.ObjectBody = gameManager.pools.objectBodyPool.get_body((DoorType)subType, _position, UDLR);
				break;

			case ObjectBaseType.BLOCK:                
				objectbase.ObjectBody = gameManager.pools.objectBodyPool.get_body((BlockType)subType, _position);
				break;

			case ObjectBaseType.TRAP:
				objectbase.ObjectBody = gameManager.pools.objectBodyPool.get_body((TrapType)subType, _position);
				break;

			default:
				Debug.Log("IF IT LOOKS LIKE DOOR, ADD GETBODY IN BOARDMANAGER");
				objectbase.ObjectBody = gameManager.pools.objectBodyPool.get_body((DoorType)subType, _position, UDLR);

				break;

		}
        node.object_here_list.Add(objectbase);

		return objectbase;
	}

	// Object Base.. RoomTransition때 생성.
	// Prefab이랑 연결.



	public void setRoom(RoomMeta roomMeta)
	{
        
        set_RoomMeta(roomMeta);
        
        gameManager.room.GetComponent<Room>().setTiles(roomMeta, gameManager.stageType);
        
        foreach (var list in roomMeta.node_list)
		{
			foreach (var node in list)
			{
				//foreach (var object_base in node.object_here_list) // for문 안에서 지워질 수 있음.
				for ( int i=0; i< node.object_here_list.Count; i++)
				{
					if (!node.object_here_list[i].is_alive || node.object_here_list[i].node_now != node)
					{
						node.object_here_list.Remove(node.object_here_list[i]);
						i--;
					}
					else node.object_here_list[i].active();
				}
			}
		}
		foreach (var node in roomMeta.UDLR_door) //문 배치
		{
			for (int i = 0; i< node.object_here_list.Count; i++) // len 2 count 2 i 0
			{
				if (node.object_here_list[0].node_now != node) 
				{
					node.object_here_list.Remove(node.object_here_list[i]);
					i--;
				}
				else
				{
					node.object_here_list[i].active();
				}
			}
		}


		roomMeta.instantDone = true;
	}

	string string_room(int room_index)
	{
		// 108
		int floor = 5 - room_index / 25; // 5 - 4 = 1
		int dirxy = room_index % 25; // 108 - 100 = 8
		int dirxy_x = dirxy / 5; // 8 / 5 = 1
		int dirxy_y = dirxy % 5; // 8 % 5 = 3


		return floor.ToString() + "th [[" + dirxy_x.ToString() + ", " + dirxy_y.ToString() +"]] ";
	}

	public void resetRoomMeta(RoomMeta roomMeta)
	{
		//오브젝트베이스중에서 플레이어 타입은 특별취급해야함
		//노드 깔끔하게 다 정리하는 resetRoom

		
		foreach (var list in roomMeta.node_list)
		{
			foreach (var node in list)
			{
				foreach (var object_base in node.object_here_list)
				{
					if (object_base is PlayerBase)
					{
						continue; // 캐릭터는 따로 처리.
					}
					if (object_base.is_alive == false)
					{
						//node.object_here_list.Remove(object_base);
					}
					object_base.unactive();
				}
				// Debug.Log(node.xy[0].ToString() + ",,," + node.xy[1].ToString());
			}
		}

		foreach (var node in roomMeta.UDLR_door)
		{
			foreach (var object_base in node.object_here_list)
			{
				object_base.unactive();
			}
		}
        for(int i = 0; i < roomMeta.room_stain_list.Count; i++)
        {
            if (roomMeta.room_stain_list[i].GetComponent<ObjectBody>()
                && roomMeta.room_stain_list[i].GetComponent<ObjectBody>().needDeathEffect
                && roomMeta.room_stain_list[i].GetComponent<ObjectBody>().DeathTrigger
                && !roomMeta.room_stain_list[i].GetComponent<ObjectBody>().DeathEffectSpawnDone)
            {

                GameObject death = Instantiate(GameManager.Instance.pools.objectBodyPool.bodyPools.MonsterBodyPool.DeathEffect, roomMeta.room_stain_list[i].transform.position, transform.rotation) as GameObject;
                GameManager.Instance.garbageBox.garbageList.Add(death);
                roomMeta.room_stain_list.Add(death);
                roomMeta.room_stain_list[i].GetComponent<ObjectBody>().DeathEffectSpawnDone = true;

            }
            roomMeta.room_stain_list[i].SetActive(false);
        }
	}

	void set_RoomMeta(RoomMeta roomMeta)
	{
        if (roomMeta.room_locked) roomMeta.room_locked = false;

		if (!roomMeta.instantDone)
        {            
            if(roomMeta.roomType == RoomType.NORMAL)
            {
                if (Random.Range(0,3) > 0) create_random_room(roomMeta);
                else create_saved_room(roomMeta);
                
                
                
            }
            else
            {
                create_saved_room(roomMeta);
            }
            create_random_monster(roomMeta);            
            create_random_item(roomMeta);
            door_connection(roomMeta);

        }
        else // instance done 룸
        {

            foreach (var stain in roomMeta.room_stain_list)
            {
                if (stain.GetComponent<ObjectBody>())
                {
                    if(!stain.GetComponent<ObjectBody>().DeathTrigger) stain.SetActive(true);
                    continue;

                }
                stain.SetActive(true);
                
            }
        }
        if (roomMeta.roomType == RoomType.INITIAL && gameManager.stagelv == 1) key_nav.SetActive(true);
        else key_nav.SetActive(false);
		// random setting function 구현 필요.
		// RoomMetatype을 읽어서 objectbase_list를 새로 랜덤으로 셋팅. objectbasepool의 call objectbase사용
		

		//roomMeta.instantDone = true;
	}
    void set_instance_done_Room(RoomMeta roomMeta)
    {

    }


	void door_connection(RoomMeta roomMeta) // 
	{
        for (int UDLR = 0; UDLR < 4; UDLR++) {
			if (roomMeta.UDLR_room[UDLR] != null)
			{
				Node connected_node = null;
				switch (UDLR)
				{
					case 0:
                        connected_node = roomMeta.node_list[0][2];
                        break;
					case 1:
                        connected_node = roomMeta.node_list[4][2];
						break;
					case 2:
						connected_node = roomMeta.node_list[2][0];
						break;
					case 3:
						connected_node = roomMeta.node_list[2][4];
						break;
					default:
						break;
				}

				connected_node.UDLR_node[UDLR] = roomMeta.UDLR_door[UDLR];
				roomMeta.UDLR_door[UDLR].UDLR_node[opposite_UDLR(UDLR)] = connected_node;
				roomMeta.UDLR_door[UDLR].UDLR_node[UDLR] = roomMeta.UDLR_room[UDLR].UDLR_door[opposite_UDLR(UDLR)];
                
                //Debug.Log("substract = " + door_tag[roomMeta.UDLR_room[UDLR].room_index - roomMeta.room_index].ToString());
                ObjectBase temp_base;
                
				if (roomMeta.room_locked || roomMeta.UDLR_room[UDLR].room_locked)
				{ // 잠긴문.
					switch (door_tag[(roomMeta.UDLR_room[UDLR].location - roomMeta.location).x])
					{
						case 0: // same floor
							temp_base = create_object(ObjectBaseType.DOOR, (int)DoorType.LOCKED_DOOR_NORAML, roomMeta.UDLR_door[UDLR], UDLR);
							break;
						case 1: // Upper
							temp_base = create_object(ObjectBaseType.DOOR, (int)DoorType.LOCKED_DOOR_UP_STAIR, roomMeta.UDLR_door[UDLR], UDLR);
							break;
						case 2: // Lower
							temp_base = create_object(ObjectBaseType.DOOR, (int)DoorType.LOCKED_DOOR_DOWN_STAIR, roomMeta.UDLR_door[UDLR], UDLR);
							break;
						default:
							temp_base = null;
							break;
					}
				}
				else
				{
					switch (door_tag[(roomMeta.UDLR_room[UDLR].location - roomMeta.location).x])
					{
						case 0: // same floor
							temp_base = create_object(ObjectBaseType.DOOR, (int)DoorType.NORMAL, roomMeta.UDLR_door[UDLR], UDLR);
							break;
						case 1: // Upper
							temp_base = create_object(ObjectBaseType.DOOR, (int)DoorType.UP_STAIR, roomMeta.UDLR_door[UDLR], UDLR);
							break;
						case 2: // Lower
							temp_base = create_object(ObjectBaseType.DOOR, (int)DoorType.DOWN_STAIR, roomMeta.UDLR_door[UDLR], UDLR);
							break;
						default:
							temp_base = null;
							break;
					}
				}
                //이 위에 10월 11일에 임시로 테스트하려고 주석넣은거고
                //이 아래는 테스트용으로 그냥 무조건 옆문만 생성
                //temp_base = create_object(ObjectBaseType.DOOR, (int)DoorType.NORMAL, roomMeta.UDLR_door[UDLR], UDLR);

                ////위에는 테스트. 440번째줄까지가 테스트야 10월 11일
                ((DoorBase)temp_base).connected_roommeta = roomMeta.UDLR_room[UDLR];
				((DoorBase)temp_base).connected_node = roomMeta.UDLR_door[UDLR].UDLR_node[UDLR];
            }
		}
	}

    void create_saved_room(RoomMeta roomMeta)
    {
        List<NodeSetter> nodeSetters = gameManager.pools.roomPools.get_roomsetters(gameManager.stagelv, roomMeta.roomType);
        
        for (int i = 0; i < nodeSetters.Count; i++)
        {
            if (nodeSetters[i].objectBaseType != ObjectBaseType.NONE)
            {
                create_object(nodeSetters[i].objectBaseType, nodeSetters[i].subtype, roomMeta.node_list[nodeSetters[i].nodeindex / 5][(int)(nodeSetters[i].nodeindex % 5)]);
            }
        }
        


    }
    void create_random_monster(RoomMeta roomMeta)
    {
        switch (roomMeta.roomType)
        {
            case RoomType.NORMAL:
                create_random_monster_normalroom(roomMeta);
                break;
            case RoomType.WELL:
                create_random_monster_wellroom(roomMeta);
                break;
            case RoomType.WORKSHOP:
                create_random_monster_workshop(roomMeta);
                break;
        }
    }
    void create_random_room(RoomMeta roomMeta)
    {
        for(int i = 0; i < 12; i++)
        {
            switch (Random.Range(0, 9))
            {
                case 0:
                    random_create(roomMeta, ObjectBaseType.BLOCK, (int)BlockType.CASE);
                    break;
                case 1:
                    random_create(roomMeta, ObjectBaseType.BLOCK, (int)BlockType.CASE_LOCKED);
                    break;
                default:
                    create_random_block(roomMeta);
                    break;

            }

        }
    }

    void create_random_block(RoomMeta roomMeta)
    {
        switch (gameManager.stagelv)
        {
            case 1:
                random_create(roomMeta, ObjectBaseType.BLOCK, (int)BlockType.BOX);
                break;
            case 2:
                int setnum = Random.Range(0, 3);
                if(setnum == 0) random_create(roomMeta, ObjectBaseType.BLOCK, (int)BlockType.BOX);
                else if(setnum == 1) random_create(roomMeta, ObjectBaseType.TRAP, (int)TrapType.NORMAL_TYPE2);
                else random_create(roomMeta, ObjectBaseType.TRAP, (int)TrapType.NORMAL_TYPE3);
                break;
        }
    }

    #region monster spawnfunction
    void create_random_zombie(RoomMeta roomMeta)
    {
        if (Random.Range(0,2) == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    if(GameManager.Instance.stagelv > 1)
                    {

                    }
                    else
                    {
                        random_create(roomMeta, ObjectBaseType.MONSTER, (int)MonsterType.ZOMBIE);
                    }
                    
                }
                    
            }
        }
        
        
    }
    void create_random_ghost(RoomMeta roomMeta, int numberOfGhost=1)
    {
        for (int i = 0; i < numberOfGhost; i++)
        {
            random_create(roomMeta, ObjectBaseType.MONSTER, (int)MonsterType.GHOST);
        }
    }
    void create_random_zombie_toungue(RoomMeta roomMeta, int numberOfZombie=1)
    {
        for (int i = 0; i < numberOfZombie; i++)
        {
            random_create(roomMeta, ObjectBaseType.MONSTER, (int)MonsterType.ZOMBIE_TONGUE);
        }
    }
    #endregion
    #region normalroom_monster
    void create_random_monster_normalroom(RoomMeta roomMeta)
    {
        switch (GameManager.Instance.stagelv)
        {
            case 1:
                create_random_zombie(roomMeta);
                break;
            case 2:
                create_random_zombie(roomMeta);
                break;
        }
    }
    #endregion
    #region wellroom_monster
    void create_random_monster_wellroom(RoomMeta roomMeta)
    {
        switch (GameManager.Instance.stagelv)
        {
            case 2:
                int setnum = Random.Range(0, 3);
                if(setnum == 0) create_random_ghost(roomMeta);                
                else if(setnum == 1) create_random_zombie(roomMeta);
                break;
        }
    }
    #endregion
    #region workshop_monster
    void create_random_monster_workshop(RoomMeta roomMeta)
    {        
        switch (GameManager.Instance.stagelv)
        {
            case 2:
                int setnum = Random.Range(0, 5);
                if (setnum == 0) create_random_ghost(roomMeta);
                else if (setnum == 1) create_random_zombie(roomMeta);
                break;
        }
    }
    #endregion


    void create_random_item(RoomMeta roomMeta)
    {
        switch (roomMeta.roomType)
        {
            #region case Normal
            case RoomType.NORMAL:                
                for (int i = 0; i < 2; i++)
                {
                    switch (Random.Range(0, 12))
                    {
                        case 0:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_HEART);
                            break;
                        case 1:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_KEY);
                            break;
                        case 2:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_SHIELD);
                            break;
                        case 3:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_SWORD);
                            break;
                        default:
                            break;
                    }
                }
                break;
            #endregion
            #region case Well
            case RoomType.WELL: 
                if (Random.Range(0, 3) == 0)
                {
                    for(int i = 0; i < 25; i++)
                        if(Random.Range(0,2) == 0) random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_POOP);
                }
                break;
            #endregion
            #region case WORKSHOP
            case RoomType.WORKSHOP:
                if (Random.Range(0, 3) == 0)
                {
                    for (int i = 0; i < 15; i++)
                        if (Random.Range(0, 2) == 0) random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_POOP);
                }
                break;
            #endregion
            #region case INITIAL
            case RoomType.INITIAL:
                for (int i = 0; i < 2; i++)
                {
                    switch (Random.Range(0, 6))
                    {
                        case 0:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_HEART);
                            break;
                        case 1:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_KEY);
                            break;
                        case 2:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_SHIELD);
                            break;
                        case 3:
                            random_create(roomMeta, ObjectBaseType.ITEM, (int)ItemType.ITEM_SWORD);
                            break;
                        default:
                            break;
                    }
                }
                break;
            #endregion

        }
    }
    void random_create(RoomMeta roomMeta, ObjectBaseType objectbase_type, int subType)
	{
        Node randomNode;
        if(objectbase_type == ObjectBaseType.ITEM || objectbase_type == ObjectBaseType.BLOCK)
		    randomNode = RandomNode(roomMeta, false);
        else
            randomNode = RandomNode(roomMeta);
        //Vector3 randomPosition = randomNode.position;
        if (randomNode == null) return;
		//roomMeta.object_base_list.Add(create_object(objectbase_type, randomNode));
		randomNode = find_neighbor_node(roomMeta, randomNode);
		create_object(objectbase_type, subType, randomNode);

	}
    


    int opposite_UDLR(int UDLR)
	{
		if (UDLR % 2 == 0)
		{
			return UDLR + 1;
		}
		else
		{
			return UDLR - 1;
		}
	}

	Node find_neighbor_node(RoomMeta roommeta, Node node)
	{
		if (node.object_here_list.Count == 0) return node;

		bool check = true;
		foreach (var objectbase in node.object_here_list)
		{
			if(objectbase.is_collide) check = false;
		}
		if (check) return node;

		for (int distance = 1; distance < 10; distance++)
		{
			for (int i = 0; i < Constant.MAX_INDEX; i++)
			{
				for (int j = 0; j < Constant.MAX_INDEX; j++)
				{
					if (Mathf.Abs(node.xy[0] - i) + Mathf.Abs(node.xy[1] - j) != distance) continue;
					if (roommeta.node_list[i][j] == null) continue;
					if (roommeta.node_list[i][j].is_extra_node) continue;

					if (roommeta.node_list[i][j].object_here_list.Count == 0) return roommeta.node_list[i][j];
				}
			}
		}

		return null;
	}

}
