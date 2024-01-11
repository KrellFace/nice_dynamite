using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public LayerMask obstacleLayer = 3; // Layer used for obstacles and walls
    public float spawnRadius = 100f; // Radius within which to spawn
    public float threatLevel = 0f;
    private float threatLevelMax = 100f;
    private float respawnTimer = 0f;
    private float threatLevelIncreaseTimer = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Vector3 spawnPoint = ;
        //
        // if (FindSpawnPoint(out spawnPoint))
        // {
        //     transform.position = spawnPoint;
        // }
        // else
        // {
        //     Debug.Log("No suitable location found to spawn.");
        // }
    }

    bool FindSpawnPoint(Vector3 point)
    {
        spawnRadius = 100 - threatLevel;
        for (int i = 0; i < 100; i++) // Try 100 times
        {
            //Vector3 randomPoint = player.transform.position + Random.insideUnitSphere * spawnRadius;

            // Check if the point is not intersecting with any obstacle
            if (!Physics.CheckSphere(point, 1f, obstacleLayer))
            {
                //check for ground underneath point
                
                // if (Physics.Raycast(point, Vector3.down))
                // {
                //     return true;
                // }
                //point = randomPoint;
                return true; // Suitable point found
            }
            else
            {
                Debug.Log(point + " intersecting with obstacle.");
            }
        }

        //point = Vector3.zero;
        return false; // Suitable point not found
    }

    private void FixedUpdate()
    {
        threatLevelIncreaseTimer += Time.deltaTime;
        if (threatLevelIncreaseTimer >= 10f)
        {
            threatLevel++;
            threatLevelIncreaseTimer = 0f;
        }
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= 6f)
        {
            Vector3 randomPoint = player.transform.position + Random.insideUnitSphere * spawnRadius;
            randomPoint.y = 1.5f;
            if (FindSpawnPoint(randomPoint))
            {
                randomPoint = player.transform.position + Random.insideUnitSphere * spawnRadius;
                randomPoint.y = 1.5f;
                transform.position = randomPoint;
            }
            respawnTimer =0f;

        }

        if (threatLevel >= threatLevelMax)
        {
            //TODO
        }
    }
}
