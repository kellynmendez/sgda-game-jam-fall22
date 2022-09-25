using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigAnimationScript : MonoBehaviour
{

    [SerializeField] ParticleSystem trash;

    public void DigEvent(int s)
    {

        Instantiate(trash, new Vector3(this.transform.position.x, this.transform.position.y , this.transform.position.z - 0.5f), Quaternion.identity);
        SoundManager.PlaySound("sfx_ratdeath");
    }
}
