using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager
{
	
	public int[,] roomMeta = new int[Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX, 4];

	int[] visited = new int[Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX];
	int visit_idx = 0;
	int nth_group = 0;

	int[,] xy_z = new int[4, 3]{
		{ -5, -30, 20}, // U
		{ 5, -20, 30}, // D
		{ -1, -26, 24}, // L (- , Top, Bottom)
		{ 1, -24, 26}, // R
	};

	void print_map()
	{
		for (int i = 0; i < 125; i++)
		{
			Debug.Log(i.ToString() + "th :" + roomMeta[i, 0].ToString() + ", " + roomMeta[i, 1].ToString() + ", " + roomMeta[i, 2].ToString() +", "+ roomMeta[i, 3].ToString());
		}
	}
	public void make_map()
	{
		int try_times = 5;
		do
		{
			try_times--;
			map_init();
			map_rand();
			map_fix();
			if(try_times< 0)
				throw new System.ArgumentException("Trying Making Map is Fail");
		} while (map_evaluation());
	}

	public RoomMeta set_roomMeta_list(List<List<List<RoomMeta>>> RoomMeta_list, RoomMeta prev_roomMeta = null)//List<List<List<RoomMeta>>> RoomMeta_list
	{
		make_map();


		RoomMeta roomMeta_now = null;
		int roomMeta_now_index;
		List<int> rand_magic = new List<int>();

		if (prev_roomMeta != null) {
			roomMeta_now_index = prev_roomMeta.room_index;
		}
		else
		{
			roomMeta_now_index = Random.Range(0, Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX);
		}
		rand_magic.Add(roomMeta_now_index); // [0] 은 init room

		// MagicCircle 4개 생성.
		//int[] rand_magic = new int[4];
		for (int nth = 0; nth < 4; nth++) // [1] 은 magic, [2]~[4]는 well
		{
			while (true)
			{
				int temp = Random.Range(0, Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX);
				if (rand_magic.Contains(temp)) continue;

				rand_magic.Add(temp);
				/*
				bool flag = false;

				for (int ith = 0; ith < nth; ith++)
				{
					if (temp == rand_magic[ith])
					{
						flag = true;
						break;
					}
				}
				if (flag) continue;
				rand_magic[nth] = temp;
				*/
				break;
			}
		}

		
		/*

		// 오름차순 정렬.
		for (int i = 0; i < rand_magic.Length; i++)
		{
			for (int j = 0; j < i; j++)
			{
				if (rand_magic[j] > rand_magic[i])
				{
					int temp = rand_magic[j];
					rand_magic[j] = rand_magic[i];
					rand_magic[i] = temp;
				}
			}
		}
		
		*/ 
		// map RoomMeta_list에 입력.
		for (int i = 0; i < Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX; i++)
		{
			Vector3Int coord = flat2coord(i);
			
			RoomMeta_list[coord[0]][coord[1]][coord[2]].room_index = i;

			// roomType setting
			if (i == rand_magic[0])  // init room
			{
				RoomMeta_list[coord[0]][coord[1]][coord[2]].roomType = RoomType.INITIAL;
				roomMeta_now = RoomMeta_list[coord[0]][coord[1]][coord[2]];
			}
			else if (i == rand_magic[1]) {
				RoomMeta_list[coord[0]][coord[1]][coord[2]].roomType = RoomType.MAGICCIRCLE;
			}
			else if (i == rand_magic[2] || i == rand_magic[3] || i == rand_magic[4])
			{
				RoomMeta_list[coord[0]][coord[1]][coord[2]].roomType = RoomType.WELL;
			}
			else
			{
				RoomMeta_list[coord[0]][coord[1]][coord[2]].roomType = RoomType.NORMAL;
			}

			roomtype_setting(RoomMeta_list[coord[0]][coord[1]][coord[2]]);
			// room 연결 
			for (int j = 0; j < 4; j++)
			{
				if (roomMeta[i, j] == -1 || roomMeta[i, j] == -2) // 문없음, 가장자리 부분
				{
					RoomMeta_list[coord[0]][coord[1]][coord[2]].UDLR_room[j] = null;
					continue;
				}

				try
				{
					Vector3Int opposite_coord = flat2coord(roomMeta[i, j]);

					RoomMeta_list[coord[0]][coord[1]][coord[2]].UDLR_room[j] =
						RoomMeta_list[opposite_coord[0]][opposite_coord[1]][opposite_coord[2]];
				}
				catch (System.ArgumentOutOfRangeException e)
				{
					Debug.Log("EEEEEERRRRRRRRRRRRRRROOOOOOORRRRRR");
					Debug.Log(e.Message);
					Debug.Log(coord);
					Debug.Log("i = " + i.ToString() + ", j = " + j.ToString());
					Debug.Log(flat2coord(roomMeta[i, j]));
				}
			}

		}   
		
		// location index -> 
			//             0,0,0/ 0,0,1/ 0,0,2/...
			//            0,1,0/
			//           0,2,0/
			//             1,0,0/ 1,0,1/ 1,0,2/...
			//            1,1,0/
			//           1,2,0/
			//

		///// MAP adjacent room connect /////
		for (int x = 0; x < Constant.MAX_INDEX; x++) // 같은 층 
		{
			List<RoomMeta> unvisited = new List<RoomMeta>();

			for (int y = 0; y < Constant.MAX_INDEX; y++)
			{
				for (int z = 0; z < Constant.MAX_INDEX; z++)
				{
					unvisited.Add(RoomMeta_list[x][y][z]);
				}
			}

			for (int idx = 0, len = unvisited.Count; idx < len; idx++)
			{
				List<RoomMeta> adjacent = new List<RoomMeta>();

				adjacent_add(unvisited[idx], adjacent, unvisited);

				foreach (var roomMeta in adjacent)
				{
					roomMeta.adjacent_room = adjacent;
				}

				idx--;
				len = unvisited.Count;
			}	
		}

		return roomMeta_now;
	}

	void adjacent_add(RoomMeta roomMeta, List<RoomMeta> adjacent, List<RoomMeta> unvisited)
	{
		if (!unvisited.Contains(roomMeta)) return; // 방문했었다면 return

		unvisited.Remove(roomMeta); // 방문.
		adjacent.Add(roomMeta); // 연결.

		for (int UDLR = 0; UDLR < 4; UDLR++)
		{
			if (roomMeta.UDLR_room[UDLR]==null || roomMeta.UDLR_room[UDLR].location.x != roomMeta.location.x) continue;// 없거나 같은층아니면

			adjacent_add(roomMeta.UDLR_room[UDLR], adjacent, unvisited);
		}
	}


	Vector3Int flat2coord(int flat)
	{
		Vector3Int coord = new Vector3Int();
		coord[0] = flat / (Constant.MAX_INDEX * Constant.MAX_INDEX); // 높이 z 
		coord[1] = (flat - coord[0] * Constant.MAX_INDEX * Constant.MAX_INDEX) / (Constant.MAX_INDEX);  // _5 x
		coord[2] = flat % (Constant.MAX_INDEX); // _1 y
		 
		return coord;
	}


	void map_init()
	{
		for (int i = 0; i < Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				roomMeta[i, j] = -1;
			}
		}
	}

	bool is_corner(int room_index, int UDLR)
	{
		if (UDLR == 0)
		{
			if (room_index % 25 < 5) return true;
		}
		if (UDLR == 1)
		{
			if (room_index % 25 >= 20) return true;
		}
		if (UDLR == 2)
		{
			if (room_index % 5 == 0) return true;
		}
		if (UDLR == 3)
		{
			if (room_index % 5 == 4) return true;
		}

		return false;
	}

	void map_rand()
	{
		int rand;
		for (int i = 0; i < Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX; i++)
		{
			int toIndex;
			while (roomMeta[i, 0] + roomMeta[i, 1] + roomMeta[i, 2] + roomMeta[i, 3] == -4) // 통로는 하나라도 있어야함.
			{
				for (int j = 0; j < 4; j++)
				{
					rand = -1;
					while (rand != 3) //  rr -> 3: 문 없음, 0: 같은 층 문, 1: 윗 계단, 2: 아랫 계단, 
					{
						rand = Random.Range(0, 6);
						if (rand > 3) rand = 0;

						if (is_corner(i, j))
						{
							roomMeta[i, j] = -2; // 있으면안댐;
							rand = 3;
						}

						if (rand == 3) break; // 통로 없음

						toIndex = i + xy_z[j, rand];

						if (is_not_valid(toIndex)) continue;
						if (roomMeta[toIndex, opposite(j)] != -1) continue;

						roomMeta[toIndex, opposite(j)] = i;
						roomMeta[i, j] = toIndex;

						rand = 3;
						break;
					}
				}
			}
		}


	}

	bool is_not_valid(int value)
	{
		if (value < 0 || value >= Constant.MAX_INDEX * Constant.MAX_INDEX * Constant.MAX_INDEX) return true;
		return false;
	}

	void eval()
	{
		for (int i = 0; i < 125; i++)
		{
			if (visited[i] != 1)
			{

				Debug.Log(i);

				for (int j = 0; j < 4; j++)
				{
					if (roomMeta[i, j] == -1)
					{
						for (int z = 0; z < 3; z++)
						{
							int toIndex = i + xy_z[j, z];
							if (is_not_valid(toIndex)) continue;

							Debug.Log(i.ToString() + "'s opposite"+ toIndex.ToString() + "'s j way is " + roomMeta[toIndex, opposite(j)].ToString());
						}
					}
				}

			}
		}

		Debug.Log("total Team : " + nth_group.ToString());
	}





	void visited_init()
	{
		visit_idx = 0;
		nth_group = 0;

		for (int i = 0; i < 125; i++)
		{
			visited[i] = 0;
		}
	}

	void map_visit()
	{
		visited_init();

		for (int i = 0; i < 125; i++)
		{
			visit_idx = i;
			if (visited[i] == 0)
			{
				nth_group += 1;
				map_explore();
			}
		}
	}
	void map_fix()
	{
		map_visit();

		for (int i = nth_group; i > 0; i--)
		{
			map_search(i);
		}
	}

	void map_explore() // map grouping
	{
		visited[visit_idx] = nth_group;
		for (int j = 0; j < 4; j++) // 4 방향 연결된 방을 탐색
		{
			if (roomMeta[visit_idx, j] == -1) continue;
			if (roomMeta[visit_idx, j] == -2) continue; // 방 연결이 안되어있으면 continue

			if (visited[roomMeta[visit_idx, j]] != 0) continue; // 이미 방문한 room 이면 continue

			int temp = visit_idx;
			visit_idx = roomMeta[temp, j];
			map_explore();
			visit_idx = temp;
		}
	}

	void map_search(int search_group)
	{

		for (int i = 0; i < 125; i++)
		{
			if (visited[i] == search_group)
			{
				for (int j = 0; j < 4; j++) //방향
				{
					for (int k = 0; k < 3; k++) // 고도 
					{
						if (roomMeta[i, j] != -1) continue; // 연결 되어있으면 pass

						int toIndex = i + xy_z[j, k];
						if (is_not_valid(toIndex)) continue;

						if (roomMeta[toIndex, opposite(j)] == -1 && visited[toIndex] != search_group) // 반대쪽도 연결안되어있고, 같은 그룹이 아니면 연결.
						{
							roomMeta[toIndex, opposite(j)] = i; // connecting
							roomMeta[i, j] = toIndex;

							combine_groups(search_group, visited[toIndex]);


							return;
						}

					}
				}
			}
		}
	}

	void combine_groups(int group_a, int group_b) // a -> b grouping
	{
		for (int i = 0; i < 125; i++)
		{
			if (visited[i] == group_a) visited[i] = group_b;
		}
	}

	bool map_evaluation()
	{
		map_visit();
		if (nth_group == 1) return false;
		return true;
	}

	
	int opposite(int xy)
	{
		if (xy % 2 == 0)
		{
			return xy + 1;
		}
		else
		{
			return xy - 1;
		}
	}

	public void roomtype_setting(RoomMeta roomMeta)
	{
		switch (roomMeta.roomType) {

			case RoomType.INITIAL:
				break;
			case RoomType.NORMAL:	
				break;
			case RoomType.WELL:
				roomMeta.room_locked = true;
				break;
			case RoomType.MAGICCIRCLE:
				roomMeta.room_locked = true;
				break;
			default:
				break;
		}
	}

    public RoomMeta setmap(int stagelv)
    {
        List<List<List<RoomMeta>>> roommetas = GameManager.Instance.RoomMeta_list;
        List<RoomMeta> normal_list = new List<RoomMeta>();

        int Num_floor = 3;
        if (stagelv == 1) Num_floor = 2;

        for(int i = 0; i < Num_floor; i++)
        {
            set_normal_room(check_available_rooms(i, roommetas), normal_list);
        }
        set_door(normal_list, roommetas);
        set_stair(normal_list, roommetas);
        set_well_room(normal_list);
        set_workshop_room(normal_list);
        /*
        RoomMeta roomMeta = roommetas[0][Random.Range(0, Constant.MAX_INDEX)][Random.Range(0, Constant.MAX_INDEX)];
        roomMeta.roomType = RoomType.INITIAL;*/
        return set_init_room(normal_list);
    }
    List<RoomMeta> check_available_rooms(int floor, List<List<List<RoomMeta>>> roommetas)
    {
        List<RoomMeta> available_list = new List<RoomMeta>();
        if(floor == 0)
        {
            while(available_list.Count < 13)
            {
                RoomMeta _roomMeta = roommetas[0][Random.Range(0, 5)][Random.Range(0, 5)];
                bool canadd = true;
                for(int i = 0; i < available_list.Count; i++)
                {
                    if (available_list[i].location == _roomMeta.location)
                    {
                        canadd = false;
                        break;
                    }
                }
                if (canadd) available_list.Add(_roomMeta);
            }
            //for (int i = 0; i < available_list.Count; i++) Debug.Log(available_list[i].location);
            return available_list;
        }
        else
        {
            for (int i = 0; i < Constant.MAX_INDEX; i++)
            {
                for (int j = 0; j < Constant.MAX_INDEX; j++)
                {
                    if (roommetas[floor - 1][i][j].roomType == RoomType.None)
                    {
                        available_list.Add(roommetas[floor][i][j]);
                    }
                }
            }
        }
        //for (int i = 0; i < available_list.Count; i++) Debug.Log(available_list[i].location);
        return available_list;
    }
    void set_normal_room(List<RoomMeta> available_list, List<RoomMeta> normal_list)
    {
        List<RoomMeta> normal_list_now = new List<RoomMeta>();
        while(normal_list_now.Count < 12)
        {
            RoomMeta roomMeta = available_list[Random.Range(0, available_list.Count)];
            if (roomMeta.roomType == RoomType.None)
            {
                roomMeta.roomType = RoomType.NORMAL;
                normal_list_now.Add(roomMeta);
                normal_list.Add(roomMeta);
            }
        }
        //RoomMeta _roomMeta = available_list[Random.Range(0, available_list.Count)];
        //_roomMeta.roomType = RoomType.None;
    }
    
    RoomMeta set_init_room(List<RoomMeta> normal_list)
    {
        int index = Random.Range(0, normal_list.Count);
        if (normal_list[index].roomType == RoomType.NORMAL) normal_list[index].roomType = RoomType.INITIAL;

        RoomMeta initroom = normal_list[index];

        normal_list.RemoveAt(index);

        return initroom;
        ////여기해
    }

    void set_well_room(List<RoomMeta> normal_list)
    {
        int index = Random.Range(0, normal_list.Count);
        if (normal_list[index].roomType == RoomType.NORMAL) normal_list[index].roomType = RoomType.MAGICCIRCLE;
        normal_list[index].room_locked = true;
        normal_list.RemoveAt(index);
        for (int i = 0; i < 3; i++)
        {
            index = Random.Range(0, normal_list.Count);
            if (normal_list[index].roomType == RoomType.NORMAL) normal_list[index].roomType = RoomType.WELL;

            normal_list.RemoveAt(index);
        }
        
        ////여기해
    }
    void set_workshop_room(List<RoomMeta> normal_list)
    {
        for(int i = 0; i < GameManager.Instance.stagelv; i++)
        {
            int index = Random.Range(0, normal_list.Count);
            if (normal_list[index].roomType == RoomType.NORMAL) normal_list[index].roomType = RoomType.WORKSHOP;
            normal_list.RemoveAt(index);
        }
    }
    void set_door(List<RoomMeta> room_list, List<List<List<RoomMeta>>> roommetas)
    {
        // y 가 -면 U +면 D
        // z 가 + 면 R -면 L
        for (int i = 0; i < room_list.Count; i++)
        {            
            Vector3 location = room_list[i].location;
            if (location.y > 0) //U
            {
                if (roommetas[(int)location.x][(int)location.y - 1][(int)location.z].roomType != RoomType.None)
                {
                    room_list[i].UDLR_room[0] = roommetas[(int)location.x][(int)location.y - 1][(int)location.z];
                }
            }
            if (location.y < 4) //D
            {
                if(roommetas[(int)location.x][(int)location.y + 1][(int)location.z].roomType != RoomType.None)
                {
                    room_list[i].UDLR_room[1] = roommetas[(int)location.x][(int)location.y + 1][(int)location.z];
                } 
            }            
            if (location.z > 0) //L
            {
                if (roommetas[(int)location.x][(int)location.y][(int)location.z - 1].roomType != RoomType.None)
                {
                    room_list[i].UDLR_room[2] = roommetas[(int)location.x][(int)location.y][(int)location.z - 1];
                }
            }
            if (location.z < 4) //R
            {
                if (roommetas[(int)location.x][(int)location.y][(int)location.z + 1].roomType != RoomType.None)
                {
                    room_list[i].UDLR_room[3] = roommetas[(int)location.x][(int)location.y][(int)location.z + 1];
                }
            }
            
            //debug below
            /*
            for(int k = 0; k < room_list[i].UDLR_room.Length; k++)
            {
                if(room_list[i].UDLR_room[k] != null)
                    Debug.Log(room_list[i].UDLR_room[k].roomType);
            }*/
        }
        
        
    }
    void set_stair(List<RoomMeta> room_list, List<List<List<RoomMeta>>> roommetas)
    {
        int iter = 0;
        while (true)
        {
            iter++;
            if(iter > 5)
            {
                break;
            }
            List<List<RoomMeta>> linked_room_list_new = get_linkedroom(room_list);
            for (int i = 0; i < linked_room_list_new.Count; i++)
            {
                find_adjacency_and_link(linked_room_list_new[i], roommetas);
            }

            if (linked_room_list_new.Count == 1)
            {
                break;
            } 
        }
        

        for (int i = 0; i < room_list.Count; i++)
        {
            Vector3 location = room_list[i].location;
            if(location.x > 0 && location.x < 4 && location.y > 0 && location.y < 4)
            {
                for(int doorindex = 0; doorindex < 4; doorindex++)
                {
                    if(room_list[i].UDLR_room[doorindex] != null)
                    {
                        switch (doorindex)
                        {
                            case 0://UP
                                break;
                            case 1://DOWN
                                break;
                            case 2://LEFT
                                break;
                            case 3://RIGHT
                                break;
                        }
                    }
                }
                //Up
                //Down
                //Left
                //Right
            }
        }
    }
    List<List<RoomMeta>> get_linkedroom(List<RoomMeta> room_list)
    {
        List<RoomMeta> copy_room_list = new List<RoomMeta>();
        for (int i = 0; i < room_list.Count; i++) copy_room_list.Add(room_list[i]);
        List<List<RoomMeta>> lists = new List<List<RoomMeta>>();

        while(copy_room_list.Count > 0)
        {
            List<RoomMeta> new_list = new List<RoomMeta>();
            find_linkedroom(new_list, copy_room_list[0], copy_room_list);

            lists.Add(new_list);
        }
        return lists;
    }

    void find_linkedroom(List<RoomMeta> linkedroomlist, RoomMeta roomMetanow, List<RoomMeta> sourcelist)
    {
        linkedroomlist.Add(roomMetanow);
        for(int i = 0; i < sourcelist.Count; i++)
        {
            if (sourcelist[i].GetHashCode() == roomMetanow.GetHashCode())
            {
                sourcelist.RemoveAt(i);
                break;
            }
        }
        for(int i = 0; i < roomMetanow.UDLR_room.Length; i++)
        {
            if(roomMetanow.UDLR_room[i] != null && !check_containsornot(linkedroomlist, roomMetanow.UDLR_room[i]))
            {
                find_linkedroom(linkedroomlist, roomMetanow.UDLR_room[i], sourcelist);
            }
        }
    }//링크된방을 다찾으니까 계단을 잇기 전에 사용하는게 권장됨.


    void find_adjacency_and_link(List<RoomMeta> room_lumb, List<List<List<RoomMeta>>> roommetas)
    {        
        List<near_Room> near_Rooms_Upstair = new List<near_Room>();
        List<near_Room> near_Rooms_Downstair = new List<near_Room>();
        
        for (int i = 0; i < room_lumb.Count; i++)
        {
            Vector3 location = room_lumb[i].location;
            for(int k = 0; k < 4; k++)
            {
                if (room_lumb[i].UDLR_room[k] == null)
                {                    
                    switch (k)
                    {
                        case 0://U
                            
                            if (location.y <= 0) break;
                            if(location.x > 0) //즉, 맨 위층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 윗층인 동시에 북쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x - 1][(int)location.y - 1][(int)location.z].roomType != RoomType.None &&
                                    roommetas[(int)location.x - 1][(int)location.y - 1][(int)location.z].UDLR_room[1] == null)//북쪽방이니까 해당방은 남쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Upstair.Add(new near_Room(1, roommetas[(int)location.x - 1][(int)location.y - 1][(int)location.z], room_lumb[i]));
                                }                                
                            }
                            if (location.x < 4) //즉, 맨 아래층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 아래층인 동시에 북쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x + 1][(int)location.y - 1][(int)location.z].roomType != RoomType.None &&
                                    roommetas[(int)location.x + 1][(int)location.y - 1][(int)location.z].UDLR_room[1] == null)//북쪽방이니까 해당방은 남쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Downstair.Add(new near_Room(1, roommetas[(int)location.x + 1][(int)location.y - 1][(int)location.z], room_lumb[i]));
                                }
                            }
                            break;
                        case 1://D
                            if (location.y >= 4) break;
                            if (location.x > 0) //즉, 맨 위층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 윗층인 동시에 남쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x - 1][(int)location.y + 1][(int)location.z].roomType != RoomType.None &&
                                    roommetas[(int)location.x - 1][(int)location.y + 1][(int)location.z].UDLR_room[0] == null)//남쪽방이니까 해당방은 북쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Upstair.Add(new near_Room(0, roommetas[(int)location.x - 1][(int)location.y + 1][(int)location.z], room_lumb[i]));
                                }
                            }
                            if (location.x < 4) //즉, 맨 아래층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 아래층인 동시에 남쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x + 1][(int)location.y + 1][(int)location.z].roomType != RoomType.None &&
                                    roommetas[(int)location.x + 1][(int)location.y + 1][(int)location.z].UDLR_room[0] == null)//남쪽방이니까 해당방은 북쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Downstair.Add(new near_Room(0, roommetas[(int)location.x + 1][(int)location.y + 1][(int)location.z], room_lumb[i]));
                                }
                            }
                            break;
                        case 2://L
                            if (location.z <= 0) break;
                            if (location.x > 0) //즉, 맨 위층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 윗층인 동시에 서쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x - 1][(int)location.y][(int)location.z - 1].roomType != RoomType.None &&
                                    roommetas[(int)location.x - 1][(int)location.y][(int)location.z - 1].UDLR_room[3] == null)//서쪽방이니까 해당방은 동쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Upstair.Add(new near_Room(3, roommetas[(int)location.x - 1][(int)location.y][(int)location.z - 1], room_lumb[i]));
                                }
                            }
                            if (location.x < 4) //즉, 맨 아래층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 아래층인 동시에 서쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x + 1][(int)location.y][(int)location.z - 1].roomType != RoomType.None &&
                                    roommetas[(int)location.x + 1][(int)location.y][(int)location.z - 1].UDLR_room[3] == null)//서쪽방이니까 해당방은 동쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Downstair.Add(new near_Room(3, roommetas[(int)location.x + 1][(int)location.y][(int)location.z - 1], room_lumb[i]));
                                }
                            }
                            break;
                        case 3://R
                            if (location.z >= 4) break;
                            if (location.x > 0) //즉, 맨 위층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 윗층인 동시에 동쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x - 1][(int)location.y][(int)location.z + 1].roomType != RoomType.None &&
                                    roommetas[(int)location.x - 1][(int)location.y][(int)location.z + 1].UDLR_room[2] == null)//동쪽방이니까 해당방은 서쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Upstair.Add(new near_Room(2, roommetas[(int)location.x - 1][(int)location.y][(int)location.z + 1], room_lumb[i]));
                                }
                            }
                            if (location.x < 4) //즉, 맨 아래층은 아니란소리. 맨윗층이 x == 0
                            {
                                //여기는 나보다 아래층인 동시에 동쪽방을 살펴보겠다는뜻.
                                if (roommetas[(int)location.x + 1][(int)location.y][(int)location.z + 1].roomType != RoomType.None &&
                                    roommetas[(int)location.x + 1][(int)location.y][(int)location.z + 1].UDLR_room[2] == null)//동쪽방이니까 해당방은 서쪽이 null이어야 하겟지?
                                {
                                    near_Rooms_Downstair.Add(new near_Room(2, roommetas[(int)location.x + 1][(int)location.y][(int)location.z + 1], room_lumb[i]));
                                }
                            }
                            break;
                    }
                }
            }            
        }
        //make link
        near_Room _near_Room;
        if (near_Rooms_Upstair.Count > 0)
        {
            _near_Room = near_Rooms_Upstair[Random.Range(0, near_Rooms_Upstair.Count)];
            _near_Room.roomMeta.UDLR_room[_near_Room.UDLR] = _near_Room.roomMetaToConnect;
            switch (_near_Room.UDLR)
            {
                case 0:
                    _near_Room.roomMetaToConnect.UDLR_room[1] = _near_Room.roomMeta;
                    break;
                case 1:
                    _near_Room.roomMetaToConnect.UDLR_room[0] = _near_Room.roomMeta;
                    break;
                case 2:
                    _near_Room.roomMetaToConnect.UDLR_room[3] = _near_Room.roomMeta;
                    break;
                case 3:
                    _near_Room.roomMetaToConnect.UDLR_room[2] = _near_Room.roomMeta;
                    break;
            }
        }
        if(near_Rooms_Downstair.Count > 0)
        {
            _near_Room = near_Rooms_Downstair[Random.Range(0, near_Rooms_Downstair.Count)];
            _near_Room.roomMeta.UDLR_room[_near_Room.UDLR] = _near_Room.roomMetaToConnect;
            switch (_near_Room.UDLR)
            {
                case 0:
                    _near_Room.roomMetaToConnect.UDLR_room[1] = _near_Room.roomMeta;
                    break;
                case 1:
                    _near_Room.roomMetaToConnect.UDLR_room[0] = _near_Room.roomMeta;
                    break;
                case 2:
                    _near_Room.roomMetaToConnect.UDLR_room[3] = _near_Room.roomMeta;
                    break;
                case 3:
                    _near_Room.roomMetaToConnect.UDLR_room[2] = _near_Room.roomMeta;
                    break;
            }
        }

    }

    bool check_UDRooms(int UD)
    {
        bool check = true;
        return check;
    }


    bool check_containsornot(List<RoomMeta> linkedroomlist ,RoomMeta roomMeta_to_check)
    {
        for(int i = 0; i < linkedroomlist.Count; i++)
        {
            if(linkedroomlist[i].GetHashCode() == roomMeta_to_check.GetHashCode())
            {
                return true;
            }
        }
        return false;
    }
}

public class near_Room
{
    public near_Room(int _UDLR, RoomMeta _roomMeta, RoomMeta _roomMetaToConnect)
    {
        UDLR = _UDLR;
        roomMeta = _roomMeta;
        roomMetaToConnect = _roomMetaToConnect;
    }
    public int UDLR;
    public RoomMeta roomMeta;
    public RoomMeta roomMetaToConnect;
}