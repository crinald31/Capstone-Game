using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBreak : MonoBehaviour
{
    public float speed = 3f;
    private Vector3 direction;

    public float deceleration = 5f;

    public float lifetime = 3f;

    public SpriteRenderer sprite;
    public float fadeSpeed = 2.5f;

    void Start()
    {
        direction.x = Random.Range(-speed, speed);
        direction.y = Random.Range(-speed, speed);
    }

    void Update()
    {
        transform.position += direction * Time.deltaTime;

        direction = Vector3.Lerp(direction, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;

        if (lifetime < 0)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.MoveTowards(sprite.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (sprite.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
