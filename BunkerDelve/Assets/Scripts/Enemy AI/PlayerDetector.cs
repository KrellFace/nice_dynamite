using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDetector : MonoBehaviour
{

    public float threatLevel;
    public float threatLevelMax;
    public float threatLevelIncreaseInterval;
    public float speed;
    public float respawnInterval;

    private NavMeshAgent agent;

    
    private GameObject player;
    private float threatTimer;
    private float respawnTimer;
    private Vector3 moveTarget;
    
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        player = GameObject.FindGameObjectWithTag("Player");
        threatLevel = 0;
        moveTarget = RandomNavmeshLocation();

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
         var respawnTarget = centerOfRadius + (Vector3)(radius * Random.insideUnitCircle);
        // //check if the point is within the navmesh
        // NavMeshHit hit;
        // while (!NavMesh.SamplePosition(target, out hit, 0, 1))
        // {
        //     target = centerOfRadius + (Vector3)(radius * Random.insideUnitCircle);
        // }
        //move the monster to the point
        agent.destination = moveTarget;


        if (respawnTimer >= respawnInterval)
        {
            moveTarget = RandomNavmeshLocation();
            transform.position = respawnTarget;
            respawnTimer = 0;
        }

        if (threatLevel >= threatLevelMax)
        {
            //TODO
        }
            
        
    }
    
    public Vector3 RandomNavmeshLocation() {
        var radius = 100 - threatLevel;

        //Vector3 randomDirection = Random.insideUnitSphere * radius;
        //randomDirection  += transform.position;
        var centerOfRadius = player.transform.position;
        var source = centerOfRadius + (Vector3)(radius * Random.insideUnitCircle);

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(source, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }
}
