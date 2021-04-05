using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanimator : MonoBehaviour
{
    // Start is called before the first frame update
    Animator temp;
    public GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            target = GameManager.Instance.pools.objectBodyPool.get_body(MonsterType.ZOMBIE, Vector3.zero);
            target.GetComponentInChildren<Animator>().SetInteger("MoveType", 7);
        }
    }

}
