using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public float health;
    public float maxHealth;
    public float maxTotalHealth;
    public float invincibilityDuration = 1f;
    private float invincibileCounter;

    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    private GameObject[] heartContainers;
    private Image[] heartFills;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = PlayerTracker.instance.currentHealth;
        maxHealth = PlayerTracker.instance.maxHealth;
        maxTotalHealth = PlayerTracker.instance.maxTotalHealth;

        heartContainers = new GameObject[(int)maxTotalHealth];
        heartFills = new Image[(int)maxTotalHealth];

        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    private void Update()
    {
        if (invincibileCounter > 0)
        {
            invincibileCounter -= Time.deltaTime;
            if (invincibileCounter <= 0)
            {
                PlayerManager.instance.playerSprite.color = new Color(PlayerManager.instance.playerSprite.color.r, PlayerManager.instance.playerSprite.color.g, PlayerManager.instance.playerSprite.color.b, 1f);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (invincibileCounter <= 0)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0f, maxHealth);

            invincibileCounter = invincibilityDuration;

            PlayerManager.instance.playerSprite.color = new Color(PlayerManager.instance.playerSprite.color.r, PlayerManager.instance.playerSprite.color.g, PlayerManager.instance.playerSprite.color.b, 0.5f);

            if (health <= 0f)
            {
                Die();
                AudioManager.instance.PlayGameOver();
            }

            UpdateHeartsHUD();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
        UpdateHeartsHUD();
    }

    void UpdateHeartsHUD()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            heartContainers[i].SetActive(i < maxHealth);
            heartFills[i].fillAmount = (health - i >= 1f) ? 1f : (health - i > 0f ? health - i : 0f);
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < maxTotalHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab, heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }

    private void Die()
    {
        PlayerManager.instance.gameObject.SetActive(false);
        UIManager.instance.gameOverScreen.SetActive(true);
    }

    public void Invincible(float length)
    {
        invincibileCounter = length;
        PlayerManager.instance.playerSprite.color = new Color(PlayerManager.instance.playerSprite.color.r, PlayerManager.instance.playerSprite.color.g, PlayerManager.instance.playerSprite.color.b, 0.5f);
    }
}
