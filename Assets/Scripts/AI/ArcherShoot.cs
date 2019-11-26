using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShoot : MonoBehaviour
{
    public float damage;
    public GameObject arrowPrefab;
    public Transform shootPoint;

    [HideInInspector]
    public Transform enemyTransform;
    public void Shoot()
    {
        GameObject newArrow;
        if (enemyTransform)
        {
            if(Vector3.Distance(enemyTransform.position, shootPoint.position) > 20)
                newArrow = Instantiate(arrowPrefab, shootPoint.transform.position, Quaternion.LookRotation((enemyTransform.position + Vector3.up * 4f) - shootPoint.position));
            else
                newArrow = Instantiate(arrowPrefab, shootPoint.transform.position, Quaternion.LookRotation((enemyTransform.position + Vector3.up * 1.1f) - shootPoint.position));
        }
        else
            newArrow = Instantiate(arrowPrefab, shootPoint.transform.position, Quaternion.LookRotation(shootPoint.forward));

        newArrow.GetComponent<ArrowFlight>().damage = damage;
    }
}
