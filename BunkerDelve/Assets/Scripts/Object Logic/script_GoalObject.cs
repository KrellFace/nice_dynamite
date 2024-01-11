using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_GoalObject : MonoBehaviour
{

    public int goalID = 0;

    private script_RoomDressingManager roomDressingManager;
    // Start is called before the first frame update
    void Start()
    {
        roomDressingManager = FindObjectOfType<script_RoomDressingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bind(script_RoomDressingManager dm){
        roomDressingManager = dm;
    }

    public void Collect(){
        roomDressingManager.CollectGoalObject(goalID);
        Destroy(this.gameObject);
    }
}
