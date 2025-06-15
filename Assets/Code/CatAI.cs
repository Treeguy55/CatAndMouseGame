using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;
    public float viewDistance = 10f;
    public float viewAngle = 60f;

    private NavMeshAgent agent;
    private int currentPoint = 0;
    private bool isChasing = false;
    private bool playerIsHidden = false;

    public GameOverUI gameOverUI;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
            agent.destination = patrolPoints[0].position;
    }

    void Update()
    {
        if (!GameState.gameStarted)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        // Update hidden status from player component
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerIsHidden = playerMovement != null && playerMovement.isHidden;

        if (playerIsHidden)
        {
            // Player is hidden: stop chasing and return to patrol
            isChasing = false;
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Patrol();
            }
            else
            {
                agent.SetDestination(patrolPoints[currentPoint].position);
            }
            return;
        }

        // Normal chase/patrol behavior
        if (CanSeePlayer())
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > viewDistance * 1.5f)
            {
                isChasing = false;
                agent.SetDestination(patrolPoints[currentPoint].position);
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    bool CanSeePlayer()
    {
        if (playerIsHidden)
            return false;

        Vector3 dirToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (dirToPlayer.magnitude < viewDistance && angle < viewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, out hit, viewDistance))
            {
                return hit.transform == player;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerIsHidden)
        {
            gameOverUI.ShowGameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIsHidden)
        {
            gameOverUI.ShowGameOver();
        }
    }
}