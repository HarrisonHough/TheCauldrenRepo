using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsControl : MonoBehaviour {

    [SerializeField]
    private Ingredient[] ingredients;
	// Use this for initialization
	void Start () {
        ingredients = new Ingredient[transform.childCount];
        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i] = transform.GetChild(i).GetComponent<Ingredient>();
        }
	}


    public void ResetIngredients()
    {
        // resets all ingredients to be back on the table
        // should be called after wave complete
        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].Reset();
        }
    }
}
