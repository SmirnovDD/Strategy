using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public void Shoot()
    {
        Instantiate(arrowPrefab, shootPoint.transform.position, Quaternion.LookRotation(transform.forward));
    }
}
