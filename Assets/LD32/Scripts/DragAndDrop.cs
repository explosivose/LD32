using UnityEngine;
using System.Collections;

/// <summary>
/// Drag and drop rigidbody objects using the mouse cursor
/// </summary>
public class DragAndDrop : MonoBehaviour {

	public float reachDistanceFromCamera;
	public Transform counter;
	public float maxMoveForce;
	public Pid controller;
	
	private Rigidbody _carrying;
	
	// called every frame
	void Update () {
		
		// on left click start
		if (Input.GetMouseButtonDown(0)) {
			TryPickup();
		}
		// on left click stop
		if (Input.GetMouseButtonUp(0)) {
			Drop();
		}

	}
	
	// physics update
	void FixedUpdate() {
		Carry();
	}
	
	/// <summary>
	/// Tries picking up an object under the mouse cursor.
	/// </summary>
	void TryPickup() {
		Vector3 mousePos = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, reachDistanceFromCamera)) {
			if(hit.rigidbody) {
				Pickup(hit.rigidbody);
			}
		}
	}
	
	// pickup a rigidbody
	void Pickup(Rigidbody rb) {
		_carrying = rb;
		//_carrying.SendMessage("Pickup");
		_carrying.constraints = RigidbodyConstraints.FreezePositionZ;
		controller.Reset();
	}
	
	// drop the rigidboy
	void Drop() {
		if (_carrying) {
			//_carrying.SendMessage("Drop");
			_carrying.constraints = RigidbodyConstraints.None;
			_carrying.velocity = Vector3.zero;
			_carrying = null;
		}
	}
	
	// carry along the X/Y axis
	void Carry() {
		if (!_carrying)  return;
		
		// set a carry force to follow the mouse cursor
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Mathf.Abs(Camera.main.transform.position.z - counter.position.z);
		Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 direction = (target - _carrying.position).normalized;
		float force = -controller.output(0f, direction.magnitude);
		force = Mathf.Clamp(force, 0, maxMoveForce);
		//Debug.DrawRay(_carrying.position, direction* force);
		_carrying.AddForce(direction * force);
	}
}
