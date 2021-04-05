using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager
{
	public AnimManager(AnimCoroutinePool _animCoroutinePool)
	{
		animCoroutinePool = _animCoroutinePool;
	}
	public AnimCoroutinePool animCoroutinePool;


	List<AnimObject> master_animObjects = new List<AnimObject>();
	List<AnimObject> sub_animObjects = new List<AnimObject>();
    public int running_list = 0;

    //

	public bool flag_master = true;
	public bool flag_sub = true;
	public bool flag_run = true;
	public int order = 0;    

	public void process()
	{
		if (!GameManager.Instance.anim_playing) return;        
		check_process();
		arrange_list();
	}
	public void check_process()
	{
		//Debug.Log("MASTER: " + master_animObjects.Count.ToString());
		//Debug.Log("SUB: " + sub_animObjects.Count.ToString());

		if (master_animObjects.Count == 0) flag_master = false;
		else flag_master = true;

		if (sub_animObjects.Count == 0) flag_sub = false;
		else flag_sub = true;

		if (running_list == 0) flag_run = false;
		else flag_run = true;

		if (!flag_master && !flag_sub && !flag_run)
		{
			GameManager.Instance.anim_playing = false;
			order = 0;
		}
	}

	void arrange_list()
	{
        // list empty면 flase
		if (!flag_sub && flag_master) // sub 비엇고, master 안 비엇으면. 그리고 timeterm이 0이면
		{
			convey_list(order);
			order++;
			run_animation(sub_animObjects);
        }
	}
	

	void convey_list(int order)
	{
		for (int i= 0; i < master_animObjects.Count; i ++)
		{
			if (order == master_animObjects[i].priority)
			{
				sub_animObjects.Add(master_animObjects[i]);
				master_animObjects.RemoveAt(i);
				i--;
			}
		}
	}
	public void clear_anim_list()
	{
		master_animObjects.Clear();
	}

	public void anim_add(ObjectBase _objectBase, AnimType _animType, int _UDLR, int _priority, ObjectBase _target = null)
	{
		AnimObject animObject;
		if (_animType == AnimType.MOVE && GameManager.Instance.skipmode)
			animObject = new AnimObject(_objectBase, AnimType.LANDING, _UDLR, _priority, _target);
		else
			animObject = new AnimObject(_objectBase, _animType, _UDLR, _priority, _target);
		
		master_animObjects.Add(animObject);
	}

	public void run_animation(List<AnimObject> sub_animObjects)
    {
        animCoroutinePool.run_coroutine(sub_animObjects);
    }
    public void run_animation_withoutbase(GameObject animbody, AnimType animType, int UDLR)
    {
        animCoroutinePool.run_coroutine_withoutbase(animbody, animType, UDLR);
    }

	public void run_animation_withoutbase(GameObject animbody, AnimType animType, int UDLR, Vector3 from_pos=default, Vector3 to_pos=default)
	{
		animCoroutinePool.run_coroutine_withoutbase(animbody, animType, UDLR, from_pos, to_pos);
	}

}
//각 코루틴에서 직접 애니메이션 실행 후 매님오브젝트에서 자기꺼 삭제.
//check if list is empty or not
//만약 비었으면 턴 넘기기
//priority에 따라 애니메이션 실행
public class AnimObject
{
	public ObjectBase objectBase;
    public ObjectBase targetBase;
	public AnimType animType;
	public int UDLR = 0;
	public int priority;

	public AnimObject(ObjectBase _objectBase, AnimType _animType, int _UDLR, int _priority, ObjectBase _target)
	{
		objectBase = _objectBase;
		animType = _animType;
		UDLR = _UDLR;
		priority = _priority;
        targetBase = _target;
	}



}

