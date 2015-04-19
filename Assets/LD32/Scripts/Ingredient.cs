using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour {
	
	public int score;
	
		
	void Awake() {
		tag = "Ingredient";
	}
}
