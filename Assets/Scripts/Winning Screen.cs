using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningScreen : MonoBehaviour
{
    public GameObject anyKeyText;

    public float keyInputWait = 2f;
    public string mainMenuScene;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (keyInputWait > 0)
        {
            keyInputWait -= Time.deltaTime;
            if (keyInputWait <= 0 )
            {
                anyKeyText.SetActive(true);
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
