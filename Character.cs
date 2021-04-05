using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject CannotAttack;
    // Start is called before the first frame update
    public void cannot_attack()
    {
        CannotAttack.transform.localScale = new Vector3(GetComponent<ObjectBody>().body.transform.localScale.x, 1, 1);
        CannotAttack.transform.localPosition = new Vector3(0, 0.5f, 0);
        StartCoroutine(cannotattack());
    }

    IEnumerator cannotattack()
    {
        CannotAttack.SetActive(true);
        for (int i = 0; i < 45; i++)
        {
            CannotAttack.transform.localPosition = Vector3.Lerp(CannotAttack.transform.localPosition, new Vector3(0, 1.5f, 0), 0.1f);
            CannotAttack.GetComponent<SpriteRenderer>().color = Color.Lerp(CannotAttack.GetComponent<SpriteRenderer>().color, new Color(1, 0, 0, 1), 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        CannotAttack.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        CannotAttack.SetActive(false);
    }

}
