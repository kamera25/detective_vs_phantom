using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform _playerTransform;

    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playerTransform.position + new Vector3(0, 4, -5.5f);

    }
}
