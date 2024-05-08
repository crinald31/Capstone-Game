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
        Debug.Log("Trigger entered");
        if (other.tag == "Player")
        {
            PlayerHealth.instance.Heal(heal);
            Destroy(gameObject);
        }
    }
}
