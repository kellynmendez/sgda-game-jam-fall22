using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatParticles : MonoBehaviour
{
    [SerializeField] PlayerMovement _player;

    ParticleSystem ps;

    List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        ps = transform.GetComponent<ParticleSystem>();
        _player = FindObjectOfType<PlayerMovement>();
        GetComponent<ParticleSystem>().trigger.SetCollider(0, _player.gameObject.transform);
    }

    

    private void OnParticleTrigger()
    {
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
        int outsideParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, _particles);

        for (int i = 0; i < outsideParticles; i++)
        {
            ParticleSystem.Particle p = _particles[i];
            //p.;
            _particles[i] = p;
        }

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            
            Debug.Log("RAT EXPLODE!!!!!! ");
            _particles[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

    }
}
