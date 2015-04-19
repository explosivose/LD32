using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Catapult : MonoBehaviour {

	public Player.Id playerId;
	public float triggerRadius;
	public float launchSpeed;

	// launch objects 
	public void Launch() {
		List<string> ingredientNames = new List<string>();
		
		// get an array of objects within a radius
		Collider[] objs = Physics.OverlapSphere(transform.position, triggerRadius);
		// launch direction
		Vector3 launchDir = Vector3.forward + Vector3.up; // will calc a proper trajectory later
		// if the object has a rigidbody, launch it with an impulse force
		foreach (Collider c in objs) {
			if (c.attachedRigidbody) {
				c.attachedRigidbody.AddForce(launchDir * launchSpeed, ForceMode.Impulse);
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
	}
}
