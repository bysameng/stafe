using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour {

	public Checkpoint checkpoint;
	public StafeShooter stafe;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)){
			Reset();
		}
	}

	public void Reset(){
		transform.position = checkpoint.transform.position;
		GlobalSoundEffects.main.PlayClipAtPoint("respawn", transform.position, .5f);
		stafe.Reset();
	}
}
