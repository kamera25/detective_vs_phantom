using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 当たり判定用クラス
public class HitClass : MonoBehaviour
{
    public delegate void HitCallback(Collider other);

    public HitCallback hitCallback{set;private get;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(hitCallback!=null) {
            hitCallback(other);
        }
    }
}
