using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	new private Rigidbody rigidbody;

	void Awake(){
		rigidbody = gameObject.GetComponent<Rigidbody>();
		gameObject.layer = 24;
		GlobalSoundEffects.main.PlayClipAtPoint("pew", transform.position, .5f).rolloffMode = AudioRolloffMode.Linear;
	}

	void Update(){
		rigidbody.MovePosition(transform.position + transform.forward * Time.deltaTime * 10);
	}

	void OnTriggerEnter(Collider other){
		OnTriggerStay(other);
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player"){
			Debug.Log("ded");
			other.GetComponent<CheckpointManager>().Reset();
		}
		ParticleExploder.main.ExplodeEnemy(transform.position, 5);
		Destroy (this.gameObject);
	}
}
