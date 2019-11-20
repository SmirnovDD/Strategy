using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShoot : MonoBehaviour
{
    public float damage;
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public void Shoot()
    {
        GameObject newArrow = Instantiate(arrowPrefab, shootPoint.transform.position, Quaternion.LookRotation(shootPoint.forward));
        newArrow.GetComponent<ArrowFlight>().damage = damage;
    }
}
