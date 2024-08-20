using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;
    public int hits;

    public bool dealsDamageOverTime;
    public bool destroyOnHit;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (dealsDamageOverTime)
            return;

        Damageable target = collision.GetComponentInParent<Damageable>();
        if(target != null) {target.Hit(damage); target.Knockback(collision.ClosestPoint(transform.position)); }

        if(destroyOnHit) Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(!dealsDamageOverTime)
            return;

        else if (dealsDamageOverTime)
        {
            Damageable target = collision.GetComponentInParent<Damageable>();
            if (target != null) StartCoroutine(DamageRepeating(target));
        }        
    }

    public IEnumerator DamageRepeating(Damageable target)
    {
        int currentHits = 0;
        while(currentHits < hits)
        {
            target.Hit(damage);
            currentHits++;
            yield return new WaitForSeconds(target.iFrames);
        }
    }
}
