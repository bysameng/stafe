using UnityEngine;
using System.Collections;

public class EnableTrigger : MonoBehaviour {

	public GameObject objectToEnable;
	public bool destroySelf = true;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			objectToEnable.SetActive(true);
			if (destroySelf)
				Destroy(this.gameObject);
		}
	}
}
