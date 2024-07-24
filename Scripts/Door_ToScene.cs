using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door_ToScene : MonoBehaviour, IInteractable
{
    [SerializeField] LevelNames targetScene;

    private BoxCollider2D _boxCollider;

    private void OnValidate()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
        _boxCollider.edgeRadius = 0.25f;
    }

    public void Interact()
    {
        Managers.Level.LoadSceneAndPlayer(targetScene);
    }
}
