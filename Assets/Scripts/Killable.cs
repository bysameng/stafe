using UnityEngine;
using System.Collections;

public abstract class Killable : MonoBehaviour {

	public abstract void Damage(float power, Vector3 hitpoint);


}
