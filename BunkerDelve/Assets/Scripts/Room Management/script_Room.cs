using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_Room : MonoBehaviour
{
    private int xLoc;
    private int yLoc;

    public void SetLoc(int[] loc){
        xLoc = loc[0];
        yLoc = loc[1];
    }

    public int[] GetLoc(){
        return new int[2](xLoc, yLoc);
    }
}
