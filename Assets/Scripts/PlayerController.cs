using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rigidbody { get; private set; }
    private Vector2 dir = Vector2.down;
    public float speed = 5f;
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;
    public float timeDie = 1.25f;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    private AnimatedSpriteRenderer activeSpriteRenderer;
    public AnimatedSpriteRenderer spriteRendererDead;

    [Header("UI")]
    public Button btnRestart;
    private void OnEnable()
    {
        btnRestart.gameObject.SetActive(false);
    }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;
        btnRestart.onClick.AddListener(LoadControl);
    }
    private void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if ( Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if ( Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero,activeSpriteRenderer);
        }
    }
    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = dir * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position+translation);
    }


    private void SetDirection(Vector2 newDir,AnimatedSpriteRenderer currentSpriteRenderer)
    {
        dir = newDir;
        spriteRendererUp.enabled = currentSpriteRenderer==spriteRendererUp;
        spriteRendererDown.enabled = currentSpriteRenderer== spriteRendererDown;
        spriteRendererLeft.enabled = currentSpriteRenderer== spriteRendererLeft;
        spriteRendererRight.enabled = currentSpriteRenderer==spriteRendererRight;
        spriteRendererDead.enabled = currentSpriteRenderer==spriteRendererDead;
        activeSpriteRenderer = currentSpriteRenderer;
        activeSpriteRenderer.idle = newDir==Vector2.zero;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer==LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }
    public void AddSpeed()
    {
        speed++;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeadSequence();
        }
    }
    private void DeadSequence()
    {
        this.enabled = false;
        this.GetComponent<BombController>().enabled = false;
        spriteRendererUp.enabled=false;
        spriteRendererDown.enabled=false;
        spriteRendererLeft.enabled=false;
        spriteRendererRight.enabled=false;
        spriteRendererDead.enabled=true;
        Invoke(nameof(OnDeadSequenceEnded), timeDie);
    }
    private void OnDeadSequenceEnded()
    {
        gameObject.SetActive(false);
        btnRestart.gameObject.SetActive(true);
    }
    private void LoadControl()
    {
        SceneManager.LoadScene(0);
    }
}
