using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMap : MonoBehaviour
{
    [SerializeField] WorldMapController worldMapController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        worldMapController.LoadWorldMap();
    }
}
