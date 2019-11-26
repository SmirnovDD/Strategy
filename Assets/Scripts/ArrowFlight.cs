using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    public bool enemyArrow;
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
    private void FixedUpdate()
    {
        transform.forward = rigidB.velocity;
    }
    private void OnTriggerEnter(Collider other)
    {
        bool isEnemy = !other.GetComponent<ControllerAI>().isPlayerUnit;
        if ((isEnemy && !enemyArrow) || (!isEnemy && enemyArrow))
        {
            Destroy(rigidB);
            transform.SetParent(other.transform);
            UnitHealth attackedUnitHealth = other.gameObject.GetComponent<UnitHealth>();
            DealDamageToEnemy.DealDamage(attackedUnitHealth, damage);
            Destroy(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            Destroy(rigidB);
            Destroy(this);
        }
    }
}
