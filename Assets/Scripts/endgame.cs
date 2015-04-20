using UnityEngine;
using System.Collections;

public class endgame : MonoBehaviour {

	StafeShooter s;
	float timer = 5f;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			s = other.GetComponentInChildren<StafeShooter>();
		}
	}

	void Update(){
		if (s != null){
			if (s.currentForce < 10) {
				timer =- Time.deltaTime;
				if (timer < 0){
//					Application.LoadLevel(0);
				}
			}
		}
	}




}
