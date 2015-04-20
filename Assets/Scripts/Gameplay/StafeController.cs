using UnityEngine;
using System.Collections;

public class StafeController : MonoBehaviour {

	private Camera camera;

	Vector3 originalPos;
	Quaternion originalRot;
	Quaternion lastCamRot;

	void Awake(){
		originalPos = transform.localPosition;
		originalRot = transform.localRotation;
		camera = Camera.main;
	}


	// Update is called once per frame
	void Update () {
		transform.localPosition = Vector3.Slerp(transform.localPosition, originalPos, Time.deltaTime);
		float angle = Quaternion.Angle(transform.localRotation, originalRot) / 10f;
		transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRot, Time.deltaTime * angle * angle);
	}

	void LateUpdate(){
//		float rotationangle = Quaternion.Angle(lastCamRot, camera.transform.rotation);
		Quaternion delta = camera.transform.rotation * Quaternion.Inverse(lastCamRot);
		transform.rotation *= delta;
		lastCamRot = camera.transform.rotation;
	}


}
