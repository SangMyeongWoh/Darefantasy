using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> BGM_List = new List<AudioClip>();
    // Start is called before the first frame update
    public void process()
    {
        change_bgm_proces();
    }

    void change_bgm_proces()
    {
        if(GameManager.Instance.roomMeta_now != null)
        {
            if(GameManager.Instance.playerBase.objectStatus.heart > 0)
            {
                switch (GameManager.Instance.roomMeta_now.roomType)
                {
                    case RoomType.MAGICCIRCLE:
                        gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(gameObject.GetComponent<AudioSource>().volume, 0, 0.05f);
                        break;
                    default:
                        gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(gameObject.GetComponent<AudioSource>().volume, 0.2f, 0.1f);
                        break;

                }
            }
            else
            {
                gameObject.GetComponent<AudioSource>().volume = Mathf.Lerp(gameObject.GetComponent<AudioSource>().volume, 0, 0.03f);
            }
            
        }
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = BGM_List[Random.Range(0, BGM_List.Count)];
            GetComponent<AudioSource>().Play();
        }
    }
}
