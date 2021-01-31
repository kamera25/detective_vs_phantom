using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Transform transform;
    private Vector3 latesetPos = new Vector3(0,0,0);
    private float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = transform.position - latesetPos;
        latesetPos = transform.position;

        if (Input.GetKey(KeyCode.Z))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        // 矢印移動の入力
        if(Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.AddForce(new Vector3(0, 0, moveSpeed));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.AddForce(new Vector3(0, 0, -moveSpeed));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.AddForce(new Vector3(moveSpeed, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.AddForce(new Vector3(-moveSpeed, 0, 0));
        }

        if(diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff);
        }

        rigidbody.velocity *= 0.98f;
    }
}
