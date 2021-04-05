using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingManager
{
	public Key key = Key.None;
	public DoorBase door_transition = null;
    public bool cannotmove;
    public GameObject unableCheck;
    public GameObject ableCheck;

	public override void initManager(AnimManager _animManager)
	{
		thisManagerType = Turn_Type.Player_turn;
		base.initManager(_animManager);
	}

	public override void process()
	{
        if (running || !gameManager.player_turn || gameManager.anim_playing || cannotmove)
        {
            if (!unableCheck.activeSelf) {
                unableCheck.SetActive(true);
                ableCheck.SetActive(false);
            }
            
            return;
        }
        if (unableCheck.activeSelf) {
            ableCheck.SetActive(true);
            unableCheck.SetActive(false);
        } 
        door_process();
		get_key_input();
        
		base.process();
		room_process();
	}
	

	public override void process_continue()
	{
		extra_process();
		player_turn();
		base.process_continue();
		room_transition = false;
		
	}


	public void extra_process()
	{
		if (door_transition != null)
		{
            switch (key)
            {
                case Key.W:
                    gameManager.sub_managers.cameraManager.run_camera_anim(CameraAnimType.ROOMTRANSITION_SOFT, fromvector: new Vector3(0,-20,0), tovector: new Vector3(0, 20, 0));
                    break;
                case Key.A:
                    gameManager.sub_managers.cameraManager.run_camera_anim(CameraAnimType.ROOMTRANSITION_SOFT, fromvector: new Vector3(20, 0, 0), tovector: new Vector3(-20, 0, 0));
                    break;
                case Key.S:
                    gameManager.sub_managers.cameraManager.run_camera_anim(CameraAnimType.ROOMTRANSITION_SOFT, fromvector: new Vector3(0, 20, 0), tovector: new Vector3(0, -20, 0));                    
                    break;
                case Key.D:
                    gameManager.sub_managers.cameraManager.run_camera_anim(CameraAnimType.ROOMTRANSITION_SOFT, fromvector: new Vector3(-20, 0, 0), tovector: new Vector3(20, 0, 0));
                    break;

            }
			//camera
			//gameManager.sub_managers.cameraManager.run_camera_anim(CameraAnimType.ROOMTRANSITION_SOFT, fromvector: from_pos, tovector: to_pos);
		}
	}

	public override void process_done()
	{
		key = Key.None;
		enemy_turn();
		base.process_done();
		room_transition = false;
	}


	


	private void get_key_input()
	{
        /*
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (gameManager.skipmode) gameManager.skipmode = false;
			else gameManager.skipmode = true;
		
			Debug.Log("SKIPMODE : " + gameManager.skipmode.ToString());
		}*/

		if (Input.GetKeyDown(KeyCode.Space))
		{
			key = Key.Space;

			running = true;
			// move_process();
			return;
		}

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				key = Key.W;
			}
			else if (Input.GetKeyDown(KeyCode.A))
			{
				key = Key.A;
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				key = Key.S;
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				key = Key.D;
			}

			running = true;
			// move_process();

			return;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				key = Key.Up;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				key = Key.Down;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				key = Key.Left;
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				key = Key.Right;
			}


			running = true;
			room_transition = true;
			// room_process();

			//room_transition(); // Game Manager?? BoardManager??

			return;
		}
	}

	public override Node get_next_node(ObjectBase objectBase)
	{
		Node next_node = objectBase.node_now;
		switch (key) {
			case Key.W:
				next_node = next_node.UDLR_node[0];
				break;
			case Key.S:
				next_node = next_node.UDLR_node[1];
				break;
			case Key.A:
				next_node = next_node.UDLR_node[2];
				break;
			case Key.D:
				next_node = next_node.UDLR_node[3];
				break;
			case Key.Space:
				break;
			default:
				break;
		}
		if (next_node == null) return null;

		return next_node;
	}

	#region search_here & interaction
	public override void search_here(ObjectBase objectBase) // 현위치 interaction 
	{
		if (attack_attempt || !move_attempt) // 공격성공시 
		{
			return; // 이동실패시 건너뜀.
		}
		//Debug.Log("here object count: " + objectBase.node_now.object_here_list.Count.ToString());
		for (int i = 0, len = objectBase.node_now.object_here_list.Count; i < len; i++) // 추가로 생성된거는 탐색안함.
		{

			if (i >= objectBase.node_now.object_here_list.Count) break;
			if (objectBase.node_now.object_here_list[i] == objectBase) continue;

			if (interaction(playerbase, objectBase.node_now.object_here_list[i])) // object list에서 빠졌으면.
			{
				len--;
				i--;
			}

			if (continue_turn) break;
		}
	}

	public override bool interaction(ObjectBase objectBase, ObjectBase targetBase) // 삭제시 true;
	{
		if (targetBase.is_alive == false)
		{
			Debug.Log("THIS SELFFLASE");
			return false;
		}

		if (targetBase is DoorBase)
		{
			//door_process((DoorBase)targetBase);
			door_transition = (DoorBase)targetBase;
			continue_turn = true;
			return false;
		}
		else if (targetBase is ItemBase)
		{
			if (targetBase.interaction(objectBase)) return true; // 삭제 됐으면?////
			else return false;
		}

		else
		{
			targetBase.interaction(objectBase);
			return false;
		}        
	}

	#endregion

	#region room_process
	private RoomMeta get_next_room()
	{
		RoomMeta next_room = gameManager.roomMeta_now;

		//next_room = next_room.UDLR_room[(int)key];
		switch (key)
		{
			case Key.Up:
				next_room = next_room.UDLR_room[0];
				break;
			case Key.Down:
				next_room = next_room.UDLR_room[1];
				break;
			case Key.Left:
				next_room = next_room.UDLR_room[2];
				break;
			case Key.Right:
				next_room = next_room.UDLR_room[3];
				break;
			default:
				next_room = null;
				break;

		}
		return next_room;
	}

	public void room_process()
	{
		if (!room_transition) return;
        continue_turn = false;
		RoomMeta next_room = get_next_room();

		// 자연스러운 변화 필요. 
		if (next_room == null || next_room.room_locked)
		{
			process_continue();
			return;
		}

		playerbase.unactive();
		gameManager.sub_managers.boardManager.room_transition(next_room);
		playerbase.node_change(node_after_room_transition(next_room));
		playerbase.active();
		//animManager.anim_add(playerbase, AnimType.LANDING, 0, 0);
		room_landing(next_room);
		//camera
		gameManager.sub_managers.cameraManager.run_camera_anim(CameraAnimType.ROOMTRANSITION_HARD);

		//process_continue(); // 노턴소모
		process_done(); // 턴소모
		return;
	}

	public void room_landing(RoomMeta roommeta)
	{
		foreach (var nodelist in roommeta.node_list)
		{
			foreach (var node in nodelist)
			{
				foreach (var objectbase in node.object_here_list)
				{
					if (objectbase is ItemBase) continue;
					if (objectbase is MonsterBase) continue;
					if (objectbase is BlockBase) continue;
					if (objectbase is TrapBase) continue;
					animManager.anim_add(objectbase, AnimType.LANDING, 0, 0);
				}
			}
		}
	}

	public void door_process()
	{
		////////mark
		///
		if (door_transition == null) return;

		gameManager.player_turn = false;

		playerbase.unactive();
		gameManager.sub_managers.boardManager.room_transition(door_transition.connected_roommeta);
		playerbase.node_change(get_next_node(playerbase));
		Vector3 from_pos = playerbase.node_now.position;
		playerbase.ObjectBody.transform.position = playerbase.node_now.position;

		//playerbase.node_change(get_next_node(playerbase));
		playerbase.move(get_next_node(playerbase));
		Vector3 to_pos = playerbase.node_now.position;

        playerbase.ObjectBody.SetActive(true);

		
		//int _UDLR = direction(from_node, to_node);
		//animManager.run_animation_withoutbase(playerbase.ObjectBody, AnimType.MOVE, _UDLR, from_node.position, to_node.position);

		door_transition = null;
		process_continue();
	}
	
	private Node node_after_room_transition(RoomMeta next_room) {
		switch (key)
		{
			case Key.Up:
				return next_room.node_list[4][2];
			case Key.Down:
				return next_room.node_list[0][2];
			case Key.Left:
				return next_room.node_list[2][4];
			case Key.Right:
				return next_room.node_list[2][0];
			default:
				return null;
		}
	}

	#endregion


	

}
