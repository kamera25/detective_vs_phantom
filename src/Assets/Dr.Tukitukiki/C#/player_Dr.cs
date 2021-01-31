using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Dr : MonoBehaviour
{
    Rigidbody rigidbody;
    bool item_get = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        
        //左
        if(Input.GetKey(KeyCode.A)){
            transform.Translate(new Vector3(-0.1f,0,0));
        }
        //右
        if(Input.GetKey(KeyCode.D)){
            transform.Translate(new Vector3(0.1f,0,0));
        }
        //上
        if(Input.GetKey(KeyCode.W)){
            transform.Translate(new Vector3(0,0,0.1f));
        }
        //下
        if(Input.GetKey(KeyCode.S)){
            transform.Translate(new Vector3(0,0,-0.1f));
        }

        //アイテムゲットしているか確認して発動
        if(item_get && Input.GetKeyDown(KeyCode.Tab)){
            Debug.Log("OK");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Item"){
            item_get = true;
            Destroy(other.gameObject);
        }
    }
}
