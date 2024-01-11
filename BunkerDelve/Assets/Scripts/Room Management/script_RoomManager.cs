using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_RoomManager : MonoBehaviour
{
    //GAME OBJECTS
    script_RoomDressingManager roomDressingManager;
    script_GameFlowManager gameFlowManager;

    Transform enemy = null;
    //DYNAMIC VARIALBES
    private int[] currPlayerLoc = new int[]{0,0};
    
    //FIXED VARIABLES
    public GameObject[] roomPrefabs;
    public script_Room startRoom;
    public GameObject goalRoom;

    //DYNAMIC VARIABLES
    private List<script_Room> spawnedRooms = new List<script_Room>();
    private bool initialRoomsSpawned = false;
    private float initialSpawnDelay = 0f;

    private int roomsVisited = 0;
    private bool persuitStarted = false;

    private bool readyToSpawnGoal;


    private void Start() {
        roomDressingManager = FindObjectOfType<script_RoomDressingManager>();
        gameFlowManager = FindObjectOfType<script_GameFlowManager>();
    }

    private void Update() {
        if(!initialRoomsSpawned){
            initialSpawnDelay+=Time.deltaTime;
            if(initialSpawnDelay>.5f){
                initialRoomsSpawned = true;
                startRoom.SetLoc(new int[]{0,0});
                SpawnAdjacentRooms(startRoom);
            }
        }
    }
    
    public void RoomTransition(enum_Directions direction, script_Room exitedRoom){
        roomsVisited+=1;
        if(roomsVisited>10 && !persuitStarted){
            gameFlowManager.ChangeState(enum_GameFlowState.PERSUED);
            
            persuitStarted=true;
        }



        script_Room movedInTo = null;
        int[] movedIntoLocMod = new int[]{0,0};
        if(direction==enum_Directions.NORTH){
            movedIntoLocMod= new int[]{0,1};
        }else if(direction==enum_Directions.SOUTH){
            movedIntoLocMod= new int[]{0,-1};
        }else if(direction==enum_Directions.EAST){
            movedIntoLocMod= new int[]{-1,0};
        }else if(direction==enum_Directions.WEST){
            movedIntoLocMod= new int[]{1,0};
        }
        int [] exitedRoomLoc = exitedRoom.GetLoc();
        foreach(script_Room r in spawnedRooms){
            int[] r1 = r.GetLoc();
            if(r1[0]==(exitedRoomLoc[0]+movedIntoLocMod[0])&&r1[1]==(exitedRoomLoc[1]+movedIntoLocMod[1])){
                movedInTo = r;
                currPlayerLoc = movedInTo.GetLoc();
            }
        }

        List<script_Room> justSpawned = SpawnAdjacentRooms(movedInTo);
        foreach(script_Room r in justSpawned){
            SpawnAdjacentRooms(r);
        }
        DeSpawnDistantRooms();
    }

    private List<script_Room> SpawnAdjacentRooms(script_Room room){
        List<script_Room> adjacentToSpawned = new List<script_Room>();

        enum_Directions exitBlocked = room.GetExitBlocked();
        bool northAlreadySpawned= false;
        bool southAlreadySpawned = false;
        bool eastAlreadySpawned = false;
        bool westAlreadySpawned = false;
        int[] roomLoc = room.GetLoc();

        foreach(script_Room r in spawnedRooms){
            int[] r1 = r.GetLoc();
            if(r1[0]==roomLoc[0]&&r1[1]==roomLoc[1]+1){
                northAlreadySpawned = true;
                adjacentToSpawned.Add(r);
            }else if(r1[0]==roomLoc[0]&&r1[1]==roomLoc[1]-1){
                southAlreadySpawned = true;
                adjacentToSpawned.Add(r);
            }else if(r1[0]==roomLoc[0]-1&&r1[1]==roomLoc[1]){
                eastAlreadySpawned = true;
                adjacentToSpawned.Add(r);
            }else if(r1[0]==roomLoc[0]+1&&r1[1]==roomLoc[1]){
                westAlreadySpawned = true;
                adjacentToSpawned.Add(r);
            }
        }
        Vector3 roomPos = room.transform.position;
        if(exitBlocked!=enum_Directions.NORTH&&!northAlreadySpawned){
            Vector3 spawnPos = new Vector3(roomPos.x, roomPos.y, roomPos.z+15f);
            script_Room spawnedRoom;
            if(!readyToSpawnGoal){
                spawnedRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)],spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            else{
                spawnedRoom = Instantiate(goalRoom,spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            spawnedRoom.DressRoom(enum_Directions.SOUTH,roomDressingManager);
            spawnedRoom.SetLoc(new int[]{roomLoc[0], roomLoc[1]+1});
            spawnedRooms.Add(spawnedRoom);
            adjacentToSpawned.Add(spawnedRoom);
        }
        if(exitBlocked!=enum_Directions.SOUTH&&!southAlreadySpawned){
            Vector3 spawnPos = new Vector3(roomPos.x, roomPos.y, roomPos.z-15f);
            script_Room spawnedRoom;
            if(!readyToSpawnGoal){
                spawnedRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)],spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            else{
                spawnedRoom = Instantiate(goalRoom,spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            spawnedRoom.DressRoom(enum_Directions.NORTH,roomDressingManager);
            spawnedRoom.SetLoc(new int[]{roomLoc[0], roomLoc[1]-1});
            spawnedRooms.Add(spawnedRoom);
            adjacentToSpawned.Add(spawnedRoom);
        }
        if(exitBlocked!=enum_Directions.EAST&&!eastAlreadySpawned){
            Vector3 spawnPos = new Vector3(roomPos.x-15f, roomPos.y, roomPos.z);
            script_Room spawnedRoom;
            if(!readyToSpawnGoal){
                spawnedRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)],spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            else{
                spawnedRoom = Instantiate(goalRoom,spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            spawnedRoom.DressRoom(enum_Directions.WEST,roomDressingManager);
            spawnedRoom.SetLoc(new int[]{roomLoc[0]-1, roomLoc[1]});
            spawnedRooms.Add(spawnedRoom);
            adjacentToSpawned.Add(spawnedRoom);
        }
        if(exitBlocked!=enum_Directions.WEST&&!westAlreadySpawned){
            Vector3 spawnPos = new Vector3(roomPos.x+15f, roomPos.y, roomPos.z);
            script_Room spawnedRoom;
            if(!readyToSpawnGoal){
                spawnedRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)],spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            else{
                spawnedRoom = Instantiate(goalRoom,spawnPos,room.transform.rotation, this.transform).GetComponent<script_Room>();
            }
            spawnedRoom.DressRoom(enum_Directions.EAST,roomDressingManager);
            spawnedRoom.SetLoc(new int[]{roomLoc[0]+1, roomLoc[1]});
            spawnedRooms.Add(spawnedRoom);
            adjacentToSpawned.Add(spawnedRoom);
        }

        return adjacentToSpawned;
    }

    private void DeSpawnDistantRooms(){
        
        float cullDist = 2.5f;
        //enemy = FindObjectOfType<PlayerDetector>().transform;
        if(enemy!=null){
            //TO DO
        }


        List<script_Room> toRemove = new List<script_Room>();
        foreach(script_Room r in spawnedRooms){
            int[] r1 = r.GetLoc();
            if(Mathf.Abs(currPlayerLoc[0]-r1[0])>2.5||Mathf.Abs(currPlayerLoc[1]-r1[1])>2.5){
                toRemove.Add(r);
            }
        }
        foreach(script_Room r in toRemove){
            spawnedRooms.Remove(r);
            Destroy(r.gameObject);
        }
    }

    public void SetReadyToSpawnGoal(){
        readyToSpawnGoal = true;
    }
}
