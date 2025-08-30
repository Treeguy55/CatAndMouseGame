using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{
    // Reference to the player object
    public Transform player;
    // Array of patrol waypoints
    public Transform[] patrolPoints;
    // Maximum distance the cat can see
    public float viewDistance = 999f; // Effectively unlimited
    // Angle of vision (360 = full circle)
    public float viewAngle = 360f;

    // NavMesh agent for pathfinding
    private NavMeshAgent agent;
    // Current patrol waypoint index
    private int currentPoint = 0;
    // Whether the cat is actively chasing the player
    private bool isChasing = false;
    // Tracks if the player is hidden (controlled by PlayerMovement script)
    private bool playerIsHidden = false;

    // Reference to game over UI for ending the game
    public GameOverUI gameOverUI;

    void Start()
    {
        // Grab the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        // Start patrolling from the first point if available
        if (patrolPoints.Length > 0)
            agent.destination = patrolPoints[0].position;
    }

    void Update()
    {
        // Stop movement if the game hasn't started
        if (!GameState.gameStarted)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        // Check if player is hidden via PlayerMovement script
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerIsHidden = playerMovement != null && playerMovement.isHidden;

        // If player is hidden, stop chasing and patrol
        if (playerIsHidden)
        {
            isChasing = false;

            // If close to destination, move to the next patrol point
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Patrol();
            }
            else
            {
                // Keep moving toward current patrol point
                agent.SetDestination(patrolPoints[currentPoint].position);
            }
            return;
        }

        // If cat can see the player, chase them
        if (CanSeePlayer())
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        // If already chasing but player is out of sight
        else if (isChasing)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // Stop chasing if player is very far away
            if (distance > viewDistance * 1.5f)
            {
                isChasing = false;
                agent.SetDestination(patrolPoints[currentPoint].position);
            }
            else
            {
                // Keep chasing last known player position
                agent.SetDestination(player.position);
            }
        }
        // Otherwise, continue patrolling
        else
        {
            Patrol();
        }
    }

    // Handles patrolling between waypoints
    void Patrol()
    {
        // Move to next patrol point when close enough to the current one
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    // Checks if the cat can see the player
    bool CanSeePlayer()
    {
        // If hidden, cat can’t see the player
        if (playerIsHidden)
            return false;

        Vector3 dirToPlayer = player.position - transform.position;

        // Raycast to check line of sight (prevents seeing through walls)
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, out hit, viewDistance))
        {
            return hit.transform == player;
        }

        return false;
    }

    // Trigger game over when colliding with the player
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerIsHidden)
        {
            gameOverUI.ShowGameOver();
        }
    }

    // Trigger game over when entering player’s collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerIsHidden)
        {
            gameOverUI.ShowGameOver();
        }
    }
}
