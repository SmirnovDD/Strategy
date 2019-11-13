using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterUnitsInGrids : MonoBehaviour
{
    [HideInInspector]
    public GameObject collidingObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Plane")
        {
            collidingObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidingObject = null;
    }
}
