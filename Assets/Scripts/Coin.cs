using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject CoinPrefab;
    public float spawnRate = 2f;
    public float spawnX = 12f;
    public float spawnY = -3.5f;
    //public EndlessGround endless;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            SpawnCoin();
            timer = 0f;
        }
    }

    private void SpawnCoin()
    {
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
        GameObject coin = Instantiate(CoinPrefab, spawnPosition, Quaternion.identity);
        coin.name = "SpawnedCoin";
    }
}
