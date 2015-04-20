using UnityEngine;
using System.Collections;

public class SpikeTouchDestroy : MonoBehaviour {

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<CheckpointManager>().Reset();
		}
	}
}
