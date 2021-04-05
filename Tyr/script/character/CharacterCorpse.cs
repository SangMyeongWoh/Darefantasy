using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCorpse : MonoBehaviour
{    
    Vector3 wayVector = Vector3.zero;
    Vector3 posVector;
    float anglenum = 0;

    private void Start()
    {
        wayVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) + gameObject.transform.position;
        anglenum = Random.Range(-5f, 5f);
        StartCoroutine(Corpse());
    }
    IEnumerator Corpse()
    {
        posVector = transform.position;
        for (int i = 0; i < 30; i++)
        {
            if(i % 5 == 0)
            {
                gameObject.GetComponent<SortingOrderSetter>().change_sorting_order(transform.position - posVector);
                posVector = transform.position;
            }
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, wayVector, 0.1f);
            anglenum *= 0.92f;
            gameObject.transform.localEulerAngles += new Vector3(0, 0, anglenum);
            yield return new WaitForSeconds(0.02f);
        }
        gameObject.GetComponent<SortingOrderSetter>().change_sorting_order(transform.position - posVector);
    }

}
