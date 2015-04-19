using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour {

	public float triggerRadius;
	public float launchSpeed;

	// launch objects 
	public void Launch() {
		// get an array of objects within a radius
		Collider[] objs = Physics.OverlapSphere(transform.position, triggerRadius);
		// launch direction
		Vector3 launchDir = Vector3.forward + Vector3.up; // will calc a proper trajectory later
		// if the object has a rigidbody, launch it with an impulse force
		foreach (Collider c in objs) {
			if (c.attachedRigidbody)
			c.attachedRigidbody.AddForce(launchDir * launchSpeed, ForceMode.Impulse);
		}
	}
	
	void Update() {
		// testing the catapult with a keyboard
		if (Input.GetKeyDown(KeyCode.Space)) {
			Launch();
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.Lerp(Color.red, Color.clear, 0.5f);
		Gizmos.DrawSphere(transform.position, triggerRadius);
	}
}
