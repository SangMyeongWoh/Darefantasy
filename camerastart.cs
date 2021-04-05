using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerastart : MonoBehaviour
{
    public GameObject RedPortalOpenEffect;
    public GameObject RedPortalCloseEffect;
    public GameObject CloseDoor;
    public GameObject OpenDoor;
    public GameObject title;
    IEnumerator coroutine;

    private void OnEnable()
    {
        coroutine = ZoomCamera();
        StartCoroutine(coroutine);
    }
    public void stopCoroutine()
    {
        StopCoroutine(coroutine);
        GetComponent<Camera>().orthographicSize = 5;
        transform.position = new Vector3(0, 0, -10);
    }
    IEnumerator ZoomCamera()
    {
        for (int i = 0; i < 720; i++)
        {
            if(i < 227)
            {
                gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 2.5f, -10), 0.001f);
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 1, 0.001f);
            }
            
            if(i > 85 && i < 90)
            {
                gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 2.5f, -10), 0.1f);
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 1, 0.1f);
            } else if (i > 150 && i < 155)
            {
                gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 2.5f, -10), 0.1f);
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 1, 0.1f);

            } else if (i == 175)
                RedPortalOpenEffect.GetComponent<ParticleSystem>().Play();

            else if (i == 195)
            {
                RedPortalCloseEffect.GetComponent<ParticleSystem>().Play();
                
            } else if(i == 225)
            {
                CloseDoor.GetComponent<Animator>().SetBool("start", true);
            } else if(i > 227)
            {
                OpenDoor.SetActive(false);
                gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0f, -10), 0.5f);
                GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, 7, 0.03f);
                title.transform.localScale = Vector3.Lerp(title.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), 0.01f);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

}
