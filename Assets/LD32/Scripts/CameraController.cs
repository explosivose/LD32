using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float moveSpeed;

	private Vector3 startPos;
	private Quaternion startRot;
	
	void Start() {
		startPos = transform.localPosition;
		startRot = transform.localRotation;
	}

	void Update() {
		Vector3 targetPos = Game.hasStarted ? Vector3.zero : startPos;
		Quaternion targetRot = Game.hasStarted ? Quaternion.identity : startRot;
		transform.localPosition = Vector3.Slerp(transform.localPosition, targetPos, Time.deltaTime * moveSpeed);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * moveSpeed);
	}
}
