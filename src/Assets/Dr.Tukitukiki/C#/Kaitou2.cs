using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaitou2 : MonoBehaviour
{
    Renderer renderer;
    Rigidbody rigidbody;
    float speed = 10;//歩くスピード

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        int key1 = 0;
        int key2 = 0;
        //左
        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.E)){
            key1 = -3;
            renderer.material.color = new Color(1,0,0,1);
        }else if(Input.GetKey(KeyCode.A)){
            key1 = -1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //右
        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E)){
            key1 = 3;
            renderer.material.color = new Color(1,0,0,1);
        }else if(Input.GetKey(KeyCode.D)){
            key1 = 1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //上
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.E)){
            key2 = 3;
            renderer.material.color = new Color(1,0,0,1);
        }else if(Input.GetKey(KeyCode.W)){
            key2 = 1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //下
        if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.E)){
            key2 = -3;
            renderer.material.color = new Color(1,0,0,1);
        }else if(Input.GetKey(KeyCode.S)){
            key2 = -1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //加算する
        rigidbody.velocity = new Vector3(key1 * speed, 0, key2 * speed);
        
        if(key1 == 0 && key2 == 0){
            rigidbody.velocity = new Vector3(0,0,0);
        }

        // if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Q)){
        //     rigidbody.velocity = new Vector3(-1 * runspeed, 0,0);
        //     renderer.material.color = new Color(1,0,0,1);
        // }else if(Input.GetKey(KeyCode.A)){
        //     rigidbody.velocity = new Vector3(-1 * workspeed, 0, 0);
        //     renderer.material.color = new Color(1,0,0,0);
        // }
        // if(Input.GetKeyUp(KeyCode.A)){
        //     rigidbody.velocity = new Vector3(0,0,0);
        //     renderer.material.color = new Color(1,0,0,0);
        // }
        // //右
        // if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Q)){
        //     rigidbody.velocity = new Vector3(1 * runspeed, 0,0);
        //     renderer.material.color = new Color(1,0,0,1);
        // }else if(Input.GetKey(KeyCode.D)){
        //     rigidbody.velocity = new Vector3(1 * workspeed, 0, 0);
        //     renderer.material.color = new Color(1,0,0,0);
        // }
        // if(Input.GetKeyUp(KeyCode.D)){
        //     rigidbody.velocity = new Vector3(0,0,0);
        //     renderer.material.color = new Color(1,0,0,0);
        // }

        // //上
        // if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Q)){
        //     rigidbody.velocity = new Vector3(0, 0, 1 * runspeed);
        //     renderer.material.color = new Color(1,0,0,1);
        // }else if(Input.GetKey(KeyCode.W)){
        //     rigidbody.velocity = new Vector3(0, 0, 1 * workspeed);
        //     renderer.material.color = new Color(1,0,0,0);
        // }
        // if(Input.GetKeyUp(KeyCode.W)){
        //     rigidbody.velocity = new Vector3(0,0,0);
        //     renderer.material.color = new Color(1,0,0,0);
        // }

        // //下
        // if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Q)){
        //     rigidbody.velocity = new Vector3(0, 0, -1 * runspeed);
        //     renderer.material.color = new Color(1,0,0,1);
        // }else if(Input.GetKey(KeyCode.S)){
        //     rigidbody.velocity = new Vector3(0, 0, -1 * workspeed);
        //     renderer.material.color = new Color(1,0,0,0);
        // }
        // if(Input.GetKeyUp(KeyCode.S)){
        //     rigidbody.velocity = new Vector3(0,0,0);
        //     renderer.material.color = new Color(1,0,0,0);
        // }
    }
}
