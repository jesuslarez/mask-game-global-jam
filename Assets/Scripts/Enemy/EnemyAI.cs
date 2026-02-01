using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum Mode { Chase, Patrol }

    public NavMeshAgent agent;
    public float lookingDistance;
    public float lookingTimeout;
    public float currentLookingTimeout;
    private Transform patrolPointsParent;
    private PatrolPoint[] patrolPoints;
    private Mode currentMode = Mode.Patrol;

    private int currentPoint;
    private Vector3 nextPosition;
    private PlayerController player;

    private bool isStunned = false; // Flag to check if the enemy is stunned

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        patrolPointsParent = GameObject.FindGameObjectWithTag("PatrolPoint").transform;
        patrolPoints = patrolPointsParent.GetComponentsInChildren<PatrolPoint>();
        currentPoint = Random.Range(0, patrolPoints.Length - 1);
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
        //StopCoroutine();
    }

    public void StartPatrol()
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

    public void Stun(float duration)
    {
        if (isStunned) return; // Prevent multiple stuns

        isStunned = true;
        Debug.Log($"{gameObject.name} is stunned!");

        // Store the original speed of the NavMeshAgent
        float originalSpeed = agent.speed;

        // Set the speed to 0 to simulate the stun effect
        agent.speed = 0.1f;
        
        // Start a coroutine to restore the speed after the stun duration
        StartCoroutine(UnstunAfterDelay(duration, originalSpeed));
    }

    private IEnumerator UnstunAfterDelay(float duration, float originalSpeed)
    {
        yield return new WaitForSecondsRealtime(duration);

        isStunned = false;
        Debug.Log($"{gameObject.name} is no longer stunned!");

        // Restore the original speed of the NavMeshAgent
        agent.speed = 2.5f;
        agent.isStopped = false;
        StartPatrol();
        Debug.Log($"unstun: speed={agent.speed} stopped={agent.isStopped} enabled={agent.enabled} hasPath={agent.hasPath}");

    }
}