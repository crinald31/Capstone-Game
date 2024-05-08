using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float levelLoadWait = 4f;
    public string changeLevel;
    public bool paused;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pausing();
        }
    }

    public IEnumerator LevelEnd()
    {
        PlayerManager.instance.canMove = false;
        UIManager.instance.StartFadeTo();
        yield return new WaitForSeconds(levelLoadWait);
        PlayerTracker.instance.currentHealth = PlayerHealth.instance.health;
        PlayerTracker.instance.maxHealth = PlayerHealth.instance.maxHealth;
        SceneManager.LoadScene(changeLevel);
    }

    public void Pausing()
    {
        if (!paused)
        {
            UIManager.instance.pause.SetActive(true);
            paused = true;
            Time.timeScale = 0f;
        }
        else
        {
            UIManager.instance.pause.SetActive(false);
            paused = false;
            Time.timeScale = 1f;
        }
    }
}
