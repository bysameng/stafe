using UnityEngine;
using System.Collections;

public class clickanykey : MonoBehaviour {

	float timer = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0){
			timer -= Time.deltaTime;
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		if (Input.anyKeyDown){
			Application.LoadLevel(1);
		}
	}
}
