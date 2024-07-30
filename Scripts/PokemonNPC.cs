using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonNPC : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color charmanderColor = Color.red;
    [SerializeField] Color bulbasaurColor = Color.green;
    [SerializeField] Color squirtleColor = Color.blue;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        string pokemonName = ((Ink.Runtime.StringValue)Managers.Dialogue.GetVariableState("pokemon_name")).value;

        switch (pokemonName)
        {
            case "":
                spriteRenderer.color = defaultColor;
                break;
            case "Charmander":
                spriteRenderer.color = charmanderColor;
                break;
            case "Bulbasaur":
                spriteRenderer.color = bulbasaurColor;
                break;
            case "Squirtle":
                spriteRenderer.color = squirtleColor;
                break;
            default:
                Debug.LogWarning("Pokemon name not handled by switch statement: " + pokemonName);
                break;
        }
    }
}
