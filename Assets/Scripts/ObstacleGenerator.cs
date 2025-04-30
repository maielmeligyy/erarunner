using System.Collections;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnRate = 2f;
    public float spawnX = 10f;
    public float spawnY = -2.5f;
    public EndlessGround endless;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    private void SpawnObstacle()
    {
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        Debug.Log("obstacle spawned at: " + spawnPosition);
    }
}
