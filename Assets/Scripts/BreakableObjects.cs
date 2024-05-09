using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] boxBreak;
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

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
            if (PlayerManager.instance.rollCounter > 0)
            {
                Smash();
            }
        }

        if (other.tag == "PlayerBullet")
        {
            Smash();
        }
    }

    public void Smash()
    {
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(0);

        int piecesToDrop = Random.Range(1, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, boxBreak.Length);

            Instantiate(boxBreak[randomPiece], transform.position, transform.rotation);
        }

        if (shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);

            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}
