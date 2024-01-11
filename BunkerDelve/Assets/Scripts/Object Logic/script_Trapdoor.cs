using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class script_Trapdoor : MonoBehaviour
{

    private Animator animator;

    public bool debug_Open;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if(debug_Open){
            OpenTrapdoor();
        }
    }

    public void OpenTrapdoor(){
        Destroy(this.gameObject);
    }
}
