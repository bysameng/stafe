using UnityEngine;
using System.Collections;

public class EnableDelay : MonoBehaviour {

	public MonoBehaviour componentToEnable;
	public float timer = 0f;

	void Start(){
		componentToEnable.enabled = false;
		Invoke("Do", timer);
	}

	void Do(){
		componentToEnable.enabled = true;
		Destroy(this);
	}

}
