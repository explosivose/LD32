using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientConnection : MonoBehaviour {

	public Pid pid;
	
	private List<IngredientConnection> connections;
	private Rigidbody rb;
	
	void Awake() {
		connections = new List<IngredientConnection>();
		rb = transform.GetComponentInParent<Rigidbody>();
	}
	
	void FixedUpdate() {
		foreach(IngredientConnection i in connections) {
			Vector3 direction = i.transform.position - transform.position;
			float force = pid.output(0f, direction.magnitude);
			rb.AddForce(direction.normalized * force);
		}
	}
	
	void OnTriggerEnter(Collider col) {
		IngredientConnection con = col.GetComponent<IngredientConnection>();
		if (!con) return;
		if (!connections.Contains(con)) connections.Add(con);
	}
	
	void OnTriggerExit(Collider col) {
		IngredientConnection con = col.GetComponent<IngredientConnection>();
		if (!con) return;
		connections.Remove(con);
	}
}
