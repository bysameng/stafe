using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StafeShooter : MonoBehaviour {

	public Color emissiveColor;
	private Camera camera;

	public float currentForce{get; private set;}

	private float forceMultiplier = 100f;
	private float timeToTrack = 3f;

	private float cooldown = .5f;
	private float cooldownTimer = 0;

	private Vector3 lastPos;
	new private Renderer renderer;

	private float time;
	private AudioSource sourc;


	private struct TrackHistory{
		public float time;
		public float val;
		public TrackHistory(float val, float time){
			this.time = time;
			this.val = val;
		}
	}

	private LinkedList<TrackHistory> velocityHistory;
	public vp_FPController player;


	// Use this for initialization
	void Start () {
		sourc = GetComponent<AudioSource>();
		currentForce = 0;
		Cursor.visible = false;
		velocityHistory = new LinkedList<TrackHistory>();
		renderer = gameObject.GetComponent<Renderer>();
		camera = Camera.main;
		currentForce = 0f;
		lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		sourc.volume = (Mathf.Log(currentForce) + 2)/10f;
		if (cooldownTimer > 0){
			cooldownTimer -= Time.deltaTime;
			return;
		}
		if (Input.GetButtonDown("Jump")){
			//jump
			ExplosionForce(2f, currentForce * forceMultiplier);
//			player.m_Velocity = Vector3.zero;
			Vector3 dir = Vector3.up;
			player.AddForce(currentForce  / 27f * dir);
			GlobalSoundEffects.main.PlayClipAtPoint("Explosion22", transform.position, .9f);
			ClearHistory();
		}
		if (Input.GetButtonDown("Fire2")){
			//shoot
			ExplosionForce(1f, currentForce * forceMultiplier / 3f);
//			player.m_Velocity = Vector3.zero;
			Vector3 dir = -camera.transform.forward;
			player.AddForce(currentForce  / 240f * dir);
			Shoot(currentForce * forceMultiplier);
			GlobalSoundEffects.main.PlayClipAtPoint("Explosion22", transform.position);
			ClearHistory();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift)){
			ExplosionForce(2f, currentForce * forceMultiplier);
//			player.m_Velocity = Vector3.zero;
			Vector3 dir = camera.transform.forward + Vector3.up/10f;
			player.AddForce(currentForce  / 27f * dir);
			GlobalSoundEffects.main.PlayClipAtPoint("Explosion22", transform.position);
			ClearHistory();
		}
		if (Input.GetKey(KeyCode.R)){
		}

		SetEmission(currentForce);
	}

	void ClearHistory(){
		velocityHistory.Clear();
		cooldownTimer = cooldown + (currentForce / 50f);
		currentForce = 0;
	}

	void FixedUpdate(){
		if (cooldownTimer > 0){
			lastPos = transform.position;
			return;
		}
		if (Input.GetButton("Fire1")){
			float stepVelocity = Vector3.Distance(transform.position, lastPos);
			time += Time.fixedDeltaTime;
			currentForce += stepVelocity;
			velocityHistory.AddLast(new TrackHistory(stepVelocity, time));
			if (time - velocityHistory.First.Value.time > timeToTrack){
				currentForce -= velocityHistory.First.Value.val;
				velocityHistory.RemoveFirst();
			}
		}
		else{
			if (currentForce > 0) currentForce -= 2 * Time.deltaTime;
			if (currentForce < 0) currentForce = 0;
		}

		lastPos = transform.position;
	}

	void SetEmission(float power){
		power = power / 5f;
		renderer.material.SetColor("_EmissionColor", emissiveColor * power * power * power);
	}

	void Shoot(float power){
		Debug.Log(power);
		if (power == 0) return;
		RaycastHit[] c = Physics.SphereCastAll(camera.ViewportPointToRay(new Vector3(.5f, .5f)), 2f, power / 200f);
		for(int i = 0; i < c.Length; i++){
			RaycastHit hit = c[i];
			if (hit.collider != null){
				Debug.Log(hit.collider.name);
				if (hit.collider.attachedRigidbody != null){
					hit.collider.attachedRigidbody.AddForceAtPosition(camera.transform.forward * power / (hit.distance), hit.point);
				}
				Killable k = (hit.collider.GetComponent<Killable>());
				if (k != null) k.Damage(power, hit.point);
			}
		}
	}

	void ExplosionForce(float radius, float power){
		System.Threading.Thread.Sleep(30);
		GlobalSoundEffects.main.PlayClipAtPoint("Explosion"+Random.Range(1, 20), transform.position, .5f);
		Debug.Log("fire!!");
		PrefabManager.Instantiate("Explosion", transform.position);
		Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
		for(int i = 0; i < colliders.Length; i++){
			if (colliders[i].attachedRigidbody != null){
				colliders[i].attachedRigidbody.AddExplosionForce(power/2f, transform.position, radius);
			}
		}
	}

	public void Reset(){
		player.Stop();
		ClearHistory();
	}
}
