using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;
    public float visionRange = 10f;
    public float visionAngle = 60f;

    private NavMeshAgent agent;
    private int currentPoint = 0;
    private bool isChasing = false;

    public float fieldOfView = 90f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPoint].position);
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            // Lost sight of player, go back to patrolling
            isChasing = false;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }

        if (!isChasing && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance > visionRange)
            return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > visionAngle / 2f)
            return false;

        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, dirToPlayer.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, visionRange))
        {
            return hit.transform == player;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        // Vision range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Vision cone
        Vector3 forward = transform.forward * visionRange;
        float halfFOV = fieldOfView / 2f;

        Quaternion leftRayRotation = Quaternion.Euler(0, -halfFOV, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, halfFOV, 0);

        Vector3 leftRay = leftRayRotation * forward;
        Vector3 rightRay = rightRayRotation * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
    }
}
