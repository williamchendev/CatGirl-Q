using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBehaviorScript : MonoBehaviour {

	private bool z_key;


    private void Awake()
    {
        Screen.SetResolution(480, 270, false);
    }

    void Start () {
		z_key = false;
	}
	
	void Update () {
		Controller();

        if (z_key){
            SceneManager.LoadScene("LevelScene");
        }
	}

    private void Controller (){
        z_key = false;
        z_key = Input.GetKeyDown(KeyCode.Z);
    }
}
