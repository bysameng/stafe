using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			other.GetComponent<CheckpointManager>().checkpoint = this;
		}
	}
}

