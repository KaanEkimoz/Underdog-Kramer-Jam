using UnityEngine;

public class MicController : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private Transform announcementWaypoint; // Inspector’dan ver
    public KeyCode announceKey = KeyCode.E;

}
