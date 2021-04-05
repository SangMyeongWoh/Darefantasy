using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBodyPool : MonoBehaviour
{
    //public GameObject DeathEffect;
    //public GameObject Item;
    

    public List<ItemBox> itemList = new List<ItemBox>();
    public List<ItemUIBox> itemUIList = new List<ItemUIBox>();

    public GameObject spawn_item_body(ItemType itemType, Vector3 pos)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemType == itemList[i].itemType)
            {
                GameObject body = Instantiate(itemList[i].itemObject, pos, transform.rotation) as GameObject;
                GameManager.Instance.garbageBox.garbageList.Add(body);
                body.AddComponent<CoroutineBox>();
                return body;
            }
        }
        Debug.Log("no such item");
        return null;
    }

    public GameObject spawn_item_UI_body(ItemUIType itemUIType, Vector3 pos, bool forCharacter)
    {
        /*
        if (itemUIType == ItemUIType.ITEM_HEART_UI)
        {
            Debug.Log("there is no heart UI");
            return null;
        }*/
            
        for (int i = 0; i < itemUIList.Count; i++)
        {
            if (itemUIType == itemUIList[i].itemUIType)
            {
                GameObject UI_body = Instantiate(itemUIList[i].itemUIObject, pos, transform.rotation) as GameObject;
				if (forCharacter) //GameManager.Instance.sub_managers.uIManager.itemUIList.Add(UI_body);
					GameManager.Instance.sub_managers.uIManager.UIList_add(UI_body, itemUIType);
				UI_body.AddComponent<CoroutineBox>();
                //GameManager.Instance.garbageBox.garbageList.Add(UI_body);
                return UI_body;
            }
        }
        Debug.Log("no such itemUI");
        return null;
    }
	public GameObject spawn_item_body_UI(ItemType itemType, Vector3 pos)
	{
		GameObject item_UI = new GameObject();
		return item_UI;
	}


}

[System.Serializable]
public class ItemBox
{
    public ItemType itemType;
    public GameObject itemObject;
}

[System.Serializable]
public class ItemUIBox
{
    public ItemUIType itemUIType;
    public GameObject itemUIObject;
}