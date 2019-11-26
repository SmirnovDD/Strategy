using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public bool horseMan;

    public GameObject unitRagdoll;
    public float startingHP;
    private float hp;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        hp = startingHP;
    }


    public void TakeDamage(float damage)
    {
        hp -= damage;        
        if (hp <= 0 && !dead)
        {
            if (unitRagdoll)
            {
                GameObject newRagdoll = Instantiate(unitRagdoll, transform.position, transform.rotation);
                Rigidbody ragdollRigidB = newRagdoll.GetComponentInChildren<Rigidbody>();
                ragdollRigidB.AddForce(-transform.forward * 1000 + Vector3.up * 1000);
            }
            dead = true;
            Destroy(gameObject);
        }
    }
}
