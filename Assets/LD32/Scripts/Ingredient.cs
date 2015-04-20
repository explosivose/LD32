using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour {
	
	public int score;
	
		
	void Awake() {
		tag = "Ingredient";
	}
	
	// Called when another player requests ownership of a PhotonView from you (the current owner).
	void OnOwnershipRequest(object[] viewAndPlayer) {
		//PhotonView view = viewAndPlayer[0] as PhotonView;
		
		// PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;
	}
}
