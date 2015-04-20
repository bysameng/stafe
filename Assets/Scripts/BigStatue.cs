using UnityEngine;
using System.Collections;

public class BigStatue : Killable {

	public GameObject[] toDisable;
	public GameObject[] toEnable;
	private Animator animator;

	void Awake(){
		animator = gameObject.GetComponent<Animator>();
		animator.enabled = false;
	}

	public override void Damage (float power, Vector3 hitpoint)
	{
		Debug.Log("come on");
		ParticleExploder.main.ExplodeEnemy(hitpoint, 50);
		if (power > 1000f){
			StartCoroutine(explosions(hitpoint));
			Invoke("Explode", 2f);
		}
	}

	void Explode(){
		for(int i = 0; i < toDisable.Length; i++){
			toDisable[i].SetActive(false);
		}
		for(int i = 0; i < toEnable.Length; i++){
			toEnable[i].SetActive(true);
		}
		animator.enabled = true;
	}

	IEnumerator explosions(Vector3 hitpoint){
		for(int i = 0; i < 50; i++){
			Vector3 pos = hitpoint + (Random.insideUnitSphere * 10f);
			GlobalSoundEffects.main.PlayClipAtPoint("Explosion"+Random.Range(1, 22), pos, 1f).rolloffMode = AudioRolloffMode.Linear;
			GlobalSoundEffects.main.PlayClipAtPoint("glassexplosion"+Random.Range(1, 7), pos, 1f).rolloffMode = AudioRolloffMode.Linear;
			ParticleExploder.main.ExplodeEnemy(pos, 30);
			yield return new WaitForSeconds(Mathf.SmoothStep(5, 1, i/50f) * Random.Range( .01f, .1f));
		}
		Application.LoadLevel(0);
	}

}
