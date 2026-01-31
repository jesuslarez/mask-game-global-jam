using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyAI : MonoBehaviour
{
    public enum Mode { Chase, Patrol }

    public NavMeshAgent agent;
    public Transform patrolPointsParent;
    public float lookingDistance;
    public float lookingTimeout;
    public float currentLookingTimeout;
    private PatrolPoint[] patrolPoints;
    private Mode currentMode = Mode.Patrol;

    private int currentPoint;
    private Vector3 nextPosition;
    private PlayerController player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        patrolPoints = patrolPointsParent.GetComponentsInChildren<PatrolPoint>();

        StartPatrol();
    }

    private void Update()
    {
        LookForPlayer();

        GoToNextPoint();
    }

    private void LookForPlayer()
    {
        Vector3 distanceToPlayer = (player.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, distanceToPlayer, lookingDistance);

        if (hit.collider == null)
        {
            Debug.DrawRay(transform.position, distanceToPlayer * lookingDistance, Color.red);
            return;
        }

        if (hit.collider.TryGetComponent(out PlayerController playerController))
        {
            if (!IsChasing())
            {
                StartChasing();
            }

            currentLookingTimeout = lookingTimeout;

            Debug.DrawRay(transform.position, distanceToPlayer * lookingDistance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, distanceToPlayer * lookingDistance, Color.red);
        }
    }

    private void StartChasing()
    {
        currentMode = Mode.Chase;
        StartCoroutine(CheckForPlayer());
        GoToNextPoint();
    }

    private IEnumerator CheckForPlayer()
    {
        while (currentLookingTimeout > 0)
        {
            yield return new WaitForSeconds(lookingTimeout);
        }

        StartPatrol();
        StopAllCoroutines();
    }

    private void StartPatrol()
    {
        currentMode = Mode.Patrol;
        GoToNextPoint();
    }

    void GoToNextPoint()
    {
        if (IsChasing())
        {
         
            nextPosition = player.transform.position;
            currentLookingTimeout -= Time.deltaTime;

        }

        if (IsPatrolling())
        {
            if (patrolPoints.Length == 0)
            {
                Debug.LogError("No existen puntos de patrulla");
                return;
            }

            if (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                return;
            }

            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            nextPosition = patrolPoints[currentPoint].Position;
        }

        agent.SetDestination(nextPosition);
    }

    private bool IsPatrolling() => currentMode.Equals(Mode.Patrol);
    private bool IsChasing() => currentMode.Equals(Mode.Chase);
}