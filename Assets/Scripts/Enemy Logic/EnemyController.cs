using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationCompleteThreshold = 5f;

    [Header("Patrol Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waypointArriveThreshold = 0.1f;
    [SerializeField, Range(0f, 1f)] private float moveReverseWaypointChance = 0.05f;

    [Header("TV Interaction")]
    [SerializeField] private TVController tvController;
    [SerializeField] private float tvStopDistance = 1f;

    [Header("Spawnable Detection")]
    [SerializeField] private LayerMask spawnableLayer;
    [SerializeField] private float detectionCooldown = 1.5f;
    private float detectionTimer = 0f;

    [Header("Forward Box Detection")]
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1.5f, 1f, 3f);
    [SerializeField] private Vector3 boxOffset = new Vector3(0f, 0f, 2f);

    private Transform announcementTarget;
    private int currentWaypointIndex = 0;
    private int waypointDirection = 1;

    private enum State { Patrol, MoveToTV, WaitAtTV, MoveToAnnouncement }
    private State currentState = State.Patrol;

    private void Update()
    {
        if (currentState == State.Patrol && tvController != null && tvController.IsOpen())
        {
            currentState = State.MoveToTV;
        }

        if (currentState == State.WaitAtTV && tvController != null && !tvController.IsOpen())
        {
            currentState = State.Patrol;
        }

        switch (currentState)
        {
            case State.Patrol:
                HandlePatrol();

                detectionTimer -= Time.deltaTime;
                if (detectionTimer <= 0f)
                {
                    ScanForSpawnables();
                    detectionTimer = detectionCooldown;
                }
                break;

            case State.MoveToTV:
                if (tvController != null)
                {
                    MoveTowards(
                        tvController.tvFrontPoint,
                        waypointArriveThreshold,
                        onArrive: () => { currentState = State.WaitAtTV; }
                    );
                }
                break;

            case State.WaitAtTV:
                FaceIngredient(tvController.transform);
                break;

            case State.MoveToAnnouncement:
                if (announcementTarget != null)
                {
                    MoveTowards(
                        announcementTarget,
                        waypointArriveThreshold,
                        onArrive: () => { currentState = State.Patrol; }
                    );
                }
                break;
        }
    }

    private void HandlePatrol()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Transform targetWp = waypoints[currentWaypointIndex];
        MoveTowards(
            targetWp,
            waypointArriveThreshold,
            onArrive: () =>
            {
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
            }
        );
    }

    private void MoveTowards(Transform target, float arriveThreshold, System.Action onArrive)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        float dist = dir.magnitude;
        if (dir != Vector3.zero)
        {
            dir.Normalize();
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);

            float angleDiff = Quaternion.Angle(transform.rotation, lookRot);
            if (angleDiff < rotationCompleteThreshold)
            {
                if (dist > arriveThreshold)
                {
                    transform.position += dir * moveSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
                    onArrive?.Invoke();
                }
            }
        }
    }

    private void FaceIngredient(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            dir.Normalize();
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
        }
    }

    private void ScanForSpawnables()
    {
        Vector3 center = transform.position + transform.rotation * boxOffset;
        Collider[] hits = Physics.OverlapBox(center, boxHalfExtents, transform.rotation, spawnableLayer);

        foreach (Collider hit in hits)
        {
            Debug.Log("Düşman önünde hata buldu! Nesne: " + hit.name);
        }
    }

    public void TriggerAnnouncement(Transform announcementWaypoint) //bu özelliği ekle
    {
        announcementTarget = announcementWaypoint;
        currentState = State.MoveToAnnouncement;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + transform.rotation * boxOffset;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2f);
    }
}
