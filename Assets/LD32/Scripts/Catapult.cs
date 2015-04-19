using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Catapult : MonoBehaviour {

	public Player.Id playerId;
	public float triggerRadius;
	public Transform target;
	
	private float timeOfFlight;
	
	// launch objects 
	public void Launch() {
		List<string> ingredientNames = new List<string>();
		
		// get an array of objects within a radius
		Collider[] objs = Physics.OverlapSphere(transform.position, triggerRadius);
		// launch direction
		timeOfFlight = Random.Range(0.6f, 1.75f);
		Vector3 targetPos = target.position + Random.insideUnitSphere;
		Vector3 s = targetPos - transform.position;
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
	
	void Update() {
		// testing the catapult with a keyboard
		if (Input.GetKeyDown(KeyCode.Space)) {
			Launch();
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.Lerp(Color.red, Color.clear, 0.5f);
		Gizmos.DrawSphere(transform.position, triggerRadius);
		if (!target) return;
		Gizmos.DrawCube(target.position, Vector3.one * 0.5f);
		Vector3 p1 = transform.position;
		Vector3 p2 = transform.position;
		Vector3 s = target.position - transform.position;
		float T = 1.5f;
		Vector3 u = s/T  - (Physics.gravity * T)/2f;
		for(float t = 0f; t <= T; t+=T/10f) {
			p2 = transform.position + u * t + 0.5f * Physics.gravity * t * t;
			Gizmos.DrawLine(p1, p2);
			p1 = p2;
		}
	}
}
