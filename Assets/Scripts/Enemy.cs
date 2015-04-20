using UnityEngine;
using System.Collections;

public class Enemy : Killable {

	public float speed = 10f;

	private GameObject player;
	new private Rigidbody rigidbody;
	private Renderer renderer;

	private Color emissiveColor;
	private ColorDamper colordamper;

	private float HP = 1500f;
	public float fullHP = 1500f;

	private float shootTimerFull = 1f;
	private float shootTimer;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		rigidbody = gameObject.GetComponent<Rigidbody>();
		renderer = gameObject.GetComponent<Renderer>();
		emissiveColor = renderer.material.GetColor("_EmissionColor");
		colordamper = new ColorDamper(emissiveColor, .1f);
		gameObject.layer = 25;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, player.transform.position) < 50f){
			rigidbody.AddForce((player.transform.position - transform.position).normalized * speed * (HP/fullHP));
			if (shootTimer > 0){
				shootTimer -= Time.deltaTime;
			}
			else {
				Debug.Log("shoot bullet");
				GameObject g = PrefabManager.Instantiate("bullet", transform.position);
				g.transform.LookAt(player.transform.position);
				shootTimer = shootTimerFull;
			}
		}


		colordamper.Target = emissiveColor * HP/fullHP;
		renderer.material.SetColor("_EmissionColor", colordamper.Value);
	}

	public override void Damage (float power, Vector3 hitpoint)
	{
		ParticleExploder.main.ExplodeEnemy(transform.position, 10);
		HP -= power;
		colordamper.Value *= 2f;
		if(HP <= 0)
			Invoke("Die", .05f);
	}

	void Die(){
		ParticleExploder.main.ExplodeEnemy(transform.position, 130);
		GlobalSoundEffects.main.PlayClipAtPoint("Explosion"+Random.Range(1, 22), transform.position, 1f);
		Destroy (this.gameObject);
	}

	void OnDestroy(){
		SmoothDamper.main.RemoveDamper(colordamper);
	}
}
