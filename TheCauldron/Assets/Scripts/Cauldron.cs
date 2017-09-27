using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Singleton<Cauldron> {

    private Soup soup;
    public Soup Soup { get { return soup; } }

    private IngredientMixer mixer;
    public IngredientMixer Mixer { get { return mixer; } }

	// Use this for initialization
	void Start () {
        mixer = FindObjectOfType<IngredientMixer>();
        soup = FindObjectOfType<Soup>();
    }
}
