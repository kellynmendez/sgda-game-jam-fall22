using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject deathEffect;
    public GameObject hurtEffect;
    public float invincibilityDuration = 0.2f;
    public float hp = 1;

    bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead())
            Die();
    }

    void Die()
    {
        SoundManager.PlaySound("character_death");
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Returns if the damage killed this instance
    public bool TakeDmg(float dmg)
    {
        SoundManager.PlaySound("character_hurt");
        if (!isInvincible)
            hp = hp - dmg;
        if (!IsDead() && hurtEffect != null)
            Instantiate(hurtEffect, transform.position, Quaternion.identity);
        return IsDead();
    }

    public bool IsDead()
    {
        return hp <= 0;
    }
}
