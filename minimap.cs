using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    public GameObject number1;
    public GameObject floor;    

    public GameObject minimapBackground;
    public List<GameObject> minimap_list = new List<GameObject>();
    public List<MinimapImageBox> minimap_imagelist = new List<MinimapImageBox>();

	public RoomMeta roomMeta_now;
	List<int> adj_list = new List<int>();

	int minimapCount = 0;
    int roomindex = 0;

    public bool minimapPopup;
    bool animrunning;
    bool animrunning_minimap;
    int floornumber = 2;

    // Start is called before the first frame update
    void Start()
    {
        floor.GetComponent<Animator>().SetInteger("number", 10);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        process();
    }
    void process()
    {
        if(GameManager.Instance.roomMeta_now != null)
        {
            if (GameManager.Instance.stagelv > 1) floornumber = 3 * (GameManager.Instance.stagelv - 1) + 2;
            number1.GetComponent<Animator>().SetInteger("number", floornumber - GameManager.Instance.roomMeta_now.location.x);
        }
            
        if (Input.GetKey(KeyCode.Tab))
        {
            minimapCount = 5;
        }
        
        if (minimapCount > 0) // pushing tapkey
        {
            if (!minimapPopup && !animrunning) //opening
            {
                set_minimap();
                StartCoroutine(popup());
                minimapPopup = true;
            }
            if (GameManager.Instance.roomMeta_now != null)
            {
                roomindex = GameManager.Instance.roomMeta_now.location.y * 5 + GameManager.Instance.roomMeta_now.location.z;
                if (!animrunning_minimap) StartCoroutine(minimapHere(roomindex));
            }
            minimapCount--; 
        }
        else //
        {
            if (minimapPopup && !animrunning)
            {
                StartCoroutine(popdown());
                minimapPopup = false;
            }
            
        }
        
        
    }
    /*
    void popup_minimapProcess()
    {
        minimapBackground.GetComponent<SpriteRenderer>().color = Color.Lerp(minimapBackground.GetComponent<SpriteRenderer>().color, new Color(0, 0, 0, 1), 0.4f);
        for (int i = 0; i < minimap_list.Count; i++)
        {
            popup_minimapProcess();
            minimap_list[i].GetComponent<SpriteRenderer>().color = Color.Lerp(minimap_list[i].GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0.5f), 0.4f);
        }        
    }
    void popdown_minimapProcess()
    {
        minimapBackground.GetComponent<SpriteRenderer>().color = Color.Lerp(minimapBackground.GetComponent<SpriteRenderer>().color, new Color(0, 0, 0, 0), 0.4f);
        for (int i = 0; i < minimap_list.Count; i++)
        {
            popdown_minimapProcess();
            minimap_list[i].GetComponent<SpriteRenderer>().color = Color.Lerp(minimap_list[i].GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0f), 0.4f);
        }
    }
    */
    public List<Sprite> get_minimap_image(RoomType roomType)
    {
        for (int i = 0; i < minimap_imagelist.Count; i++)
        {
            if (roomType == minimap_imagelist[i].roomType)
            {
                return minimap_imagelist[i].minimapImage;
            }
        }
        return null;
    }

    void set_minimap()
    {
        List<List<List<RoomMeta>>> RoomMeta_list = GameManager.Instance.RoomMeta_list;
        int i = GameManager.Instance.roomMeta_now.location.x;
        List<Sprite> imagelist;
        for (int j = 0; j < 5; j++)
        {
            for (int k = 0; k < 5; k++)
            {
                imagelist = get_minimap_image(RoomMeta_list[i][j][k].roomType);
                switch (RoomMeta_list[i][j][k].roomType)
                {
                    case RoomType.MAGICCIRCLE:                        
                        minimap_list[j * 5 + k].GetComponent<SpriteRenderer>().sprite = imagelist[0];
                        break;
                    case RoomType.WELL:
                        if (!RoomMeta_list[i][j][k].WellON)
                        {
                            minimap_list[j * 5 + k].GetComponent<SpriteRenderer>().sprite = imagelist[0];
                        }
                        else
                        {
                            minimap_list[j * 5 + k].GetComponent<SpriteRenderer>().sprite = imagelist[1];
                        }
                        

                        break;
                    case RoomType.NORMAL:
                        minimap_list[j * 5 + k].GetComponent<SpriteRenderer>().sprite = imagelist[0];
                        break;
                    default:
                        minimap_list[j * 5 + k].GetComponent<SpriteRenderer>().sprite = minimap_imagelist[0].minimapImage[0];
                        break;
                }
            }
        }
    }
    IEnumerator minimapHere(int roomindex)
    {
        animrunning_minimap = true;
		if (roomMeta_now == null || roomMeta_now != GameManager.Instance.roomMeta_now)
		{
            roomMeta_now = GameManager.Instance.roomMeta_now;
            set_minimap();
            for (int i = 0; i < Constant.MAX_INDEX; i++)
            {
                for (int j = 0; j < Constant.MAX_INDEX; j++)
                {
                    if (GameManager.Instance.RoomMeta_list[roomMeta_now.location.x][i][j].roomType != RoomType.None)
                    {
                        minimap_list[5 * i + j].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
                    }
                    else
                    {
                        minimap_list[5 * i + j].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.17255f);
                    }
                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            if(i < 5)
            {
                minimap_list[roomindex].transform.localScale = Vector3.Lerp(minimap_list[roomindex].transform.localScale, new Vector3(1.6f, 1.6f, 1), 0.3f);
				
			}
            else
            {
                minimap_list[roomindex].transform.localScale = Vector3.Lerp(minimap_list[roomindex].transform.localScale, new Vector3(1.2f, 1.2f, 1), 0.3f);
				
			}
            yield return new WaitForSeconds(0.02f);
        }
        minimap_list[roomindex].transform.localScale = new Vector3(1, 1, 1);
        animrunning_minimap = false;

    }
    IEnumerator popup()
    {
        animrunning = true;
        for(int i = 0; i < 20; i++)
        {
            if(i < 5)
            {
                minimapBackground.transform.localScale = Vector3.Lerp(minimapBackground.transform.localScale, new Vector3(2f,0.4f,1), 0.5f);
            } else if(i >= 5 && i < 10)
            {
                minimapBackground.transform.localScale = Vector3.Lerp(minimapBackground.transform.localScale, new Vector3(0.4f, 2f, 1), 0.5f);
            }
            else
            {
                minimapBackground.transform.localScale = Vector3.Lerp(minimapBackground.transform.localScale, new Vector3(1.2f, 1.2f, 1), 0.5f);
            }
            yield return new WaitForSeconds(0.02f);
        }
        animrunning = false;
    }
    IEnumerator popdown()
    {
        animrunning = true;
        for (int i = 0; i < 20; i++)
        {
            if (i < 5)
            {
                minimapBackground.transform.localScale = Vector3.Lerp(minimapBackground.transform.localScale, new Vector3(2f, 0.4f, 1), 0.5f);
            }
            else if (i >= 5 && i < 10)
            {
                minimapBackground.transform.localScale = Vector3.Lerp(minimapBackground.transform.localScale, new Vector3(0.4f, 2f, 1), 0.5f);
            }
            else
            {
                minimapBackground.transform.localScale = Vector3.Lerp(minimapBackground.transform.localScale, new Vector3(0f, 0f, 1), 0.5f);
            }
            yield return new WaitForSeconds(0.02f);
        }
        animrunning = false;

    }
}
[System.Serializable]
public class MinimapImageBox
{
    public RoomType roomType;
    public List<Sprite> minimapImage = new List<Sprite>();
}