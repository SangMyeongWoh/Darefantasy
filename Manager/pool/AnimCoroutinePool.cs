using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCoroutinePool : MonoBehaviour
{
    bool highlightisRunning;
    public List<GameObject> highlightlist = new List<GameObject>();
    //EffectPool effectPool = GameManager.Instance.pools.effectPool;

    public void run_coroutine(List<AnimObject> sub_animObjects)
    {
        while(sub_animObjects.Count > 0)
        {
            AnimObject animObject = sub_animObjects[0];
            if (animObject.objectBase.ObjectBody == null)
            {
                switch (animObject.objectBase.objectBase_type)
                {
                    case ObjectBaseType.BLOCK:
                        BlockBase blockBase = ((BlockBase)animObject.objectBase);
                        GameManager.Instance.pools.objectBodyPool.get_body(blockBase.blockType, blockBase.node_now.position);
                        break;
                    case ObjectBaseType.CHARACTER:
                        PlayerBase playerBase = ((PlayerBase)animObject.objectBase);
                        GameManager.Instance.pools.objectBodyPool.get_body(playerBase.monsterType, playerBase.node_now.position);
                        break;
                    case ObjectBaseType.DOOR:
                        DoorBase doorBase = ((DoorBase)animObject.objectBase);
                        GameManager.Instance.pools.objectBodyPool.get_body(doorBase.doorType, doorBase.node_now.position, animObject.UDLR);
                        break;
                    case ObjectBaseType.ITEM:
                        ItemBase itemBase = ((ItemBase)animObject.objectBase);
                        GameManager.Instance.pools.objectBodyPool.get_body(itemBase.itemType, itemBase.node_now.position);
                        break;
                    case ObjectBaseType.MONSTER:
                        MonsterBase monsterBase = ((MonsterBase)animObject.objectBase);
                        GameManager.Instance.pools.objectBodyPool.get_body(monsterBase.monsterType, monsterBase.node_now.position);
                        break;
                    case ObjectBaseType.TRAP:
                        TrapBase trapBase = ((TrapBase)animObject.objectBase);
                        GameManager.Instance.pools.objectBodyPool.get_body(trapBase.trapType, trapBase.node_now.position);
                        break;
                }
            }
            else
            {
                animObject.objectBase.ObjectBody.SetActive(true);
            }

            if(animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow != null)
            {
                StopCoroutine(animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow);
				GameManager.Instance.sub_managers.animManager.running_list--;
                animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
                recall(animObject);
                animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
            }

            switch (animObject.animType)
            {
                case AnimType.ATTACK:              
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(attack(animObject, sub_animObjects));
                    break;
                case AnimType.CANNOTATTACK:
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(cannot_attack(animObject, sub_animObjects));
                    break;
                case AnimType.HIT:
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(hit(animObject, sub_animObjects));
                    break;
                case AnimType.DEATH:
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(death(animObject, sub_animObjects));
                    break;
                case AnimType.DEATHWITHOUTDELAY:
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(death_withoutdelay(animObject, sub_animObjects));
                    break;
                case AnimType.MOVE:
                    //animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.LANDING);
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(move(animObject, sub_animObjects));
                    
                    break;
                case AnimType.FASTMOVE:
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(fast_move(animObject, sub_animObjects));
                    break;

                case AnimType.TRY:
                    //StartCoroutine(try_something(animObject, sub_animObjects));
                    break;
                case AnimType.LANDING:
					animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.LANDING);
                    recall(animObject);
                    break;
                case AnimType.HIGHLIGHT:
                    if(!highlightlist.Contains(animObject.objectBase.ObjectBody))
                        highlightlist.Add(animObject.objectBase.ObjectBody);
                    break;
                case AnimType.HIT_WITHBLOOD:
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(hit_withblood(animObject, sub_animObjects));
                    break;
                case AnimType.HIT_WITHOUTBLOOD:
                    animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = StartCoroutine(hit_withoutblood(animObject, sub_animObjects));
                    break;
                case AnimType.ITEMEFFECT:
                    StartCoroutine(itemeffect(animObject));
                    break;
            }            
            sub_animObjects.RemoveAt(0);
        }
        if (!highlightisRunning && highlightlist.Count > 0) StartCoroutine(highlight(sub_animObjects));
    }

    public void run_coroutine_withoutbase(GameObject body, AnimType animType, int UDLR)
    {
        //if (body.GetComponent<CoroutineBox>().coroutineNow != null)
        //{
        //    StopCoroutine(body.GetComponent<CoroutineBox>().coroutineNow);
        //    body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);            
        //    body.GetComponent<CoroutineBox>().coroutineNow = null;
        //}
        switch (animType)
        {
            case AnimType.ATTACK:
				StartCoroutine(attack(body, animType, UDLR));
                break;
            case AnimType.HIT:
				StartCoroutine(hit(body, animType, UDLR));
                break;
            case AnimType.DEATH:
				body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.DEATH);
                break;
            case AnimType.MOVE:
                //StartCoroutine(move(body, animType, UDLR));
                break;
            case AnimType.RECALL:
                Debug.Log("can't recall without body");
                break;
            case AnimType.TRY:
                //StartCoroutine(try_something(animObject, sub_animObjects));
                break;
            case AnimType.LANDING:
				body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.LANDING);                
                break;
            case AnimType.HIGHLIGHT:
                if(!highlightlist.Contains(body))
                    highlightlist.Add(body);
                break;
            case AnimType.IDLE:
                body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
                break;
	        case AnimType.HIT_WITHBLOOD:
                break;
        }
        if (!highlightisRunning && highlightlist.Count > 0) StartCoroutine(highlight());
    }

    public void run_coroutine_withoutbase(GameObject body, AnimType animType, int UDLR, Vector3 from_pos, Vector3 to_pos)
    {
        //if (body.GetComponent<CoroutineBox>().coroutineNow != null)
        //{
        //    StopCoroutine(body.GetComponent<CoroutineBox>().coroutineNow);
        //    body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
        //    body.GetComponent<CoroutineBox>().coroutineNow = null;
        //}
        switch (animType)
        {
            case AnimType.ATTACK:
				StartCoroutine(attack(body, animType, UDLR));
                break;
            case AnimType.HIT:
				StartCoroutine(hit(body, animType, UDLR));
                break;
            case AnimType.DEATH:
				body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.DEATH);
                break;
            case AnimType.MOVE:
				StartCoroutine(move(body, animType, UDLR, from_pos, to_pos));
                break;
            case AnimType.RECALL:
                break;
            case AnimType.TRY:
                Debug.Log("nothin yet");
                break;
            case AnimType.LANDING:
                body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.LANDING);
                break;
            case AnimType.HIGHLIGHT:
                if (!highlightlist.Contains(body))
                    highlightlist.Add(body);
                break;
        }
    }

    public void recall(AnimObject animObject)
    {
        Vector3 distance = animObject.objectBase.ObjectBody.transform.position;
        if(animObject.objectBase.objectBase_type != ObjectBaseType.DOOR)
        {
            animObject.objectBase.ObjectBody.transform.position = animObject.objectBase.node_now.position;
            animObject.objectBase.ObjectBody.GetComponent<SortingOrderSetter>().change_sorting_order(animObject.objectBase.ObjectBody.transform.position - distance);
        }
    }

    #region withbase
    IEnumerator attack(AnimObject animObject, List<AnimObject> sub_animObjectsm)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        if (animObject.UDLR == 2) //means left
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (animObject.UDLR == 3) //means right
        {
            
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }
        yield return new WaitForSeconds(Random.Range(0, 0.2f));
        
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i == 0 && animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>())
                animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.ATTACK);
            else if(i == Constant.HIGHTLIGHT_ANIMTIME  - 20 && animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>())
                animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.ATTACKEND);            
            yield return new WaitForSeconds(0.02f);
        }
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;
	}

    IEnumerator cannot_attack(AnimObject animObject, List<AnimObject> sub_animObjectsm)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        if (animObject.UDLR == 2) //means left
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (animObject.UDLR == 3) //means right
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }
        yield return new WaitForSeconds(Random.Range(0, 0.2f));
        animObject.objectBase.ObjectBody.GetComponent<Character>().cannot_attack();
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i == 0) animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.ATTACK);
            else if (i == Constant.HIGHTLIGHT_ANIMTIME - 20) animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.ATTACKEND);
            yield return new WaitForSeconds(0.02f);
        }
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;
    }
    
    IEnumerator itemeffect(AnimObject animObject)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        
        for (int i = 0; i < animObject.objectBase.used_item_List.Count; i++)
        {
            int LR = 0;
            if(animObject.targetBase.node_now.position.x - animObject.objectBase.node_now.position.x > 0)
            {
                LR = 1;
            } else if (animObject.targetBase.node_now.position.x - animObject.objectBase.node_now.position.x == 0)
            {
                LR = Random.Range(0, 2);
            }
            GameManager.Instance.pools.effectPool.callEffect(animObject.objectBase.used_item_List[i], animObject.targetBase.node_now, LR);
            yield return new WaitForSeconds(Random.Range(0, 0.2f));
        }
        animObject.objectBase.used_item_List.Clear();
        GameManager.Instance.sub_managers.animManager.running_list--;
    }


    IEnumerator hit(AnimObject animObject, List<AnimObject> sub_animObjects)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        
        yield return new WaitForSeconds(0.5f);
        if (animObject.UDLR == 2)
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);

        }
        else if (animObject.UDLR == 3)
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }

        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i == 0) animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.HIT);
            else if (i == Constant.HIGHTLIGHT_ANIMTIME - 30)
            {
                animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
            }
            yield return new WaitForSeconds(0.02f);
        }
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;
    }

    IEnumerator hit_withblood(AnimObject animObject, List<AnimObject> sub_animObjects)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        
        yield return new WaitForSeconds(0.5f);
        if (animObject.UDLR == 2)
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);

        }
        else if (animObject.UDLR == 3)
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }

        GameManager.Instance.sub_managers.cameraManager.run_camera_anim(CameraAnimType.HP_DOWN);
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i == 0) animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.HIT);
            else if (i == Constant.HIGHTLIGHT_ANIMTIME - 30)
            {
                animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
            }
            yield return new WaitForSeconds(0.02f);
        }
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;
    }

    IEnumerator hit_withoutblood(AnimObject animObject, List<AnimObject> sub_animObjects)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;

        yield return new WaitForSeconds(0.5f);
        if (animObject.UDLR == 2)
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);

        }
        else if (animObject.UDLR == 3)
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }

        GameManager.Instance.sub_managers.cameraManager.run_camera_anim(CameraAnimType.SHAKE_CAMERA, count:15);
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i == 0) animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.HIT);
            else if (i == Constant.HIGHTLIGHT_ANIMTIME - 30)
            {
                animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
            }
            yield return new WaitForSeconds(0.02f);
        }
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;
    }    

    IEnumerator death(AnimObject animObject, List<AnimObject> sub_animObjects)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        yield return new WaitForSeconds(Random.Range(0.5f, 0.7f));

        animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.DEATH);
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;
    }

    IEnumerator death_withoutdelay(AnimObject animObject, List<AnimObject> sub_animObjects)
    {
        //GameManager.Instance.sub_managers.animManager.running_list++;        
        animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.DEATH);        
        //GameManager.Instance.sub_managers.animManager.running_list--;
        yield return new WaitForSeconds(0f);
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
    }

    IEnumerator move(AnimObject animObject, List<AnimObject> sub_animObjects)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        if (animObject.UDLR == 2) //means left
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);

        } else if(animObject.UDLR == 3) //means right
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }
        animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.MOVE);

        Vector3 min_distance = (animObject.objectBase.node_now.position - animObject.objectBase.ObjectBody.transform.position) / (Constant.ANIMTIME);
        Vector3 distance = animObject.objectBase.ObjectBody.transform.position;

        for(int i = 0; i < Constant.ANIMTIME; i++)
        {
            animObject.objectBase.ObjectBody.transform.position += min_distance;
            animObject.objectBase.ObjectBody.GetComponent<SortingOrderSetter>().change_sorting_order(animObject.objectBase.ObjectBody.transform.position - distance);
            distance = animObject.objectBase.ObjectBody.transform.position;
            if (i % 5 == 0)
            {
                
            }
            yield return new WaitForSeconds(0.02f);
        }
        animObject.objectBase.ObjectBody.transform.position = animObject.objectBase.node_now.position;
        animObject.objectBase.ObjectBody.GetComponent<SortingOrderSetter>().change_sorting_order(animObject.objectBase.node_now.position - distance);
        animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;

    }

    IEnumerator fast_move(AnimObject animObject, List<AnimObject> sub_animObjects)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        if (animObject.UDLR == 2) //means left
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);

        }
        else if (animObject.UDLR == 3) //means right
        {
            animObject.objectBase.ObjectBody.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }
        animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.MOVE);

        Vector3 min_distance = (animObject.objectBase.node_now.position - animObject.objectBase.ObjectBody.transform.position) / 2;
        Vector3 distance = animObject.objectBase.ObjectBody.transform.position;

        for (int i = 0; i < 5; i++)
        {
            animObject.objectBase.ObjectBody.transform.position += min_distance;
            animObject.objectBase.ObjectBody.GetComponent<SortingOrderSetter>().change_sorting_order(animObject.objectBase.ObjectBody.transform.position - distance);
            distance = animObject.objectBase.ObjectBody.transform.position;
            if (i % 5 == 0)
            {

            }
            yield return new WaitForSeconds(0.02f);
        }
        animObject.objectBase.ObjectBody.transform.position = animObject.objectBase.node_now.position;
        animObject.objectBase.ObjectBody.GetComponent<SortingOrderSetter>().change_sorting_order(animObject.objectBase.node_now.position - distance);
        animObject.objectBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
        animObject.objectBase.ObjectBody.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;

    }

    IEnumerator highlight(List<AnimObject> sub_animObjects)
    {
        highlightisRunning = true;
        
        for(int i = 0; i < highlightlist.Count; i++)
        {
            SpriteRenderer[] rendererlist = highlightlist[i].GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < rendererlist.Length; j++)
            {
                if (rendererlist[j].sortingOrder == Constant.FLOORSPRITESORTINGORDER)
                {
                    rendererlist[j].sortingOrder = 15001;
                }
                else rendererlist[j].sortingOrder += 15000;
            }

        }
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if(i < 15)
            {
                Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0.8f), 0.2f);
            } else if(i > Constant.HIGHTLIGHT_ANIMTIME - 15)
            {
                Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0f), 0.2f);                
            }             
            
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < highlightlist.Count; i++)
        {
            SpriteRenderer[] rendererlist = highlightlist[i].GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < rendererlist.Length; j++)
            {
                if (rendererlist[j].sortingOrder == 15001)
                {
                    rendererlist[j].sortingOrder = Constant.FLOORSPRITESORTINGORDER;
                }
                else rendererlist[j].sortingOrder -= 15000;
            }
        }
        highlightlist.RemoveRange(0, highlightlist.Count);
        

       
        highlightisRunning = false;
        
    }

    #endregion

    #region withoutbase

    IEnumerator attack(GameObject body, AnimType animType, int UDLR)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        if (UDLR == 2) //means left
        {
            body.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);

        }
        else if (UDLR == 3) //means right
        {
            body.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }
        yield return new WaitForSeconds(Random.Range(0, 0.2f));
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i == 0) body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.ATTACK);
            else if (i == Constant.HIGHTLIGHT_ANIMTIME - 20) body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.ATTACKEND);
            yield return new WaitForSeconds(0.02f);
        }
        body.GetComponent<CoroutineBox>().coroutineNow = null;
        GameManager.Instance.sub_managers.animManager.running_list--;
    }

    IEnumerator hit(GameObject body, AnimType animType, int UDLR)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        if (UDLR == 2)
        {
            body.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);

        }
        else if (UDLR == 3)
        {
            body.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i == 0) body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.HIT);
            else if (i == Constant.HIGHTLIGHT_ANIMTIME - 30)
            {
                body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
            }            

            yield return new WaitForSeconds(0.02f);
        }
        body.GetComponent<CoroutineBox>().coroutineNow = null;
		GameManager.Instance.sub_managers.animManager.running_list--;
	}

    IEnumerator move(GameObject body, AnimType animType, int UDLR, Vector3 from_pos, Vector3 to_pos)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        if (UDLR == 2) //means left
        {
            body.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(1, 1, 1);

        }
        else if (UDLR == 3) //means right
        {
            body.GetComponent<ObjectBody>().body.transform.localScale = new Vector3(-1, 1, 1);
        }
        body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.MOVE);

		Vector3 min_distance = (to_pos - from_pos) / Constant.ANIMTIME;
        Vector3 distance = from_pos;
        for (int i = 0; i < Constant.ANIMTIME; i++)
        {
            body.transform.position += min_distance;
            if (i % 5 == 0)
            {
                body.GetComponent<SortingOrderSetter>().change_sorting_order(body.transform.position - distance);
                distance = body.transform.position;
		    }
            yield return new WaitForSeconds(0.02f);
        }
        body.transform.position = to_pos;
        body.GetComponent<SortingOrderSetter>().change_sorting_order(body.transform.position - distance);
        body.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
        body.GetComponent<CoroutineBox>().coroutineNow = null;
		GameManager.Instance.sub_managers.animManager.running_list--;
	}

    IEnumerator highlight()
    {
        highlightisRunning = true;
        for (int i = 0; i < highlightlist.Count; i++)
        {
            SpriteRenderer[] rendererlist = highlightlist[i].GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < rendererlist.Length; j++)
            {
                if(rendererlist[j].sortingOrder == Constant.FLOORSPRITESORTINGORDER)
                {
                    rendererlist[j].sortingOrder = 15001;
                }
                else rendererlist[j].sortingOrder += 15000;

            }
        }
        for (int i = 0; i < Constant.HIGHTLIGHT_ANIMTIME; i++)
        {
            if (i < 15)
            {
                Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0.8f), 0.2f);
            }
            else if (i > Constant.HIGHTLIGHT_ANIMTIME - 15)
            {
                Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color =
                    Color.Lerp(Camera.main.GetComponent<CameraManager>().Blind.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0f), 0.2f);
            }

            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < highlightlist.Count; i++)
        {
            SpriteRenderer[] rendererlist = highlightlist[i].GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < rendererlist.Length; j++)
            {
                if (rendererlist[j].sortingOrder == 15001)
                {
                    rendererlist[j].sortingOrder = Constant.FLOORSPRITESORTINGORDER;
                }
                else rendererlist[j].sortingOrder -= 15000;
            }
        }
        highlightlist.RemoveRange(0, highlightlist.Count);
        highlightisRunning = false;

    }
    #endregion


    IEnumerator Itemdeath(GameObject body)
    {
        GameManager.Instance.sub_managers.animManager.running_list++;
        SpriteRenderer[] spriteRenderers = body.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < 20; i++)
        {
            for(int j = 0; j < spriteRenderers.Length; j++)
            {
                spriteRenderers[j].color = Color.Lerp(spriteRenderers[j].color, new Color(1, 1, 1, 0), 0.3f);
            }
            if(i < 5)
            {
                body.transform.localScale = Vector3.Lerp(body.transform.localScale, new Vector3(3, 0.2f, 1), 0.5f);
            }
            else
            {
                body.transform.localScale = Vector3.Lerp(body.transform.localScale, new Vector3(0.01f, 10f, 1), 0.8f);
            }
            yield return new WaitForSeconds(0.02f);
		}
		GameManager.Instance.sub_managers.animManager.running_list--;
	}

}

