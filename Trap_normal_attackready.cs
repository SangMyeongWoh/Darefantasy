using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_normal_attackready : MonoBehaviour
{
    public GameObject subSprite;
    // Start is called before the first frame update
    
    public void activeattack()
    {
        StartCoroutine(ready());
    }
    public void unactiveattack()
    {
        StartCoroutine(notready());
    }

    public IEnumerator notready()
    {
        for (int i = 0; i < 20; i++)
        {
            subSprite.GetComponent<SpriteRenderer>().color = Color.Lerp(subSprite.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 1), 0.3f);
            yield return new WaitForSeconds(0.02f);
        }

    }
    public IEnumerator ready()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 20; i++)
        {
            subSprite.GetComponent<SpriteRenderer>().color = Color.Lerp(subSprite.GetComponent<SpriteRenderer>().color, new Color(1, 1, 1, 0), 0.3f);
            yield return new WaitForSeconds(0.02f);
        }

    }
}
