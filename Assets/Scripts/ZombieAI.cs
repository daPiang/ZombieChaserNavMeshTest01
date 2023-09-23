using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    // [SerializeField] private Rigidbody rb;
    [SerializeField] private PatrolPoints patrolPoints;
    [SerializeField] private Transform player;
    [SerializeField] private float sprintMul;
    private float multiplier = 1;
    private Transform currentPoint;
    private Vector3 zombieFlat;
    private Vector3 waypointFlat;
    private Vector3 playerFlat;
    private enum ZombieState
    {
        Patrol,
        Chase
    }
    private ZombieState zombieState;
    [SerializeField] private Animator anims;
    [SerializeField] private Transform rayOrigin, rayGoal;

    private void Start() {
        currentPoint = patrolPoints.GetNextWaypoint(currentPoint);
        zombieState = ZombieState.Patrol;
    }

    private void Update() {
        agent.speed = 0.5f * multiplier;

        Debug.Log(zombieState);

        GetFlats();
        HandleAnims();
        CastToPlayer();

        switch(zombieState)
        {
            case ZombieState.Patrol:
                Patrol();
                break;
            case ZombieState.Chase:
                Chase();
                break;
        }
    }

    private void Patrol()
    {
        if(currentPoint != null)
        {   
            agent.SetDestination(currentPoint.position);
        }

        if(zombieFlat == waypointFlat)
        {
            currentPoint = patrolPoints.GetNextWaypoint(currentPoint);
        }

        multiplier = 1;
    }

    private void Chase()
    {
        agent.SetDestination(playerFlat);

        multiplier = sprintMul;
    }

    private void GetFlats()
    {
        zombieFlat = new(
            transform.position.x,
            0,
            transform.position.z
        );

        waypointFlat = new(
            currentPoint.position.x,
            0,
            currentPoint.position.z
        );

        playerFlat = new(
            player.position.x,
            0,
            player.position.z
        );
    }

    private void HandleAnims()
    {
        anims.SetBool("isMoving", IsMoving());
        anims.SetBool("isRunning", IsSprinting());
    }

    private bool IsMoving()
    {
        return agent.velocity != Vector3.zero;
    }

    private bool IsSprinting()
    {
        return multiplier != 1;
    }

    private void CastToPlayer()
    {
        Ray ray = new(rayOrigin.position, rayGoal.position - rayOrigin.position);
        float maxDistance = Vector3.Distance(rayOrigin.position, rayGoal.position);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);

            if(hit.transform.CompareTag("Player"))
            {
                zombieState = ZombieState.Chase;
            }
            else zombieState = ZombieState.Patrol;
        }
    }
}
