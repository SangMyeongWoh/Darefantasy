using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingManager : MonoBehaviour
{
	public GameManager gameManager;
	public GameObject room;
	public AnimManager animManager;
	public ObjectBase playerbase;
	public Turn_Type thisManagerType;

	public List<MovingObject> moving_list = new List<MovingObject>();
	public int moving_priority = 0;

	public bool running = false; // get key 받아서 running 중.
	public bool room_transition = false; // room transition 일어남.
	public bool continue_turn = false;
	public bool attack_attempt = false;
	public bool move_attempt = false;

	// 움직임에 대한 정의. node transition..
	// room transition 은 player만 가짐..?
	public void Start()
	{
		gameManager = GameManager.Instance;
	}

	public virtual void initManager(AnimManager _animManager)
	{
		animManager = _animManager;
		room = GameManager.Instance.room;
		playerbase = GameManager.Instance.playerBase;

	}

	public void init_player(ObjectBase objectBase)
	{
		playerbase = objectBase;

	}

	public virtual void process()
	{
		if (!running || room_transition) return;
        clear_list();
		set_movinglist();
        for(int i = 0; i < moving_list.Count; i++)
        {
            if(moving_list[i].objectBase.objectBase_type == ObjectBaseType.CHARACTER)
            {
                break;
            }
        }
		move_process();
	}

	public abstract Node get_next_node(ObjectBase objectBase);


	public void clear_list()
	{
		animManager.clear_anim_list();
		//moving_list.Clear();
	}


	public void set_movinglist()
	{
		if (moving_list.Count > 0) return;

		for (int i = 0; i < Constant.MAX_INDEX; i++)
		{
			for (int j = 0; j < Constant.MAX_INDEX; j++)
			{
				for (int k = 0; k < gameManager.roomMeta_now.node_list[i][j].object_here_list.Count; k++)
				{
					if (gameManager.roomMeta_now.node_list[i][j].object_here_list[k].turn_type == thisManagerType ||
						gameManager.roomMeta_now.node_list[i][j].object_here_list[k].turn_type == Turn_Type.Both_turn)
					{
						ObjectBase temp_objectbase = gameManager.roomMeta_now.node_list[i][j].object_here_list[k];
						for (int turn = temp_objectbase.has_turns; turn > 0; turn--)
						{
							moving_list.Add(new MovingObject(temp_objectbase, temp_objectbase.has_turns - turn));
						}
					}
					
				}
			}
		}
	}


	public void move_process()
	{
		for (int i = 0, len = moving_list.Count; i < len; i++)
		{
			if (moving_list[i].priority > moving_priority) continue;
			ObjectBase objectBase = moving_list[i].objectBase;
			if (objectBase.is_alive == false) continue;
			continue_turn = false;
			running = true;

			Node next_node = get_next_node(objectBase);

			attempt_attack(objectBase, next_node); // true 면 공격 성공, false면 공격 X
            if(objectBase.movable) attempt_move(objectBase, next_node);
            search_here(objectBase); // 이동 했으면 그자리 탐색.

			// 무빙리스트에서 삭제			
			moving_list.RemoveAt(i);
			i--;
			len--;
		}

		if (continue_turn || moving_list.Count > 0) process_continue();
		else process_done();
	}

	public virtual void process_continue()
	{
		if (moving_list.Count > 0) moving_priority++;
        
		//new WaitForSeconds(Constant.DELAY_TIME);
	}
	public virtual void process_done()	{
		moving_priority = 0;
        //new WaitForSeconds(Constant.DELAY_TIME);
    }
    public virtual void set_itemeffectlist(ObjectBase objectBase)
    {
        if (objectBase is PlayerBase)
        {

        }
    }

	public void player_turn()
	{
		gameManager.anim_playing = true;
		gameManager.player_turn = true;
        gameManager.enemy_turn = false;
        gameManager.object_turn = false;
		running = false;
	}
	public void enemy_turn()
	{
		gameManager.anim_playing = true;
		gameManager.player_turn = false;
        gameManager.enemy_turn = true;
        gameManager.object_turn = false;
        running = false;
	}
    public void object_turn()
    {
        gameManager.anim_playing = true;
        gameManager.player_turn = false;
        gameManager.enemy_turn = false;
        gameManager.object_turn = true;
        running = false;
    }


	public abstract void search_here(ObjectBase objectBase);
	public abstract bool interaction(ObjectBase objectBase, ObjectBase targetBase); 
	


	//move_to_node(objectBase, next_node);
	// ojbectbase에 지가 있는 Node를 가지고 있었으면 좋겠음.


	//스페이스바 입력했을 경우 처리
	//스페이스바 눌르면 제자리 고정, 내자리에 있는 아이템 먹기

	//진행방향에 ObjectBase가 있을 경우 타입과 상관없이 interaction함수 호출.
	
	protected void attempt_attack(ObjectBase objectBase, Node next_node)
    {       
        attack_attempt = false;
		if (next_node == null)	return;

		foreach (var targetBase in next_node.object_here_list)
        {
            if (objectBase.is_target(targetBase))
            {
				attack_attempt = attack_and_hit(objectBase, targetBase) || attack_attempt;
                if(!objectBase.can_splash) break; //공격을 하나만.Q
            }
            //objectBase.interaction(targetBase);
        }
		
        //이동인지 공격인지 정했어. 애님큐에 이동이나 공격에 대한 애니메이션 클래스를 넣어줘야돼.
    }

	protected void attempt_move(ObjectBase objectBase, Node next_node)
	{
        
		move_attempt = false;
		if (next_node == null) return;
		if (attack_attempt) return; // 공격 성공했으면 건너뜀.

        foreach (var targetBase in next_node.object_here_list)
        {
            if (targetBase == objectBase) continue;
			if (targetBase.is_collide)
			{
				if( objectBase is PlayerBase)
					continue_turn = true;
				return;
			}
        }
		objectBase.move(next_node);
		move_attempt = true;
		// 이동 완료.
	}    

	bool attack_and_hit(ObjectBase objectBase, ObjectBase targetBase)
	{
        int[] damages = objectBase.attack(targetBase);
		bool flag = targetBase.hit(damages);

		return flag; // attack & hit 이 이루어졌으면 True 
	}
}


public class MovingObject
{
	public ObjectBase objectBase;
	public int priority;

	public MovingObject(ObjectBase _objectBase, int _priority)
	{
		objectBase = _objectBase;
		priority = _priority;
	}
}

