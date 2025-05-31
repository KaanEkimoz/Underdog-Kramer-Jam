using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public Vector3 GetRandomPoint()
    {
        var bounds = GetComponent<Collider>().bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.center.y;

        return new Vector3(x, y, z);
    }
 
}
