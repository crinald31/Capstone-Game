using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerTransform;
    public Transform gunTransform;
    public Rigidbody2D rb;
    private Vector2 playerInput;
    private Camera mainCamera;
    public Animator animator;
    public List<PlayerGun> playerGuns = new List<PlayerGun>();

    public SpriteRenderer playerSprite;

    public float playerSpeed;

    private float activeSpeed;
    public float rollSpeed = 8f;
    public float rollLength = 0.5f;
    public float rollCooldown = 1f;
    public float rollInvincibility = 0.5f;
    public float rollCounter;
    private float rollCooldownCounter;
    private int equippedGun;

    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        mainCamera = Camera.main;
        playerSpeed = 5f;
        activeSpeed = playerSpeed;

        UIManager.instance.equippedGun.sprite = playerGuns[equippedGun].gunUI;
        UIManager.instance.gunText.text = playerGuns[equippedGun].nameOfWeapon;
    }

    void Update()
    {
        PlayerMovement();
        UpdatePlayerAim();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (playerGuns.Count > 0)
            {
                equippedGun++;
                if (equippedGun >= playerGuns.Count)
                {
                    equippedGun = 0;
                }
                SwitchGun();
            }
            else
            {
                Debug.LogError("No Player Guns");
            }
        }
    }

    private void PlayerMovement()
    {
        if (canMove && !LevelManager.instance.paused)
        {
            playerInput.x = Input.GetAxisRaw("Horizontal");
            playerInput.y = Input.GetAxisRaw("Vertical");
            playerInput.Normalize();

            rb.velocity = playerInput * activeSpeed;

            if (playerInput != new Vector2(0f, 0f))
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rollCooldownCounter <= 0 && rollCounter <= 0)
                {
                    activeSpeed = rollSpeed;
                    rollCounter = rollLength;
                    animator.SetTrigger("roll");
                    PlayerHealth.instance.Invincible(rollInvincibility);
                }
            }
            if (rollCounter > 0)
            {
                rollCounter -= Time.deltaTime;
                if (rollCounter <= 0)
                {
                    activeSpeed = playerSpeed;
                    rollCooldownCounter = rollCooldown;
                }
            }

            if (rollCooldownCounter > 0)
            {
                rollCooldownCounter -= Time.deltaTime;
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
            animator.SetBool("isMoving", false);
        }
    }

    private void UpdatePlayerAim()
    {
        if (canMove && !LevelManager.instance.paused)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(playerTransform.position);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunTransform.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                gunTransform.localScale = new Vector3(1f, 1f, 1f);
            }

            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SwitchGun()
    {
        foreach (PlayerGun gun in playerGuns)
        {
            gun.gameObject.SetActive(false);
        }
        playerGuns[equippedGun].gameObject.SetActive(true);

        UIManager.instance.equippedGun.sprite = playerGuns[equippedGun].gunUI;
        UIManager.instance.gunText.text = playerGuns[equippedGun].nameOfWeapon;
    }
}
