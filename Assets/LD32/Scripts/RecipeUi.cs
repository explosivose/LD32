using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeUi : MonoBehaviour {

	public Player.Id playerId;
	
	private Player player;
	
	private Text text;
		
	void Start() {
		text = GetComponent<Text>();
		// display recipe for local player
		if (name.Contains("2")) playerId = Player.Id.two;
		if (name.Contains("1")) playerId = Player.Id.one;
		player = Game.Instance.PlayerById(playerId);
	} 
	
	void Update() {
		if (!Game.hasStarted) return;
		text.text = player.player.name + " " + player.player.GetScore() + "\n";
		text.text += player.currentRecipe.name +  " Recipe: \n";
		foreach(GameObject ingredient in player.currentRecipe.ingredients) {
			text.text += ingredient.name + ", ";
		} 
	}
}
