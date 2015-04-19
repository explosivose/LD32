using UnityEngine;
using System.Collections;

// spawns recipes!
public class RecipeSpawner : MonoBehaviour {

	
	private bool _spawning;
	private Recipe _recipe;
	
	void Awake() {

	}
	
	/// <summary>
	/// Spawns a random recipe for a given player
	/// </summary>
	/// <param name="p">P.</param>
	public void SpawnRecipe(Recipe recipe) {
		_recipe = recipe;
		if(!_spawning) StartCoroutine(SpawnRecipeRoutine());

	}
	
	/// <summary>
	/// Spawn a recipe routine.
	/// </summary>
	/// <param name="recipe">Recipe.</param>
	/// <param name="position">Position.</param>
	IEnumerator SpawnRecipeRoutine() {
		_spawning = true;
		yield return new WaitForSeconds(2f);
		
		// spawn ingredients from recipe
		foreach(GameObject ingredient in _recipe.ingredients) {
			GameObject instance = Instantiate(ingredient, transform.position, Quaternion.identity) as GameObject;
			instance.name = ingredient.name;
			yield return new WaitForSeconds(0.5f);
		}
		_spawning = false;
	}
}
