using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Catapult : Photon.MonoBehaviour {

	public Player.Id playerId;
	public float triggerRadius;
	public Transform target;
	public Transform launchArea;
	public Transform arm;
	public Vector3 armLaunchedRotation;
	
	private float timeOfFlight;
	private Quaternion _armInitialRot;
	private bool _launched;
		
	// launch objects 
	void Launch() {
		List<string> ingredientNames = new List<string>();
		
		// get an array of objects within a radius
		Collider[] objs = Physics.OverlapSphere(launchArea.position, triggerRadius);
		// launch direction
		timeOfFlight = Random.Range(0.6f, 1.75f);
		Vector3 targetPos = target.position + Random.insideUnitSphere;
		Vector3 s = targetPos - launchArea.position;
		Vector3 u = s/timeOfFlight - (Physics.gravity * timeOfFlight)/2f;
		// if the object has a rigidbody, launch it with an impulse force
		foreach (Collider c in objs) {
			if (c.attachedRigidbody) {
				c.attachedRigidbody.AddForce(u, ForceMode.VelocityChange);
				// if it was an ingredient add to list of names
				if (c.attachedRigidbody.tag == "Ingredient") {
					ingredientNames.Add(c.attachedRigidbody.name);
				}
			}
		}
		// give player a score for the launched ingredients
		Game.Instance.Score(playerId, ingredientNames);
		
	}
	
	IEnumerator OnPushButtonDown() {
		yield return new WaitForSeconds(0.2f);
		Launch();
		photonView.RPC("CatapultLaunchRPC", PhotonTargets.All);
	}
	
	[RPC]
	IEnumerator CatapultLaunchRPC() {
		_launched = true;
		yield return new WaitForSeconds(0.2f);
		_launched = false;
	}
	
	void Awake() {
		_armInitialRot = arm.localRotation;
	}
	
	void Update() {
		if (_launched) {
			arm.localRotation = Quaternion.Lerp(arm.localRotation, Quaternion.Euler(armLaunchedRotation), Time.deltaTime * 4f);
		} else {
			arm.localRotation = Quaternion.Slerp(arm.localRotation, _armInitialRot, Time.deltaTime * 4f);
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.Lerp(Color.yellow, Color.clear, 0.5f);
		if (!launchArea) return;
		Gizmos.DrawSphere(launchArea.position, triggerRadius);
		if (!target) return;
		Gizmos.DrawCube(target.position, Vector3.one * 0.5f);
		Vector3 p1 = launchArea.position;
		Vector3 p2 = launchArea.position;
		Vector3 s = target.position - launchArea.position;
		float T = 1.5f;
		Vector3 u = s/T  - (Physics.gravity * T)/2f;
		for(float t = 0f; t <= T; t+=T/10f) {
			p2 = launchArea.position + u * t + 0.5f * Physics.gravity * t * t;
			Gizmos.DrawLine(p1, p2);
			p1 = p2;
		}
	}
}
