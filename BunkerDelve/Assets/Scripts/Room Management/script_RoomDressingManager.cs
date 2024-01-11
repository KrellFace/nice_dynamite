using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class script_RoomDressingManager : MonoBehaviour

{

    //GAME OBJECTS
    [Header("Set Dressing Prefabs")]
    public GameObject[] basicBoxDressings;
    public GameObject[] tJunctDressings;

    
    [Header("Other Room Prefabs")]
    public GameObject[] doorBlocks;
    public GameObject[] goalObjects;

    //FIXED VARIABLES
    
    public float goalObjectChance = 0.3f;
    
    public GameObject GetDressingForRoomType(enum_RoomTypes type){
        GameObject ret = null;
        if(type==enum_RoomTypes.BASICBOX){
            ret = Instantiate(basicBoxDressings[Random.Range(0,basicBoxDressings.Length)]);
        }
        else if(type==enum_RoomTypes.TJUNCTION){
            ret = Instantiate(tJunctDressings[Random.Range(0,tJunctDressings.Length)]);
        }
        return ret;
    }

    public GameObject GetBlockedDoorPrefab(){
        return Instantiate(doorBlocks[Random.Range(0,doorBlocks.Length)]);
    }

    public void SpawnGoalObject(Transform loc, Transform parentRoom){
        if(Random.Range(0f, 1f)<goalObjectChance){
            GameObject obj = Instantiate(goalObjects[Random.Range(0,goalObjects.Length)]);
            obj.transform.position = loc.position;
            obj.transform.parent = parentRoom;
        }
    }
}
