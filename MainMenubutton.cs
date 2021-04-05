using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenubutton : MonoBehaviour
{
    public int count = 0;
    public GameObject SpriteObject;
    public GameObject backword;
    public GameObject hidescreen;
    public GameObject title;

    bool startgame;
    private void FixedUpdate()
    {
        if (count > 0)
            count--;
        if(count > 4 && !startgame)
        {
            SpriteObject.transform.localScale = Vector3.Lerp(SpriteObject.transform.localScale, new Vector3(1.3f, 1.3f, 1), 0.4f);
            SpriteObject.GetComponent<SpriteRenderer>().color = Color.Lerp(SpriteObject.GetComponent<SpriteRenderer>().color, new Color(0,0.6f,1), 0.2f);            
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0.2f, 0.05f);
            //SpriteObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            backword.SetActive(true);
            //can transition
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(StartGame());
                startgame = true;
            }
        }
        else
        {
            backword.SetActive(false);
            SpriteObject.transform.localScale = Vector3.Lerp(SpriteObject.transform.localScale, new Vector3(1f, 1f, 1), 0.4f);
            SpriteObject.GetComponent<SpriteRenderer>().color = Color.Lerp(SpriteObject.GetComponent<SpriteRenderer>().color, new Color(0.7f, 0.7f, 0.7f), 0.2f);
            //SpriteObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0f, 0.05f);
        }
    }
    IEnumerator StartGame()
    {        
        for(int i = 0; i < 60; i++)
        {
            hidescreen.GetComponent<SpriteRenderer>().color = Color.Lerp(hidescreen.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1), 0.1f);
            title.GetComponent<AudioSource>().volume = Mathf.Lerp(title.GetComponent<AudioSource>().volume, 0, 0.1f);
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0f, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene("GameStart");

    }
    private void OnMouseOver()
    {
        if (count < 10)
            count += 2;
    }
}
