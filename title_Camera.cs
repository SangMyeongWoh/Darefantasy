using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class title_Camera : MonoBehaviour
{
    public GameObject background;
    public GameObject backgorund2;
    int count = 0;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        background.transform.localPosition += new Vector3(Random.Range(-0.06f,0.06f),Random.Range(-0.06f,0.06f),0);
        background.transform.localPosition = Vector3.Lerp(background.transform.localPosition, Vector3.zero, 0.3f);
        if(count > 30)
            backgorund2.GetComponent<SpriteRenderer>().color = Color.Lerp(backgorund2.GetComponent<SpriteRenderer>().color, new Color(0, 0f, 1, 0.1f), 0.002f);
        count++;
    }
}
