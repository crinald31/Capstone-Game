using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPoint;
    public Sprite gunUI;

    public float shotInterval;
    private float bulletCounter;
    public string nameOfWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.instance.canMove && !LevelManager.instance.paused)
        {
            if (bulletCounter > 0)
            {
                bulletCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    Instantiate(bullet, shootPoint.position, shootPoint.rotation);
                    bulletCounter = shotInterval;
                }
            }
        }
    }
}
