using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject HeartUI;
    public GameObject ItemUI;

    public GameObject healthnumeber;

	public List<GameObject> itemUIList = new List<GameObject>();
	public List<GameObject> heartUIList = new List<GameObject>();


	public void process()
    {
        //if(GameManager.Instance.playerBase != null)
        //    healthnumeber.GetComponent<Animator>().SetInteger("number", GameManager.Instance.playerBase.objectStatus.heart);
        for (int i = 0; i < itemUIList.Count; i++)
        {
            if(itemUIList[i] == null)
            {
                itemUIList.RemoveAt(i);
                break;
            }

            itemUIList[i].transform.position = Vector3.Lerp(itemUIList[i].transform.position, new Vector3(6.1f, -2f + i, 0), 0.1f);
            itemUIList[i].transform.position = Vector3.Lerp(itemUIList[i].transform.position, itemUIList[i].transform.position + new Vector3(Random.Range(-0.2f,0.2f), Random.Range(-0.2f,0.2f), 0), 0.04f);
        }
		for (int i = 0; i < heartUIList.Count; i++)
		{
			if (heartUIList[i] == null)
			{
				heartUIList.RemoveAt(i);
				break;
			}

			heartUIList[i].transform.position = Vector3.Lerp(heartUIList[i].transform.position, new Vector3(7.1f, -2f + i, 0), 0.1f);
			heartUIList[i].transform.position = Vector3.Lerp(heartUIList[i].transform.position, heartUIList[i].transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0), 0.04f);
		}
	}
    public void heartUI_death()
    {
        HeartUI.GetComponentInChildren<HeartUIDeath>().heartUI_death();
    }

	public void UIList_add(GameObject UI_body, ItemUIType itemUIType)
	{
		if (itemUIType == ItemUIType.ITEM_HEART_UI || itemUIType == ItemUIType.ITEM_BLUEHEART_UI || itemUIType == ItemUIType.ITEM_BAG_HEART_UI)
			GameManager.Instance.sub_managers.uIManager.heartUIList.Add(UI_body);
		else
			GameManager.Instance.sub_managers.uIManager.itemUIList.Add(UI_body);
	}
    
}
