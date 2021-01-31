using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTitle : MonoBehaviour
{
    int phantom_count;//怪盗の選択数を調べる
    
    float timer = 3.0f;
    bool gamestart = true;
    public GameObject haikei;
    bool page = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    // if(Input.GetKey(KeyCode.S)) {
        //     SceneManager.LoadScene("MainScene");
        // }
        
    }

    //スタート
    void StarGame(){
        page = true;
    }
}
