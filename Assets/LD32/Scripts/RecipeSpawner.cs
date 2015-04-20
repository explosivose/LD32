using UnityEngine;
using System.Collections;

// spawns recipes!
public class RecipeSpawner : MonoBehaviour {

	public Recipe randomShit;
	
	private bool _spawning;
	private Recipe _recipe;
	
	IEnumerator Start() {
		yield return new WaitForSeconds(Random.Range(3f, 6f));
		if (Game.hasStarted) {
			int index = Random.Range(0, randomShit.ingredients.Count-1);
			GameObject randomIngredient = randomShit.ingredients[index];
			SpawnIngredient(randomIngredient);
		}
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
			SpawnIngredient(ingredient);
			yield return new WaitForSeconds(0.5f);
		}
		_spawning = false;
	}
	
	void SpawnIngredient(GameObject ingredient) {
		GameObject instance = Instantiate(ingredient, transform.position, Quaternion.identity) as GameObject;
		instance.name = ingredient.name;
	}
}
