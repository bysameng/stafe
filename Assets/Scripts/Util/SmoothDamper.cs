using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothDamper : MonoBehaviour {
	public static SmoothDamper main;

	private List<ValueDamper> dampers;


	void Awake(){
		main = this;
		dampers = new List<ValueDamper>();
	}


	// Update is called once per frame
	void Update () {
		for (int i = 0; i < dampers.Count; i++){
			dampers[i].Update();
		}
	}

	public void AddDamper(params ValueDamper[] damper){
		for(int i = 0; i < damper.Length; i++){
			AddDamper(damper[i]);
		}
	}
	public void RemoveDamper(params ValueDamper[] damper){
		for(int i = 0; i < damper.Length; i++){
			RemoveDamper(damper[i]);
		}
	}
	public ValueDamper AddDamper(ValueDamper damper){
		dampers.Add(damper);
		return damper;
	}
	public void RemoveDamper(ValueDamper damper){
		dampers.Remove(damper);
	}
}

public abstract class ValueDamper{
	public ValueDamper(){SmoothDamper.main.AddDamper(this);}
	public abstract void Update();
	public bool isUnscaled = false;
}

public class FloatDamper : ValueDamper{
	public float Value{ get; set; }
	public float Target{ get; set; }
	public float Speed{ get; set; }
	public float velocity;
	public FloatDamper(float speed) : this(0, speed){}
	public FloatDamper(float init, float speed) : base(){
		Value = Target = init;
		this.Speed = speed;
	}
	public override void Update(){
		if (Mathf.Abs(Value - Target) > .01f)
			Value = Mathf.SmoothDamp(Value, Target, ref velocity, Speed);
	}
}

public class Vector3Damper : ValueDamper{
	public Vector3Damper(Vector3 init, float speed) : base(){
		SetValues(init, speed);
	}
	public void SetValues(Vector3 target, float speed){
		Value = Target = target; Speed = speed;
	}
	public Vector3 Value{ get; set; }
	public Vector3 Target{ get; set; }
	public float Speed{ get; set; }
	private Vector3 velocity;
	public override void Update(){
		if (Vector3.Distance(Value, Target) > .01f)
			Value = Vector3.SmoothDamp(Value, Target, ref velocity, Speed);
	}
}

public class ColorDamper : ValueDamper{
	public ColorDamper(Color init, float speed) : base(){
		SetValues(init, speed);
	}
	public void SetValues(Color target, float speed){
		Target = target; Speed = speed;
	}
	public Color Value{ get; set; }
	public Color Target{ get; set; }
	public float Speed{ get; set; }
	private Vector4 velocity;
	public override void Update(){
		if (Vector4.Distance(Value, Target) > .01f)
			Value = Utilities.Vector4SmoothDamp(Value, Target, ref velocity, Speed);
	}
}