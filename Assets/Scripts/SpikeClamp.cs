using UnityEngine;
using System.Collections;

public class SpikeClamp : MonoBehaviour {

	private enum State{Rest, Clamp, Close, Reset};

	public SpikeTouchDestroy leftSpike;
	public SpikeTouchDestroy rightSpike;

	private float timer;

	private float restTime = .5f;
	private float clampSpeed = 1f;
	private float closeTime = 1.2f;
	private float resetTime = 1.5f;

	private State state = State.Rest;

	private Vector3 target;
	private Vector3 velocity;
	private float smoothTime;

	private Vector3 offset;


	void Start(){
		offset = leftSpike.transform.localPosition;
		state = State.Rest;
//		leftSpike.transform.localPosition = new Vector3(0f, 0f, 100f);
		rightSpike.transform.localPosition = new Vector3(-leftSpike.transform.localPosition.x, leftSpike.transform.localPosition.y, leftSpike.transform.localPosition.z);
		SetSmoother(offset, 1f);
		timer = 1f;
	}


	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0)
			ChangeState();

		if (state == State.Clamp){
//			smoothTime = 190f/Mathf.Pow(leftSpike.transform.localPosition.x, 5f);
			smoothTime = Mathf.SmoothStep(.01f, .20f, leftSpike.transform.localPosition.x/5f);
		}
		leftSpike.transform.localPosition = Vector3.SmoothDamp(leftSpike.transform.localPosition, target, ref velocity, smoothTime);
		rightSpike.transform.localPosition = new Vector3(leftSpike.transform.localPosition.x, leftSpike.transform.localPosition.y, -leftSpike.transform.localPosition.z);
		if (leftSpike.transform.localPosition.x <= .05f && state == State.Clamp){
//			leftSpike.killing = rightSpike.killing = false;
			ChangeState();
		}
	}


	private void ChangeState(){
		switch(state){
		case State.Rest:
			state = State.Clamp;
			timer = clampSpeed;
			SetSmoother(Vector3.zero, .045f);
			break;
		case State.Clamp:
			state = State.Close;
			timer = closeTime;
			break;
		case State.Close:
			state = State.Reset;
			timer = resetTime;
			SetSmoother(offset, resetTime/2f);
			break;
		case State.Reset:
			state = State.Rest;
			timer = restTime;
			break;
		}
	}


	private void SetSmoother(Vector3 target, float time){
		this.target = target;
		this.smoothTime = time;
	}


}
