using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float damage = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        GameObject cObject = collision.gameObject;
        if (cObject.tag == "Rat" || cObject.tag == "Destructible")
        {
            Health cHealth = cObject.GetComponent<Health>();
            cHealth.TakeDmg(damage);
        }


    }
}
