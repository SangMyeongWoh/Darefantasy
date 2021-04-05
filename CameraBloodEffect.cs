using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBloodEffect : MonoBehaviour
{
    public List<Sprite> blood_sprite_list = new List<Sprite>();

    public IEnumerator bloodstain()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = blood_sprite_list[Random.Range(0, blood_sprite_list.Count)];
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        for (int i = 0; i < 60; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, new Color(1, 0, 0, 0), 0.1f);
            yield return new WaitForSeconds(0.02f);
        }

    }
}
