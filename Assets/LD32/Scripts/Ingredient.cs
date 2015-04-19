using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {
	
	public int score;
	
	public Transform instance {
		get { return _instance; }
		set {
			_instance = value;
			_instance.name = name; // removes "(Clone)" from instantiations
		}
	}
	
	private Transform _instance;
	
	
	void Awake() {
		tag = "Ingredient";
	}
}
