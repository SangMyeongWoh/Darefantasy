using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicCircle : MonoBehaviour
{
    public List<GameObject> TriangleList = new List<GameObject>();
    public List<GameObject> TriangleTextList = new List<GameObject>();

    public GameObject Circle_Text;
    public GameObject BackCircle;
    public GameObject CenterCircle;
    public GameObject tentacle;
    public GameObject Effect;
    

    private void OnEnable()
    {
        StartCoroutine(magic_circle());
    }

    public void stage_up()
    {
        StartCoroutine(start_stage_change());
    }

    void FixedUpdate()
    {
    }

    IEnumerator magic_circle()
    {
        if (GameManager.Instance.wellpoint > 2) {
            GetComponent<AudioSource>().Play();
            Effect.GetComponent<ParticleSystem>().Play();
        }
        if(GameManager.Instance.wellpoint > 0)
            Circle_Text.GetComponent<AudioSource>().Play();
        for (int i = 0; i < 80; i++)
        {
            for(int j = 0; j < GameManager.Instance.wellpoint; j++)
            {
                TriangleList[j].GetComponent<SpriteRenderer>().color = Color.Lerp(TriangleList[j].GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1f), 0.03f);
                if(!TriangleList[j].GetComponentInChildren<ParticleSystem>().isPlaying)
                    TriangleList[j].GetComponentInChildren<ParticleSystem>().Play();
                TriangleTextList[j].GetComponent<SpriteRenderer>().color = Color.Lerp(TriangleTextList[j].GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1f), 0.03f);
            }
            yield return new WaitForSeconds(0.02f);
        }
        
             
    }
    IEnumerator start_stage_change()
    {
        if (GameManager.Instance.wellpoint > 2)
        {
            GameManager.Instance.sub_managers.playerManager.cannotmove = true;
            BackCircle.GetComponent<AudioSource>().Play();
            CenterCircle.GetComponentInChildren<ParticleSystem>().Play();
            
            GameManager.Instance.sub_managers.cameraManager.run_camera_anim(CameraAnimType.SHAKE_CAMERA, count: 180);
            SpriteRenderer[] spriteRenderers = Circle_Text.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < 70; i++)
            {
                if (i > 30)
                {
                    for (int j = 0; j < spriteRenderers.Length; j++)
                    {
                        spriteRenderers[j].GetComponent<SpriteRenderer>().color = Color.Lerp(spriteRenderers[j].GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1f), 0.1f);
                    }
                    CenterCircle.GetComponent<SpriteRenderer>().color = Color.Lerp(CenterCircle.GetComponent<SpriteRenderer>().color, new Color(CenterCircle.GetComponent<SpriteRenderer>().color.r, 0, 0, 1f), 0.1f);
                }
                BackCircle.GetComponent<SpriteRenderer>().color = Color.Lerp(BackCircle.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1), 0.1f);
                yield return new WaitForSeconds(0.02f);
            }
            GameManager.Instance.playerBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.HIT);
            //GameManager.Instance.garbageBox.clean_all();
            gameObject.GetComponentInChildren<Animator>().enabled = true;
            //GameManager.Instance.anim_playing = false;
            CenterCircle.GetComponentInChildren<ParticleSystem>().Stop();
            yield return new WaitForSeconds(1f);
            GameManager.Instance.sub_managers.cameraManager.run_camera_anim(CameraAnimType.HIDESCREEN, 270, Vector3.zero, 0);
            GameManager.Instance.sub_managers.cameraManager.run_camera_anim(CameraAnimType.ZOOM_CAMERA, 120, Vector3.zero, 2);
            GetComponent<AudioSource>().Stop();
            yield return new WaitForSeconds(0.5f);
            if(GameManager.Instance.stagelv > 1)
            {
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene("MainMenu");
            }
            
            GameManager.Instance.sub_managers.cameraManager.CameraEffect_door.GetComponent<Animator>().SetBool("stageUp", true);
            yield return new WaitForSeconds(1f);
            GameManager.Instance.stagelv++;
            GameManager.Instance.stage_start(GameManager.Instance.stagelv);
            GameManager.Instance.roomMeta_now.node_list[2][2].object_here_list.Add(GameManager.Instance.playerBase);
            GameManager.Instance.playerBase.node_now = GameManager.Instance.roomMeta_now.node_list[2][2];
            GameManager.Instance.playerBase.ObjectBody.GetComponentInChildren<Animator>().SetInteger("MoveType", (int)AnimType.IDLE);
            GameManager.Instance.sub_managers.playerManager.cannotmove = false;
            
            
        }
    }

}
