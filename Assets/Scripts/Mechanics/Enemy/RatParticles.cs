using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatParticles : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] PlayerAttack _atk;

    HealthManager _healthMngr;
    ParticleSystem ps;

    List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        ps = transform.GetComponent<ParticleSystem>();
        _player = FindObjectOfType<PlayerController>();
        _healthMngr = _player.GetComponent<HealthManager>();
        GetComponent<ParticleSystem>().trigger.SetCollider(0, _player.gameObject.transform);
    }

    private void OnParticleTrigger()
    {
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            _healthMngr.DecreaseHealth(1);
            //Debug.Log("RAT EXPLODE!!!!!! ");
            SoundManager.PlaySound("sfx_ratdeath5");
            _particles[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

    }
}
