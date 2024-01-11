using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class script_PhotoPopup : MonoBehaviour
{
    public Image img;
    public TMP_Text tmpObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bind(Sprite s, string photoText){
        img.sprite = s;
        tmpObj.text = photoText;
    }
}
