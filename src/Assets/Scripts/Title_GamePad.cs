using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_GamePad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVector1 = new Vector3(0,0,0);
        Vector3 playerVector2 = new Vector3(0,0,0);
        Vector3 playerVector3 = new Vector3(0,0,0);
        Vector3 playerVector4 = new Vector3(0,0,0);
        playerVector1.x = Input.GetAxis("GamePad0_X");

        if(Input.GetAxis("GamePad0_X") > 0){
            //Debug.Log(playerVector1.x);
        }
    }
}
