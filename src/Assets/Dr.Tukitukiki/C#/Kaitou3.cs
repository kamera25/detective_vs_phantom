using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaitou3 : MonoBehaviour
{
    Renderer renderer;
    Rigidbody rigidbody;
    float speed = 10;//歩くスピード
    Material material;
    
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
        material = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {

        int key1 = 0;
        int key2 = 0;
        //左
        if(Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.P)){
            key1 = -3;
            renderer.material.color = material.color;
        }else if(Input.GetKey(KeyCode.F)){
            key1 = -1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //右
        if(Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.P)){
            key1 = 3;
            renderer.material.color = new Color(1,0,0,1);
        }else if(Input.GetKey(KeyCode.H)){
            key1 = 1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //上
        if(Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.P)){
            key2 = 3;
            renderer.material.color = new Color(1,0,0,1);
        }else if(Input.GetKey(KeyCode.T)){
            key2 = 1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //下
        if(Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.P)){
            key2 = -3;
            renderer.material.color = new Color(1,0,0,1);
        }else if(Input.GetKey(KeyCode.G)){
            key2 = -1;
            renderer.material.color = new Color(1,0,0,0);
        }
        //加算する
        rigidbody.velocity = new Vector3(key1 * speed, 0, key2 * speed);
        
        if(key1 == 0 && key2 == 0){
            rigidbody.velocity = new Vector3(0,0,0);
        }
    }
}
