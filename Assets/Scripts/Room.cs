using System.Collections;
using System.Collections.Generic;
using Minifantasy;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] doors;
    public List<GameObject> enemies = new List<GameObject>();
    private Coroutine doorCloseCoroutine;

    public bool doorClosed;
    //public bool isCleared;
    [HideInInspector]
    public bool active;
    public float doorOpenTime = 1.5f;


    void Start()
    {
        //ToggleEnemies(false);
    }

    
    void Update()
    {
        /*if (enemies.Count > 0 && active && isCleared)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            if (enemies.Count == 0)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(false);
                    doorClosed = false;
                }
            }
        }*/
    }

    public void ToggleEnemies(bool activate)
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(activate);
            }
        }
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
            doorClosed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraManager.instance.ChangeTarget(transform);
            if (doorClosed && enemies.Count > 0)
            {
                foreach (GameObject door in doors) 
                {
                    door.SetActive(true);
                }
            }
            active = true;
            ToggleEnemies(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            active = false;
            ToggleEnemies(false);
        }
    }
}
