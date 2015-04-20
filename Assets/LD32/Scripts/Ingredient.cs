using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Ingredient : Photon.MonoBehaviour {
	
	public int score;
	
	private Rigidbody	_rb;
	private Color 		_debugOwner;
	private Color		_debugKinematic;
	
	private Vector3 	_latestPos;
	private Vector3 	_onUpdatePos;
	private Quaternion 	_latestRot;
	private Quaternion 	_onUpdateRot;
	private float 		_updateFraction;
	
	void Awake() {
		tag = "Ingredient";
		_latestPos = transform.localPosition;
		_onUpdatePos = transform.localPosition;
		_latestRot = transform.localRotation;
		_onUpdateRot = transform.localRotation;
		_rb = GetComponent<Rigidbody>();
		_debugOwner = Color.Lerp(Color.green, Color.clear, 0.5f);
		_debugKinematic = Color.Lerp(Color.red, Color.clear, 0.5f);
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			Vector3 pos = transform.localPosition;
			Quaternion rot = transform.localRotation;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		}
		else {
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
			
			_latestPos = pos;
			_onUpdatePos = transform.localPosition;
			_latestRot = rot;
			_onUpdateRot = transform.localRotation;
			_updateFraction = 0f;
		}
	}
	
	void Update() {
		if (!photonView.isMine) {
			_rb.isKinematic = true;	// receive physics results from owner
			_updateFraction += Time.deltaTime * 9;
			transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestPos, _updateFraction);
			transform.localRotation = Quaternion.Lerp(_onUpdateRot, _latestRot, _updateFraction);
		} else {
			_rb.isKinematic = false; // calculate physics locally
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = photonView.isMine ? _debugOwner : _debugKinematic;
		Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
	}
}
