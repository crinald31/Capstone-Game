using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 direction;
    public float speed;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        direction = PlayerManager.instance.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("IgnoreBullets"))
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(4);
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerHealth>().TakeDamage(0.5f);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
