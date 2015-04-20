using UnityEngine;
using System.Collections;

public class TimedText : MonoBehaviour {

	Vector3Damper posdamper;
	ColorDamper colordamper;
	new private Renderer renderer;

	public float delay = 0f;
	public bool fadeIn = true;
	public Vector3 beginOffset = Vector3.zero;
	public float timeToFade = 5f;

	// Use this for initialization
	void Start () {
		renderer = gameObject.GetComponent<Renderer>();
		colordamper = new ColorDamper(renderer.material.color, .1f);
		posdamper = new Vector3Damper(transform.position, .1f);
		posdamper.Value += beginOffset;
		if (fadeIn){
			Color c = colordamper.Target;
			c.a = 0f;
			colordamper.Value = c;
		}
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.color = colordamper.Value;
		if (timeToFade > 0) {
			timeToFade -= Time.deltaTime;
			if (timeToFade <= 0){
				Fade();
			}
		}
		transform.position = posdamper.Value;

	}


	void Fade(){
		Color c = colordamper.Target;
		c.a = 0f;
		colordamper.Target = c;
	}

	void OnDestroy(){
		SmoothDamper.main.RemoveDamper(colordamper, posdamper);
	}
}
