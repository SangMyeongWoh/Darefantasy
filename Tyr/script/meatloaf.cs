using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatloaf : MonoBehaviour
{
    Vector3 wayVector = Vector3.zero;
    Vector3 posVector;

    private void Start()
    {
        wayVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) + gameObject.transform.position;
        StartCoroutine(Corpse());
    }
    IEnumerator Corpse()
    {
        posVector = transform.position;
        for (int i = 0; i < 30; i++)
        {
            if (i % 5 == 0)
            {
                gameObject.GetComponent<SortingOrderSetter>().change_sorting_order(transform.position - posVector);
                posVector = transform.position;
            }
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, wayVector, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        gameObject.GetComponent<SortingOrderSetter>().change_sorting_order(transform.position - posVector);
    }

}
