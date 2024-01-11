using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_GoalObject : MonoBehaviour
{

    public int goalID = 0;
    public Sprite goalSprite;
    public string goalText;

    private script_RoomDressingManager roomDressingManager;
    private script_GameFlowManager gameFlowManager;
    // Start is called before the first frame update
    void Start()
    {
        roomDressingManager = FindObjectOfType<script_RoomDressingManager>();
        gameFlowManager = FindObjectOfType<script_GameFlowManager>();
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
        gameFlowManager.CollectGoalObject(this);
        Destroy(this.gameObject);
    }

    
}
