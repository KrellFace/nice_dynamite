using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{

    public float threatLevel;
    public float threatLevelMax;
    public float threatLevelIncreaseInterval;
    public float speed;
    public float respawnInterval;

    
    private GameObject player;
    private float threatTimer;
    private float respawnTimer;
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        threatLevel = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        threatTimer += Time.deltaTime;
        respawnTimer += Time.deltaTime;
        if(threatTimer >= threatLevelIncreaseInterval)
        {
            threatLevel += 1;
            threatTimer = 0;
        }
        
        //spawn the monster in a certain radius of the player

        //pick a point within a radius of 100 - threatLevel from the player
        var centerOfRadius = player.transform.position;
        var radius = 100 - threatLevel;
        var target = centerOfRadius + (Vector3)(radius * Random.insideUnitCircle);


        if (respawnTimer >= respawnInterval)
        {
            transform.position = target;
            respawnTimer = 0;
        }
            
        
    }
}
