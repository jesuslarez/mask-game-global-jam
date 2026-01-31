using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform patrolPointsParent;
    private PatrolPoint[] patrolPoints;


    private int currentPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        patrolPoints = patrolPointsParent.GetComponentsInChildren<PatrolPoint>();
        GoToNextPoint();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPoint].position);
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
}