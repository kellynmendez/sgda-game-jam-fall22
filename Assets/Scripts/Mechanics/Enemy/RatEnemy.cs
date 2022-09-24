using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatEnemy : MonoBehaviour
{
    private Transform _target;
    private NavMeshAgent _agent;
    private const float _chaseDelayTime = 0.2f;

    void Start()
    {
        // Finding player transform
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        // Updating the target destination agent should be moving toward
        StartCoroutine(UpdateDestination());

        #region NavMeshPro required lines for nav mesh agent
        var agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        #endregion
    }

    private IEnumerator UpdateDestination()
    {
        _agent.SetDestination(_target.position);
        yield return new WaitForSeconds(_chaseDelayTime);
    }
}
