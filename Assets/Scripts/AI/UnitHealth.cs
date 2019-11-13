using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public float startingHP;
    private float hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = startingHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Destroy(gameObject);
    }
}
