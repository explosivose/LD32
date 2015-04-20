using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

static class RecipeExtensions {

	public const string recipeKey = "rcp";
	
	public static void SetRecipe(this Player player, int index) {
		player.currentRecipeIndex = index;
		Hashtable hash = new Hashtable();
		hash[recipeKey] = index;
		player.player.SetCustomProperties(hash);
	}
	
	public static int GetRecipe(this Player player) {
		object recipeIndex;
		if (player.player.customProperties.TryGetValue(recipeKey, out recipeIndex)) {
			return (int) recipeIndex;
		}
		return 0;
	}
	
	
}
