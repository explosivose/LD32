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
	public RecipeSpawner spawner;
	public PhotonPlayer player;
	
	public Recipe currentRecipe {
		get {
			return Game.Instance.recipes[currentRecipeIndex];
		}
	}
	
	public int currentRecipeIndex {
		get {
			return _recipeIndex;
		}
		set {
			if (value < Game.Instance.recipes.Length && value >= 0) {
				_recipeIndex = value;
			} else {
				Debug.LogError("Tried to use recipe index out of range!");
			}
		}
	}
	
	private int _recipeIndex;
}
