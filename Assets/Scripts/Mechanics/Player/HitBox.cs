using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    CircleCollider2D hitCollider;

    // Start is called before the first frame update
    void Start()
    {
        hitCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
