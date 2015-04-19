using UnityEngine;
using System.Collections;

/// <summary>
/// Player data
/// </summary>
[System.Serializable]
public class Player {


	public enum Id {
		one, two
	}
	public Id id;
	public string name;
	public int score;
	public Recipe currentRecipe;
	public RecipeSpawner spawner;
	
}
