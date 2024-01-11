using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;


public class script_Room : MonoBehaviour
{
    //GAME OBJECTS
    private script_RoomManager roomManager;
    public Transform[] goalObjectLocations;
    //private script_RoomDressingManager roomDressingManager;

    //FIXED VARIABLES
    public enum_RoomTypes roomType;

    //DYNAMIC VARIABLES
    private int[] loc;
    private enum_Directions exitBlocked = enum_Directions.NONE;

    private void Start() {
        roomManager = FindObjectOfType<script_RoomManager>();
        //roomDressingManager = FindObjectOfType<script_RoomDressingManager>();
        Debug.Log("Start run");
    }

    public void DressRoom(enum_Directions cameFrom, script_RoomDressingManager roomDressingManager){
        //Debug.Log(roomManager);
        //Debug.Log(roomDressingManager);

        //SET DRESSING
        GameObject dressing = roomDressingManager.GetDressingForRoomType(roomType);
        if(dressing!=null){
            dressing.transform.parent = this.transform;
            dressing.transform.localPosition = new Vector3(0,0,0);

        }
        
        //DOOR BLOCK
        bool spawnBlock = false;
        Vector3 spawnBlockLoc = new Vector3(0,1,0);
        float dirBlockChooser = Random.Range(0f, 1f);
        if(dirBlockChooser<0.2f&&cameFrom!=enum_Directions.NORTH){
            exitBlocked = enum_Directions.NORTH;
            spawnBlock = true;
            spawnBlockLoc = new Vector3(0,1f,6.5f);
        }else if(dirBlockChooser<0.4f&&cameFrom!=enum_Directions.SOUTH){
            exitBlocked = enum_Directions.SOUTH;
            spawnBlock = true;
            spawnBlockLoc = new Vector3(0,1f,-6.5f);
        }else if(dirBlockChooser<0.6f&&cameFrom!=enum_Directions.SOUTH){
            exitBlocked = enum_Directions.EAST;
            spawnBlock = true;
            spawnBlockLoc = new Vector3(-6.5f,1f,0);
        }else if(dirBlockChooser<0.6f&&cameFrom!=enum_Directions.WEST){
            exitBlocked = enum_Directions.WEST;
            spawnBlock = true;
            spawnBlockLoc = new Vector3(6.5f,1f,0);
        }
        if (spawnBlock){
            GameObject block = roomDressingManager.GetBlockedDoorPrefab();
            block.transform.parent = this.transform;
            block.transform.localPosition = spawnBlockLoc;
        }

        //GOAL OBJECT
        if(roomType!=enum_RoomTypes.GOAL){
            roomDressingManager.SpawnGoalObject(goalObjectLocations[Random.Range(0,goalObjectLocations.Length)]);
        }
        
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            //Debug.Log("Player Exited");
            enum_Directions leavingDir;
            Vector3 globalRoomPos = this.transform.position;
            Vector3 playerPos = other.gameObject.transform.position;
            if(playerPos.z>globalRoomPos.z-8f && playerPos.z<globalRoomPos.z+8f ){
                if(playerPos.x>globalRoomPos.x){
                    leavingDir = enum_Directions.WEST;
                }
                else{
                    leavingDir = enum_Directions.EAST;
                }
            }
            else{
                if(playerPos.z>globalRoomPos.z){
                    leavingDir = enum_Directions.NORTH;
                }
                else{
                    leavingDir = enum_Directions.SOUTH;
                }
            }

            //Debug.Log(leavingDir);
            roomManager.RoomTransition(leavingDir, this);
        }
    }

    public void SetLoc(int[] newLoc){
        this.loc = newLoc;
    }

    public int[] GetLoc(){
        return loc;
    }

    public void SetBlockedExit(enum_Directions dir){
        exitBlocked = dir;
    }

    public enum_Directions GetExitBlocked(){
        return exitBlocked;
    }
}
