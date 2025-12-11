using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    void Start()
    {
        int obstacleCount = Random.Range(1, 4);
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(5f, 25f));
            Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], pos, Quaternion.identity, transform);
        }
    }
}
