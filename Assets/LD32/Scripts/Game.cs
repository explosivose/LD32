using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game logic. 
/// </summary>
public class Game : Photon.MonoBehaviour {

	public static Game Instance;
	public static bool hasStarted;
	
	public float startTimer;
	[Header("Player 1 (server)")]
	public GameObject foodSpawner1;
	public GameObject camera1;
	public GameObject catapult1;
	public GameObject character1;
	[Header("Player 2 (client)")]
	public GameObject foodSpawner2;
	public GameObject camera2;
	public GameObject catapult2;
	public GameObject character2;
	
	public Player playerOne {get; set;}
	public Player playerTwo {get; set;}
	
	public Recipe[] recipes;

	public void JoinedGame() {
		if (PhotonNetwork.isMasterClient) {
			playerOne.player = PhotonNetwork.player;
			Camera.main.gameObject.SetActive(false);
			camera1.SetActive(true);
			catapult1.SetActive(true);
			catapult1.GetComponent<Catapult>().playerId = playerOne.id;
		} else {
			// me
			playerTwo.player = PhotonNetwork.player;
			Camera.main.gameObject.SetActive(false);
			camera2.SetActive(true);
			catapult2.SetActive(true);
			catapult2.GetComponent<Catapult>().playerId = playerTwo.id;
			// the other player
			playerOne.player = PhotonNetwork.masterClient;
			character1.SetActive(true);
		}
	}
	
	// spawn a character for the opposite player
	public void PlayerJoined(PhotonPlayer player) {
		if (PhotonNetwork.isMasterClient) {
			playerTwo.player = player;
			character2.SetActive(true);
			GameStart();
		} 
	}
	
	public void GameStart() {
		Debug.Log("Starting the game...");
		playerOne.player.SetScore(0);
		playerTwo.player.SetScore(0);
		SpawnRecipe(playerOne);
		SpawnRecipe(playerTwo);
		Menu.Instance.ShowServerBrowser(false);
		photonView.RPC("GameStartRPC", PhotonTargets.All);
	}
	
	public void GameStop() {
		Debug.Log("Game over...");
		photonView.RPC("GameStopRPC", PhotonTargets.All);
		
	}
	
	[RPC]
	void GameStartRPC() {
		hasStarted = true;
	}
	
	[RPC]
	void GameStopRPC() {
		hasStarted = false;
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
				player.player.AddScore(ingredient.GetComponent<Ingredient>().score);
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
		playerOne.spawner = foodSpawner1.GetComponent<RecipeSpawner>();
		playerTwo = new Player();	// client
		playerTwo.id = Player.Id.two;
		playerTwo.spawner = foodSpawner2.GetComponent<RecipeSpawner>();
	}
	
	void Start() {
	
	}
	
	void OnGUI() {
		if (!hasStarted) return;
		//GUILayout.Label("Player1 is making: " + playerOne.currentRecipe.name);
		GUILayout.Label("Player1 score: " + playerOne.player.GetScore());
		GUILayout.Label("Player2 score: " + playerTwo.player.GetScore());
	}
	
		
}

