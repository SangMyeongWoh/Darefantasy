using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBox : MonoBehaviour
{
    public List<GameObject> garbageList = new List<GameObject>();

    public void clean_all()
    {
		// StartCoroutine(clean_object());
		for_clean_object();
    }

    IEnumerator clean_object()
    {
        for (int i = 0; i < garbageList.Count; i++)
        {
            if(garbageList[i] != null) Destroy(garbageList[i]);
            yield return new WaitForSeconds(0.02f);
        }
        garbageList.RemoveRange(0, garbageList.Count);
    }

	public void for_clean_object()
	{
		for (int i = 0; i < garbageList.Count; i++)
		{
			if (garbageList[i] != null) Destroy(garbageList[i]);
		}
		garbageList.RemoveRange(0, garbageList.Count);
	}
}
