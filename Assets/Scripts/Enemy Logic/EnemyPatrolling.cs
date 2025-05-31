using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationCompleteThreshold = 5f;
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waypointArriveThreshold = 0.1f;
    [SerializeField, Range(0f, 1f)] private float moveReverseWaypointChance = 0.05f;

    private int currentIndex = 0;
    private int direction = 1; // 1 = ileri, -1 = geri

    void Update()
    {
        if (waypoints.Length < 2) return;

        Transform target = waypoints[currentIndex];
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0f; // Y ekseni sabit
        float distanceToTarget = directionToTarget.magnitude;
        directionToTarget.Normalize();

        if (directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Rotasyonun hedefe yakýn olup olmadýðýný kontrol et
            float angleDiff = Quaternion.Angle(transform.rotation, lookRotation);

            // Eðer yeterince dönmüþse hareket et
            if (angleDiff < rotationCompleteThreshold)
            {
                if (distanceToTarget > waypointArriveThreshold)
                {
                    transform.position += directionToTarget * moveSpeed * Time.deltaTime;
                }
                else
                {
                    // Hedefe çok yaklaþtýysa pozisyonu direkt hedefe ayarla
                    transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);

                    // Sonraki hedefe geç
                    if (Random.value < moveReverseWaypointChance)
                    {
                        direction *= -1;
                    }

                    currentIndex += direction;

                    if (currentIndex >= waypoints.Length)
                    {
                        currentIndex = waypoints.Length - 2;
                        direction = -1;
                    }
                    else if (currentIndex < 0)
                    {
                        currentIndex = 1;
                        direction = 1;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}