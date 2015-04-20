using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// spawns recipes!
public class RecipeSpawner : MonoBehaviour {

	public Recipe randomShit;
	
	private bool _spawning;
	private Recipe _recipe;
	
	IEnumerator Start() {
		while(true) {
			yield return new WaitForSeconds(Random.Range(3f, 6f));
			if (Game.hasStarted && !_spawning) {
				int index = Random.Range(0, randomShit.ingredients.Count-1);
				GameObject randomIngredient = randomShit.ingredients[index];
				SpawnIngredient(randomIngredient);
			}
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
		GameObject instance = PhotonNetwork.Instantiate(
			ingredient.name,
			transform.position,
			Random.rotation, 0);
		instance.name = ingredient.name;
		
		// get the rigidbodies in the hierarchy
		List<Rigidbody> rbs = new List<Rigidbody>();
		if (instance.GetComponent<Rigidbody>()) {
			rbs.Add(instance.GetComponent<Rigidbody>());
		}
		instance.GetComponentsInChildren<Rigidbody>(rbs);
		// make them not kinematic (for the owner)
		foreach(Rigidbody rb in rbs) {
			rb.isKinematic = false;
		}

	}
}
