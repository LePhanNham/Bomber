using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.zero;
    public float speed = 3f;
    public float changeDirectionTime = 2f;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    private AnimatedSpriteRenderer activeSpriteRenderer;
    public AnimatedSpriteRenderer spriteRendererDead;
    public float timeDie = 1.25f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(PickRandomDirection), 0f, changeDirectionTime);
    }

    private void PickRandomDirection()
    {
        int dirIndex = Random.Range(0, 4);
        switch (dirIndex)
        {
            case 0:
                SetDirection(Vector2.up, spriteRendererUp);
                break;
            case 1:
                SetDirection(Vector2.down, spriteRendererDown);
                break;
            case 2:
                SetDirection(Vector2.left, spriteRendererLeft);
                break;
            case 3:
                SetDirection(Vector2.right, spriteRendererRight);
                break;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDir, AnimatedSpriteRenderer currentSpriteRenderer)
    {
        direction = newDir;
        spriteRendererUp.enabled = currentSpriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = currentSpriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = currentSpriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = currentSpriteRenderer == spriteRendererRight;
        spriteRendererDead.enabled = currentSpriteRenderer == spriteRendererDead;
        activeSpriteRenderer = currentSpriteRenderer;
        activeSpriteRenderer.idle = newDir == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeadSequence();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }

    private void DeadSequence()
    {
        CancelInvoke();
        this.enabled = false;
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDead.enabled = true;
        Invoke(nameof(OnDeadSequenceEnded), timeDie);
    }

    private void OnDeadSequenceEnded()
    {
        gameObject.SetActive(false);
    }
}
