using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game logic. 
/// </summary>
public class Game : MonoBehaviour {

	public static Game Instance;
	public static bool hasStarted;
	
	public float startTimer;
	public GameObject foodSpawner1;
	public GameObject foodSpawner2;
	
	public Player playerOne {get; set;}
	public Player playerTwo {get; set;}
	
	public Recipe[] recipes;

	
	public void GameStart() {
		SpawnRecipe(playerOne);
		SpawnRecipe(playerTwo);
		hasStarted = true;
	}

	void SpawnRecipe(Player player) {
		player.currentRecipe = recipes[Random.Range(0, recipes.Length-1)];
		player.spawner.SpawnRecipe(player.currentRecipe);
	}

	/// <summary>
	/// Score the specified playerId according to ingredientNames.
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="ingredientNames">Ingredient names.</param>
	public void Score(Player.Id playerId, List<string> ingredientNames) {
		Player player = PlayerById(playerId);
		// should make this event last a long time, counting each ingredient infront of the player somewhere
		foreach(string ingredientName in ingredientNames) {
			// search for ingredient in recipe by name
			GameObject ingredient = player.currentRecipe.ingredients.Find(
			delegate(GameObject search) {
				return search.name == ingredientName;
			}
			);
			// if found increment player score
			if (ingredient) {
				player.score += ingredient.GetComponent<Ingredient>().score;
			}
		}
		
		// recipes are only scored the one time! go to next recipe
		SpawnRecipe(player);
	}
	
	// returns Player obj for a given Player.Id (player one or player two)
	public Player PlayerById(Player.Id playerId) {
		if (playerId == Player.Id.one) return playerOne;
		if (playerId == Player.Id.two) return playerTwo;
		Debug.Log("Invalid Player.Id");
		return null;
	}
	
	void Awake() {
		if (Instance == null) Instance = this;
		else Destroy(this);
		playerOne = new Player();	// server
		playerOne.id = Player.Id.one;
		playerOne.name = "Contestant 1";
		playerOne.spawner = foodSpawner1.GetComponent<RecipeSpawner>();
		playerTwo = new Player();	// client
		playerTwo.id = Player.Id.two;
		playerTwo.name = "Contestant 2";
		playerTwo.spawner = foodSpawner2.GetComponent<RecipeSpawner>();
	}
	
	IEnumerator Start() {
		Debug.Log("Game start in " + startTimer + " seconds");
		yield return new WaitForSeconds(startTimer);
		GameStart();
	}
	
	void OnGUI() {
		if (!hasStarted) return;
		//GUILayout.Label("Player1 is making: " + playerOne.currentRecipe.name);
		GUILayout.Label("Player1 score: " + playerOne.score);
		
	}
	
}

