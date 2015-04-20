using UnityEngine;
using System.Collections;

public class ParticleExploder : MonoBehaviour {
	public static ParticleExploder main;

	public ParticleSystem enemy;

	void Awake(){
		main = this;
	}

	public void ExplodeEnemy(Vector3 position, int amount){
		transform.position = position;
		enemy.Emit(amount);
	}

}
