using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    #region effect_objects

    public GameObject Blind;
    public GameObject BloodEffect;
    public GameObject CameraEffect_door;
    public GameObject HeartUI;
    public GameObject TotalUI;
    public GameObject HideScreen;
    public GameObject SoftTransitionObject;
    public GameObject DeathScene;

    #endregion

    SpriteRenderer[] spriteRenderers;
    bool scenechangestart;

    private void FixedUpdate()
    {
    }

    #region coroutine pool

    IEnumerator shake_camera(int count)
    {
        for (int i = 0; i < count; i++)
        {
            gameObject.transform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, -10), 0.2f);
            yield return new WaitForSeconds(0.02f);
        }
        gameObject.transform.position = new Vector3(0, 0, -10);
    }

    IEnumerator room_transition_hard()
    {
        gameObject.transform.position = new Vector3(0, 1.2f, -10);
        HeartUI.GetComponent<Animator>().SetInteger("MoveType", (int)AnimType.LANDING);
        for(int i = 0; i < GameManager.Instance.sub_managers.uIManager.itemUIList.Count; i++)
        {
            GameManager.Instance.sub_managers.uIManager.itemUIList[i].transform.position += new Vector3(Random.Range(-0.1f, 0.1f), 0.5f + 0.5f*i, 0);
        }
        for (int i = 0; i < 10; i++)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, 0, -10), 0.4f);
            yield return new WaitForSeconds(0.02f);
        }
        gameObject.transform.position = new Vector3(0, 0, -10);
    }

    IEnumerator room_transition_soft(Vector3 fromvector, Vector3 tovector)
    {
        SoftTransitionObject.transform.position = fromvector;
        Vector3 movevector = (tovector - fromvector)/20f;
        if(movevector.x != 0)
        {
            SoftTransitionObject.transform.eulerAngles = Vector3.zero;
        }
        else
        {
            SoftTransitionObject.transform.eulerAngles = new Vector3(0,0,90);
        }
        for (int i = 0; i < 20; i++)
        {
            SoftTransitionObject.transform.position += movevector;            
            
            yield return new WaitForSeconds(0.02f);
        }        
    }

    IEnumerator hide_screen(int count)
    {
        if (count < 50)
            count = 50;
        for (int i = 0; i < count; i++)
        {
            if(i < 25)
            {
                HideScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(HideScreen.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1), 0.1f);
            }
            else if(i > count - 25)
            {
                HideScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(HideScreen.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0), 0.1f);
            }
            yield return new WaitForSeconds(0.02f);
        }
        HideScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    IEnumerator zoom_camera(int count, Vector3 pos, int size)
    {
        if (count < 45)
            count = 45;
        for (int i = 0; i < count; i++)
        {
            if (i < 15)
            {
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, size, 0.2f);
                transform.position = Vector3.Lerp(transform.position, pos + new Vector3(0,0,-10), 0.1f);
            }
            else if (i > count - 30)
            {
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 5, 0.2f);
                transform.position = Vector3.Lerp(transform.position, new Vector3(0,0,-10), 0.1f);
            }
            TotalUI.transform.localScale = new Vector3(Camera.main.GetComponent<Camera>().orthographicSize / 5, Camera.main.GetComponent<Camera>().orthographicSize / 5, 1);
            yield return new WaitForSeconds(0.02f);
        }
        TotalUI.transform.localScale = new Vector3(1, 1, 1);
        GetComponent<Camera>().orthographicSize = 5;
    }

    IEnumerator character_death()
    {
        GameManager.Instance.sub_managers.uIManager.heartUI_death();
        StartCoroutine(zoom_camera(10000, GameManager.Instance.playerBase.node_now.position, 3));
        yield return new WaitForSeconds(5f);
        
        DeathScene.GetComponent<Animator>().SetInteger("Type", Random.Range(1, 2));
        DeathScene.GetComponent<AudioSource>().Play();
        spriteRenderers = DeathScene.GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < 50000; i++)
        {
            if (Input.anyKeyDown)
            {                
                if(i < 500)
                {                    
                    HideScreen.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    DeathScene.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    for (int j = 0; j < spriteRenderers.Length; j++)
                    {
                        spriteRenderers[j].color = new Color(1, 1, 1, 1);
                    }
                    i = 500;
                }
                else
                {
                    i = 500;
                    scenechangestart = true;
                }
                

            }
            if (scenechangestart)
            {
                
                DeathScene.GetComponent<SpriteRenderer>().color = Color.Lerp(DeathScene.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0f), 0.1f);
                for (int j = 0; j < spriteRenderers.Length; j++)
                {
                    spriteRenderers[j].color = Color.Lerp(spriteRenderers[j].color, new Color(1, 1, 1, 0), 0.1f);
                }
                DeathScene.GetComponent<AudioSource>().volume = Mathf.Lerp(DeathScene.GetComponent<AudioSource>().volume, 0, 0.1f);
                if (i > 550)
                    SceneManager.LoadScene("MainMenu");
            }
            else
            {
                HideScreen.GetComponent<SpriteRenderer>().color = Color.Lerp(HideScreen.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1f), 0.02f);
                if (i > 90 && i < 500)
                {
                    DeathScene.GetComponent<SpriteRenderer>().color = Color.Lerp(DeathScene.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1f), 0.01f);
                    for (int j = 0; j < spriteRenderers.Length; j++)
                    {
                        spriteRenderers[j].color = Color.Lerp(spriteRenderers[j].color, new Color(1, 1, 1, 1), 0.01f);
                    }

                }
            }
            
                
            yield return new WaitForSeconds(0.02f);
        } 
    }

    #endregion

    /*
    public void run_camera_anim(CameraAnimType cameraAnimType, int count, Vector3 pos, int size)
    {
        switch (cameraAnimType)
        {
            case CameraAnimType.ROOMTRAINSITION_HARD:
                StartCoroutine(room_transition_hard());
                break;
            case CameraAnimType.ROOMTRAINSITION_SOFT:
                Debug.Log("need from vector and to vector");
                break;
            case CameraAnimType.HIDESCREEN:
                StartCoroutine(hide_screen(count));
                break;
            case CameraAnimType.SHAKE_CAMERA:
                StartCoroutine(shake_camera(count));
                break;
            case CameraAnimType.HP_DOWN:
                HeartUI.GetComponent<Animator>().SetInteger("MoveType", (int)AnimType.HIT);
                StartCoroutine(BloodEffect.GetComponent<CameraBloodEffect>().bloodstain());
                StartCoroutine(shake_camera(15));
                break;
            case CameraAnimType.ZOOM_CAMERA:
                StartCoroutine(zoom_camera(count, pos, size));
                break;
        }
    }
	*/

    public void run_camera_anim(CameraAnimType cameraAnimType, int count = 0, Vector3 pos=default, int size=0, Vector3 fromvector = default, Vector3 tovector = default)
    {
        switch (cameraAnimType)
        {
            case CameraAnimType.ROOMTRANSITION_HARD:
                StartCoroutine(room_transition_hard());
                break;
            case CameraAnimType.ROOMTRANSITION_SOFT:
                StartCoroutine(room_transition_soft(fromvector, tovector));
                break;
            case CameraAnimType.HIDESCREEN:
                StartCoroutine(hide_screen(count));
                break;
            case CameraAnimType.SHAKE_CAMERA:
                StartCoroutine(shake_camera(count));
                break;
            case CameraAnimType.HP_DOWN:
                HeartUI.GetComponent<Animator>().SetInteger("MoveType", (int)AnimType.HIT);
                StartCoroutine(BloodEffect.GetComponent<CameraBloodEffect>().bloodstain());
                StartCoroutine(shake_camera(15));
                break;
            case CameraAnimType.ZOOM_CAMERA:
                StartCoroutine(zoom_camera(count, pos, size));
                break;
            case CameraAnimType.CHARACTER_DEATH:
                StartCoroutine(character_death());
                break;
        }
    }

}
