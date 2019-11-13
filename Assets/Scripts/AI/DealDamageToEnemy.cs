using UnityEngine;

public class DealDamageToEnemy : MonoBehaviour
{
    public static void DealDamage(UnitHealth healthScript, float damage)
    {
        healthScript.TakeDamage(damage);
    }
}
