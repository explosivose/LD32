using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour {

	public float triggerRadius;
	public float launchSpeed;

	public void Launch() {
		Collider[] objs = Physics.OverlapSphere(transform.position, 1f);
		Vector3 launchDir = Vector3.forward + Vector3.up; // will calc a proper trajectory later
		foreach (Collider c in objs) {
			if (c.attachedRigidbody)
			c.attachedRigidbody.AddForce(launchDir * launchSpeed, ForceMode.Impulse);
		}
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Launch();
		}
	}
}
