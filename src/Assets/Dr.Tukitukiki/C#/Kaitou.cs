using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaitou : MonoBehaviour
{
    bool colorOn = false;
    float workspeed = 10;//歩くスピード
    float runspeed = 30;//走るスピード
    Renderer renderer;
    Rigidbody rigidbody;

    float maxSpeed = 5.0f;//最大スピード
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector;
        vector.x = Input.GetAxis("Horizontal");
        vector.z = Input.GetAxis("Vertical");

        //走るスピード
        if (Input.GetKey(KeyCode.Tab))
        {
            rigidbody.velocity = new Vector3(vector.x * runspeed, 0, vector.z * runspeed);
            renderer.material.color = new Color(1,0,0,1);
        }
        //歩くスピード
        else
        {
            rigidbody.velocity = new Vector3(vector.x * workspeed, 0, vector.z * workspeed);
            renderer.material.color = new Color(1,0,0,0);
        }

        //スティック倒してなかったら止める
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            rigidbody.velocity = new Vector3(0, 0, 0);
            renderer.material.color = new Color(1,0,0,0);
        }





        //左
        /*if(Input.GetKey(KeyCode.LeftArrow)){
            transform.Translate(new Vector3(-0.1f,0,0));
        }
        if(Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.Tab)){
            transform.Translate(new Vector3(-0.3f,0,0));
            renderer.material.color = new Color(1,0,0,1);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow)){
            renderer.material.color = new Color(1,0,0,0);
        }
        //右
        if(Input.GetKey(KeyCode.RightArrow)){
            transform.Translate(new Vector3(0.1f,0,0));
        }
        if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.Tab)){
            transform.Translate(new Vector3(0.3f,0,0));
            renderer.material.color = new Color(1,0,0,1);
        }
        if(Input.GetKeyUp(KeyCode.RightArrow)){
            renderer.material.color = new Color(1,0,0,0);
        }

        //上
        if(Input.GetKey(KeyCode.UpArrow)){
            transform.Translate(new Vector3(0,0,0.1f));
        }
        if(Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.Tab)){
            transform.Translate(new Vector3(0,0,0.3f));
            renderer.material.color = new Color(1,0,0,1);
        }
        if(Input.GetKeyUp(KeyCode.UpArrow)){
            renderer.material.color = new Color(1,0,0,0);
        }

        //下
        if(Input.GetKey(KeyCode.DownArrow)){
            transform.Translate(new Vector3(0,0,-0.1f));
        }      
        if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.Tab)){
            transform.Translate(new Vector3(0,0,-0.3f));
            renderer.material.color = new Color(1,0,0,1);
        }
        if(Input.GetKeyUp(KeyCode.DownArrow)){
            renderer.material.color = new Color(1,0,0,0);
        }*/
    }
}
