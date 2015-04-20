using UnityEngine;
using System.Collections;

/// <summary>
/// Drag and drop rigidbody objects using the mouse cursor
/// </summary>
public class DragAndDrop : MonoBehaviour {

	public float reachDistanceFromCamera;
	public Transform catapultPlatform;
	public float jointBreakForce;
	public float maxMoveForce;
	public Pid controller;
	
	private Rigidbody _carrying;
	private Rigidbody _attach;
	private Vector3 _attachOffset;
	private FixedJoint _joint;
	
	void Awake() {
		_attach = transform.Find("Grabber").GetComponent<Rigidbody>();
	}
	
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

		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Mathf.Abs(transform.position.z - catapultPlatform.position.z);
		Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 offset = _attachOffset * 1f/(Vector3.Distance(target, catapultPlatform.position)+1f);
		_attach.transform.position = Vector3.Lerp(
			_attach.transform.position,
			target + offset, Time.deltaTime * 4f);
		
		if (!_joint) Drop();

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
				Debug.DrawLine(transform.position ,hit.point, Color.red);
			}
		}
	}
	
	// pickup a rigidbody
	void Pickup(Rigidbody rb) {
		_carrying = rb;
		//_carrying.SendMessage("Pickup");
		PhotonView view = _carrying.GetComponent<PhotonView>();
		if (view) {
			if (!view.owner.isLocal) {
				view.TransferOwnership(PhotonNetwork.player);
			}
		}
		//_carrying.transform.position = _attach.transform.position;
		if (!_joint) _joint = _attach.gameObject.AddComponent<FixedJoint>();
		_joint.connectedBody = rb;
		_joint.breakForce = jointBreakForce;
		float zOffset = _attach.transform.position.z - _carrying.transform.position.z;
		_attachOffset = Vector3.forward * zOffset;
	}
	
	void OnJointBreak() {
		Drop();
	}
	
	// drop the rigidboy
	void Drop() {
		if (_carrying) {
			//_carrying.SendMessage("Drop");
			if (_joint) _joint.breakForce = 0f;
			_carrying.constraints = RigidbodyConstraints.None;
			_carrying.velocity = Vector3.zero;
			_carrying = null;
			_attachOffset = Vector3.zero;
		}
	}
	
	// carry along the X/Y axis
	void Carry() {
		if (!_carrying)  return;
		

		// set a carry force to follow the mouse cursor
		/*

		Debug.DrawLine(transform.position, target, Color.green);
		Vector3 direction = (target - _carrying.position).normalized;
		float force = -controller.output(0f, direction.magnitude);
		force = Mathf.Clamp(force, 0, maxMoveForce);
		_carrying.AddForce(direction * force);
		Debug.DrawRay(_carrying.position, direction * force, Color.blue);
		*/
	}
}
