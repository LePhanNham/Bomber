using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDead;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float decisionTime = 2f;
    public float followRange = 5f;
    public float deathTime = 1.25f;

    public Rigidbody2D Rigidbody { get; private set; }

    private Vector2 direction = Vector2.down;
    private AnimatedSpriteRenderer activeSpriteRenderer;
    private Transform playerTarget;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTarget = player.transform;
    }

    private void OnEnable()
    {
        StartCoroutine(ChooseDirection());
    }

    private void Update()
    {
        UpdateSpriteDirection();
    }

    private void FixedUpdate()
    {
        Vector2 movement = direction * speed * Time.fixedDeltaTime;
        Rigidbody.MovePosition(Rigidbody.position + movement);
    }

    private IEnumerator ChooseDirection()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(decisionTime);

            Vector2 newDirection = Vector2.zero;

            if (playerTarget != null && Vector2.Distance(transform.position, playerTarget.position) <= followRange)
            {
                Vector2 toPlayer = (playerTarget.position - transform.position).normalized;
                newDirection = Mathf.Abs(toPlayer.x) > Mathf.Abs(toPlayer.y)
                    ? (toPlayer.x > 0 ? Vector2.right : Vector2.left)
                    : (toPlayer.y > 0 ? Vector2.up : Vector2.down);
            }
            else
            {
                newDirection = RandomDirection();
            }

            direction = newDirection;
        }
    }

    private Vector2 RandomDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            default: return Vector2.right;
        }
    }

    private void UpdateSpriteDirection()
    {
        if (direction == Vector2.up)
            SetDirection(spriteRendererUp);
        else if (direction == Vector2.down)
            SetDirection(spriteRendererDown);
        else if (direction == Vector2.left)
            SetDirection(spriteRendererLeft);
        else if (direction == Vector2.right)
            SetDirection(spriteRendererRight);
        else
            SetDirection(activeSpriteRenderer, Vector2.zero);
    }

    private void SetDirection(AnimatedSpriteRenderer renderer, Vector2? overrideDir = null)
    {
        Vector2 dirToSet = overrideDir ?? direction;

        spriteRendererUp.enabled = renderer == spriteRendererUp;
        spriteRendererDown.enabled = renderer == spriteRendererDown;
        spriteRendererLeft.enabled = renderer == spriteRendererLeft;
        spriteRendererRight.enabled = renderer == spriteRendererRight;
        spriteRendererDead.enabled = renderer == spriteRendererDead;

        activeSpriteRenderer = renderer;
        activeSpriteRenderer.idle = dirToSet == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        StopAllCoroutines();
        enabled = false;

        // Disable all normal renderers
        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;

        // Enable dead animation
        spriteRendererDead.enabled = true;

        Invoke(nameof(OnDeathFinished), deathTime);
    }

    private void OnDeathFinished()
    {
        gameObject.SetActive(false);
    }
}
