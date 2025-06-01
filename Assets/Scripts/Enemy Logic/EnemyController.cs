using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Effects")]
    public GameObject happyParticles;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;

    [Header("Patrol Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField, Range(0f, 1f)] private float moveReverseWaypointChance = 0.05f;

    [Header("TV Interaction")]
    [SerializeField] private TVController tvController;
    [SerializeField] private float tvStopDistance = 1f;

    [Header("Spawnable Detection")]
    [SerializeField] private LayerMask spawnableLayer;
    public LayerMask shelfLayer;
    [SerializeField] private float detectionCooldown = 1.5f;
    private float detectionTimer = 0f;

    [Header("Forward Box Detection")]
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1.5f, 1f, 3f);
    [SerializeField] private Vector3 boxOffset = new Vector3(0f, 0f, 2f);

    private NavMeshAgent agent;
    private Transform announcementTarget;
    private int currentWaypointIndex = 0;
    private int waypointDirection = 1;
    private bool isWatchingTV = false;

    private enum State { Patrol, MoveToTV, WaitAtTV, MoveToAnnouncement }
    private State currentState = State.Patrol;

    private void Awake()
    {
        happyParticles.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        // --- Durum geçişleri ---
        if (currentState == State.Patrol && tvController != null && tvController.IsOpen())
        {
            currentState = State.MoveToTV;
            agent.SetDestination(tvController.tvFrontPoint.position);
        }

        if (currentState == State.WaitAtTV && tvController != null && !tvController.IsOpen())
        {
            currentState = State.Patrol;
            GoToNextWaypoint();
        }

        // --- Durum işlemleri ---
        switch (currentState)
        {
            case State.Patrol:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    GoToNextWaypoint();
                }

                detectionTimer -= Time.deltaTime;
                if (detectionTimer <= 0f)
                {
                    ScanForSpawnables();
                    detectionTimer = detectionCooldown;
                }
                break;

            case State.MoveToTV:
                if (!agent.pathPending && agent.remainingDistance <= tvStopDistance)
                {
                    currentState = State.WaitAtTV;
                    agent.ResetPath();
                }
                break;

            case State.WaitAtTV:
                isWatchingTV = true;
                happyParticles.SetActive(true);
                FaceTarget(tvController.transform);
                break;

            case State.MoveToAnnouncement:
                if (announcementTarget != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = State.Patrol;
                    GoToNextWaypoint();
                }
                break;
        }

        if (currentState != State.WaitAtTV && happyParticles.activeSelf)
        {
            happyParticles.SetActive(false);
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        if (Random.value < moveReverseWaypointChance)
            waypointDirection *= -1;

        currentWaypointIndex += waypointDirection;

        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = waypoints.Length - 2;
            waypointDirection = -1;
        }
        else if (currentWaypointIndex < 0)
        {
            currentWaypointIndex = 1;
            waypointDirection = 1;
        }

        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void FaceTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    private void ScanForSpawnables()
    {
        Vector3 center = transform.position + transform.rotation * boxOffset;
        Collider[] hits = Physics.OverlapBox(center, boxHalfExtents, transform.rotation, spawnableLayer);

        foreach (Collider hit in hits)
        {
            Debug.Log("Spawnable tespit edildi: " + hit.name);
        }
    }

    private void ChechShelves()
    {
        Vector3 center = transform.position + transform.rotation * boxOffset;
        Collider[] hits = Physics.OverlapBox(center, boxHalfExtents, transform.rotation, shelfLayer);

        foreach (Collider hit in hits)
        {
            
            if (hit.TryGetComponent<ShelfDetector>(out ShelfDetector detector))
            {
                if (!detector.HasCorrectPickupable) 
                {
                    Debug.Log("boss kizdi");

                }

            }
        }
    }

    public void TriggerAnnouncement(Transform announcementWaypoint)
    {
        announcementTarget = announcementWaypoint;
        currentState = State.MoveToAnnouncement;
        agent.SetDestination(announcementTarget.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + transform.rotation * boxOffset;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
    }
}
