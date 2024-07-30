using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area1_Apartment_Controller : MonoBehaviour
{
    [SerializeField] static Transform introTransform;

    public static Transform GetIntroSceneTransform()
    {
        return introTransform;
    }
}
