using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Load a scene
    /// </summary>
    /// <param name="sceneNumber"></param>
    public void LoadLevel(int sceneNumber){
        SceneManager.LoadScene(sceneNumber);
    }
}
