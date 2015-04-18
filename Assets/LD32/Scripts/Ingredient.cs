using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour {
	
	public float jointBreakForce;
	
	public Rigidbody rb {
		get; private set;
	}
	
	void Awake() {
		rb = GetComponent<Rigidbody>();
	}
	
	void OnCollisionEnter(Collision col) {
		Ingredient other = col.gameObject.GetComponent<Ingredient>();
		if (!other) return;
		FixedJoint joint = gameObject.AddComponent<FixedJoint>();
		joint.connectedBody = other.rb;
		joint.breakForce = jointBreakForce;
	}
}
