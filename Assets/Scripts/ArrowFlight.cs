using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    public bool enemyArrow;
    public float flightSpeed;
    private Rigidbody rigidB;
    [HideInInspector]
    public float damage;

    private AudioSource audioS;
    // Start is called before the first frame update
    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
        rigidB.AddForce(transform.forward * flightSpeed);
        audioS = GetComponent<AudioSource>();
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
            audioS.Play();
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
            audioS.Play();
            Destroy(rigidB);
            Destroy(this);
        }
    }
}
