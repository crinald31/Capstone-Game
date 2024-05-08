using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CenterRoom : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public Room room;

    public bool isCleared;
    private int playerTriggerCount = 0;

    void Start()
    {
        if (isCleared)
        {
            room.doorClosed = true;
            ActivateEnemies();
        }
    }

    void Update()
    {
        if (enemies.Count > 0 && room.active && isCleared)
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
                room.OpenDoors();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerTriggerCount++;
            ActivateEnemies();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerTriggerCount--;
            if (playerTriggerCount <= 0)
            {
                playerTriggerCount = 0;
                DeactivateEnemies();
            }
        }
    }

    public void ActivateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }
    }

    public void DeactivateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }
    }
}
