using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrops : MonoBehaviour
{
    public float heal = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerHealth.instance.health < PlayerHealth.instance.maxHealth)
            {
                PlayerHealth.instance.Heal(heal);
                Destroy(gameObject);
                AudioManager.instance.PlaySFX(7);
            }
        }
    }
}
