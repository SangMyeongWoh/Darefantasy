using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMeta
{
	// 요 두개 필요한가? RoomMeta에 nodelist있음.
	//    public List<ObjectBase> object_base_list = new List<ObjectBase>();
	//    public List<GameObject> object_body_list = new List<GameObject>();

	public List<Sprite> room_thema_list;
	public List<GameObject> room_stain_list;
	public List<List<Node>> node_list;  // 이거사용

	public RoomMeta thisRoomMeta;
	public RoomMeta[] UDLR_room; // 연결없으면 null
	public Node[] UDLR_door;
	public bool room_locked;
	public bool WellON;

    public RoomType roomType;
    public Sprite room_thema_floor;
    public int room_index;
	public Vector3Int location;
	public List<RoomMeta> adjacent_room;

	// location index -> 
	//             0,0,0/ 0,0,1/ 0,0,2/...
	//            0,1,0/
	//           0,2,0/
	//             1,0,0/ 1,0,1/ 1,0,2/...
	//            1,1,0/
	//           1,2,0/
	//
    // y 가 +면 D -면 U
    // z 가 + 면 R -면 L


	public bool instantDone;
	//이 인덱스 인포는 hee02가 편한대로 정하기

	public void init_roomMeta()
	{
		// 여기에 룸메타 초기화하는거 다 적기.

		//roomMeta.object_body_list.Clear();
		init_node();

		room_stain_list = new List<GameObject>();
		room_thema_list = new List<Sprite>();

		UDLR_room = new RoomMeta[4] { null, null, null, null };

		roomType = RoomType.None;
		room_thema_floor = null;
		room_locked = false;
		instantDone = false;
		WellON = false;

	}

	void init_node()
	{
		foreach (var list in node_list)
		{
			foreach (var node in list)
			{
				foreach (var objectbase in node.object_here_list)
				{
					//objectbase.unactive();
					// 여기 base 관련 청소 해야됨. body랑 base..?
					if (objectbase is PlayerBase) continue;
					objectbase.is_alive = false;
				}
				node.object_here_list.Clear();
			}
		}

		foreach (var node in UDLR_door)
		{
			node.object_here_list.Clear();
		}
	}

	public RoomMeta(int x, int y, int z)
	{
		// 생성시 정의하면 변경 없음.
		location = new Vector3Int(x, y, z);
		thisRoomMeta = this;
		

		UDLR_door = new Node[4] {		//// DOOR NODE 생성
		new Node(-1, 2,this), new Node(5, 2,this), new Node(2, -1,this), new Node(2, 5,this)
		}; // 

		node_list = new List<List<Node>>();

		//////////////////////////////

		for (int i = 0; i < Constant.MAX_INDEX; i++) {
			node_list.Add(new List<Node>());
			for (int j = 0; j < Constant.MAX_INDEX; j++)
			{
				node_list[i].Add(new Node(i, j, thisRoomMeta));
			}
		}

		for (int i = 0; i < Constant.MAX_INDEX; i++)
		{
			for (int j = 0; j < Constant.MAX_INDEX; j++)
			{
				// i v j > // Node Linked
				if (i != 0)
					node_list[i][j].UDLR_node[0] = node_list[i - 1][j];

				if (i != Constant.MAX_INDEX - 1)
					node_list[i][j].UDLR_node[1] = node_list[i + 1][j];

				if (j != 0)
					node_list[i][j].UDLR_node[2] = node_list[i][j - 1];

				if (j != Constant.MAX_INDEX - 1)
					node_list[i][j].UDLR_node[3] = node_list[i][j + 1];


				//노드 연결할것

			}
		}

	}
}

