using UnityEngine;
using System.Collections;

/// <summary>
/// Drag and drop rigidbody objects using the mouse cursor
/// </summary>
public class DragAndDrop : MonoBehaviour {

	public float reachDistanceFromCamera;
	public Transform counter;
	public Pid controller;
	
	private Rigidbody _carrying;
	
	void Update () {
		
		// on left click start
		if (Input.GetMouseButtonDown(0)) {
			TryPickup();
		}
		if (Input.GetMouseButtonUp(0)) {
			Drop();
		}

		
	}
	
	void FixedUpdate() {
		Carry();
	}
	
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
	
	void Pickup(Rigidbody rb) {
		_carrying = rb;
		controller.Reset();
		//_carrying.isKinematic = true;
		_carrying.useGravity = false;
		
	}
	
	void Drop() {
		if (_carrying) {
			//_carrying.isKinematic = false;
			_carrying.useGravity = true;
			_carrying = null;
		}
	}
	
	// carry along the X/Y axis
	void Carry() {
		if (!_carrying)  return;
		
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Mathf.Abs(Camera.main.transform.position.z - counter.position.z);
		Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
		//_carrying.position = Vector3.Lerp(_carrying.position, target, Time.deltaTime * 4f);
		Vector3 direction = (target - _carrying.position).normalized;
		float force = -controller.output(0f, direction.magnitude);
		Debug.DrawRay(_carrying.position, direction* force);
		_carrying.AddForce(direction * force);
	}
}
