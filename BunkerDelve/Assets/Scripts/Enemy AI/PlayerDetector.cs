using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDetector : MonoBehaviour
{

    public Vector3 targetPosition;

    private Seeker seeker;
    private CharacterController controller;
    private GameObject player;

    private script_GameEndedUI gameEndedUI;

    public Path path;

    public float threatLevel = 0;
    private float threatLevelMax = 20;
    private float threatLevelIncreaseTimer = 0;
    private float gridRefreshTimer = 0;
    private float targetChangeTimer = 0;
    private bool spawnedIn = false;
    public bool enemyActive = false;
    private float despawnTimer = 0f;
    private float respawnTimer = 0f;

    public float speed = 2;
    public float rotationSpeed = 2f;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    public void Start () {
        seeker = GetComponent<Seeker>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPosition = player.transform.position;

        gameEndedUI = FindObjectOfType<script_GameEndedUI>();

        controller = GetComponent<CharacterController>();

        
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
    }

    public void OnPathComplete (Path p) {
        //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void FixedUpdate () {
        if (path == null) {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;

        // Move the agent using the CharacterController component
        controller.SimpleMove(velocity);
        
        threatLevelIncreaseTimer += Time.deltaTime;
        gridRefreshTimer += Time.deltaTime;

        //Spawn enemy in after set delay
        if(enemyActive&&!spawnedIn){
            respawnTimer += Time.deltaTime;
            if(respawnTimer >= 15f)
            {
                SpawnEnemy();
                respawnTimer = 0f;
                spawnedIn = true;
            }
        }

        //While spawned in, constantly track player. Despawn after timer so long as player is far enough away
        if(spawnedIn){
            targetChangeTimer += Time.deltaTime;
            if(targetChangeTimer >= 3f)
            {
                //targetPosition = player.transform.position + (Vector3)((100 - threatLevel) * Random.insideUnitCircle);
                targetPosition = player.transform.position;
                seeker.StartPath(transform.position, targetPosition, OnPathComplete);
                targetChangeTimer = 0f;
            }
            despawnTimer+=Time.deltaTime;
            if(despawnTimer>=12f&&Vector3.Distance(this.transform.position, player.transform.position)>15f){
                DespawnEnemy();
                despawnTimer = 0f;
                spawnedIn = false;
            }
        }

        /*
        if(respawnTimer >= 20f)
        {
            var data = AstarPath.active.data;
            //respawn the agent somewhere on the grid within a certain distance of the player
            //var pos = player.transform.position + (Vector3)(20 * Random.insideUnitCircle);
            var pos = player.transform.position + (Vector3)((100 - threatLevel) * Random.insideUnitCircle.normalized);
            

            var node = data.gridGraph.GetNearest(pos, NNConstraint.None).node;
            transform.position = (Vector3)node.position;
            respawnTimer = 0f;
        }
        */


        if (threatLevelIncreaseTimer >= 10f&&threatLevel<threatLevelMax)
        {
            threatLevel++;

            threatLevelIncreaseTimer = 0f;
            //Debug.Log("Threat level:" + threatLevel);
        }
        if(gridRefreshTimer >= 4f)
        {
              AstarPath.active.Scan();
           // Bounds bounds = GetComponent<Collider>().bounds;
           // AstarPath.active.UpdateGraphs(bounds);
           //Debug.Log("updated graph");
            gridRefreshTimer = 0f;
        }
        var direction = (player.transform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


    }


    private void SpawnEnemy(){
        var data = AstarPath.active.data;
        //respawn the agent somewhere on the grid within a certain distance of the player
        //var pos = player.transform.position + (Vector3)(20 * Random.insideUnitCircle);
        var pos = player.transform.position + (Vector3)((30 - threatLevel) * Random.insideUnitCircle.normalized);
        var node = data.gridGraph.GetNearest(pos, NNConstraint.None).node;
        transform.position = (Vector3)node.position;

        Debug.Log("Spawning enemy at pos " + pos + " with threat " + threatLevel + " and player pos: " + player.transform.position);
    }

    private void DespawnEnemy(){
        transform.position = new Vector3(0,-50,0);
    }

    public void SetEnemyActive(){
        enemyActive = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Game Over!");
            gameEndedUI.ActivateEndScreen(false);
        }
    }

}
