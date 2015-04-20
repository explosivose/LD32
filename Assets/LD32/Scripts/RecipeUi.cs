using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeUi : MonoBehaviour {

	public Player.Id playerId;
	private Text text;
	
	private Player player;
	
	void Start() {
		text = GetComponent<Text>();
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
