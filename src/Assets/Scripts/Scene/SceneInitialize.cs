using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitialize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("TitleScene");
		SceneManager.LoadScene("Fade", LoadSceneMode.Additive);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
