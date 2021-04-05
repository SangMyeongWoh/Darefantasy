using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderSetter : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    public ParticleSystemRenderer[] particleSystems;

    Vector3 last_pos;
    bool ready_to_change_order;
    /// <summary>
    /// 
    /// </summary>

    void Start()
    {        
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        particleSystems = GetComponentsInChildren<ParticleSystemRenderer>();
        set_sorting_order();
        ready_to_change_order = true;
    }

    private void OnEnable()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        particleSystems = GetComponentsInChildren<ParticleSystemRenderer>();
        change_sorting_order(gameObject.transform.position - last_pos);
        
    }
    private void OnDisable()
    {
        last_pos = gameObject.transform.position;
    }

    public void set_sorting_order()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i])
            {
                if (spriteRenderers[i].sortingOrder != Constant.FLOORSPRITESORTINGORDER && spriteRenderers[i].sortingOrder > -30000 && spriteRenderers[i].sortingOrder < 30000)
                    spriteRenderers[i].sortingOrder += 7000 - (int)(gameObject.transform.position.y * 1000);
            }
                
            

        }
        for(int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i])
                particleSystems[i].sortingOrder += 7000 - (int)(gameObject.transform.position.y * 1000);
        }
    }
    

    public void change_sorting_order(Vector3 distance)
    {
        if (ready_to_change_order)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i])
                    if (spriteRenderers[i].sortingOrder > -30000 && spriteRenderers[i].sortingOrder < 30000) spriteRenderers[i].sortingOrder += -(int)(distance.y * 1000);
                
            }
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i])
                    particleSystems[i].sortingOrder += -(int)(distance.y * 1000);

            }
        }
        
    }


}
