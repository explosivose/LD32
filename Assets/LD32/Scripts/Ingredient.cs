using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour {
	
	public float jointBreakForce;
	
	public Rigidbody rb {
		get; private set;
	}
	
	public void Pickup() {
		carried = true;
	}
	
	public void Drop() {
		carried = false;
		lastCarriedTime = Time.time;
	}
	
	private float lastCarriedTime;
	private bool carried;
	
	void Awake() {
		rb = GetComponent<Rigidbody>();
		lastCarriedTime = Mathf.Infinity;
	}
}
