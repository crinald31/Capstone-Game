using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float bulletSpeed;
    public GameObject impactEffect;
    public int dmg = 25;

    void Start()
    {
        bulletSpeed = 7.5f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnoreBullets"))
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(4);

            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyManager>().Damage(dmg);
            }
        }
 
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
