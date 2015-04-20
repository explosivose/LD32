using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class Ingredient : Photon.MonoBehaviour {
	
	public int score;
	
	
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
	}
	
	// Called when another player requests ownership of a PhotonView from you (the current owner).
	void OnOwnershipRequest(object[] viewAndPlayer) {
		//PhotonView view = viewAndPlayer[0] as PhotonView;
		
		// PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;
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
			_updateFraction += Time.deltaTime * 9;
			transform.localPosition = Vector3.Lerp(_onUpdatePos, _latestPos, _updateFraction);
			transform.localRotation = Quaternion.Lerp(_onUpdateRot, _latestRot, _updateFraction);
		}
	}
}
