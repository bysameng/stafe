using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour {

	private float power = 5f;

	private Vector3 force{get{return transform.forward * power * Time.deltaTime;}}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){

		if (other.tag == "Player"){
			GlobalSoundEffects.main.PlayClipAtPoint("speedpad", transform.position, 1f);
		}
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Player"){
			other.GetComponent<vp_FPController>().AddForce(force);
		}
		Rigidbody r = other.GetComponent<Rigidbody>();
		if (r != null){
			r.AddForce(force);
		}
	}
}
