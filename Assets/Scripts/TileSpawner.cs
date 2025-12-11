using GoogleMobileAds.Api;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform player;
    public int numberOfTiles = 5;
    public float tileLength = 30f;

    private float spawnZ = 0f;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player.position.z - tileLength > (spawnZ - tileLength * numberOfTiles))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(tilePrefab, Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
