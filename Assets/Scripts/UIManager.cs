using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject gameOverScreen;
    public GameObject pause;
    public Image screenFade;
    public Image equippedGun;
    public Text gunText;

    public float screenFadeSpeed;
    private bool fadeTo;
    private bool fadeOut;
    public string newGame;
    public string mainMenu;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        fadeOut = true;
        fadeTo = false;
    }

    void Update()
    {
        if (fadeOut)
        {
            screenFade.color = new Color(screenFade.color.r, screenFade.color.g, screenFade.color.b, Mathf.MoveTowards(screenFade.color.a, 0f, screenFadeSpeed * Time.deltaTime));
            if (screenFade.color.a == 0f)
            {
                fadeOut = false;
            }
        }
        if (fadeTo)
        {
            screenFade.color = new Color(screenFade.color.r, screenFade.color.g, screenFade.color.b, Mathf.MoveTowards(screenFade.color.a, 1f, screenFadeSpeed * Time.deltaTime));
            if (screenFade.color.a == 1f)
            {
                fadeTo = false;
            }
        }
    }

    public void StartFadeTo()
    {
        fadeTo = true;
        fadeOut = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGame);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }

    public void Resume()
    {
        LevelManager.instance.Pausing();
    }
}
