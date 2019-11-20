using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    public float flightSpeed;
    private Rigidbody rigidB;
    [HideInInspector]
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
        rigidB.AddForce(transform.forward * flightSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isEnemy = !other.GetComponent<ControllerAI>().isPlayerUnit;
        if (isEnemy)
        {
            Destroy(rigidB);
            transform.SetParent(other.transform);
            UnitHealth attackedUnitHealth = other.gameObject.GetComponent<UnitHealth>();
            DealDamageToEnemy.DealDamage(attackedUnitHealth, damage);
        }
    }
}
