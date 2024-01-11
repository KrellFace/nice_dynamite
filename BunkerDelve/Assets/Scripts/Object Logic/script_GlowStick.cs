using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_GlowStick : MonoBehaviour
{
    float glowStickTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        glowStickTimer+=Time.deltaTime;
        if(glowStickTimer>8f){
            Destroy(this.gameObject);
        }
    }
}
