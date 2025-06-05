
+using System.Collections;
+using System.Collections.Generic;
+using UnityEngine;
+
+public class EnemyController : MonoBehaviour
+{
+    public Rigidbody2D rigidbody { get; private set; }
+    private Vector2 direction = Vector2.down;
+
+    [Header("Movement")]
+    public float speed = 3f;
+    public float decisionTime = 2f;
+    public float followRange = 5f;
+    public float deathTime = 1.25f;
+
+    [Header("Animations")]
+    public AnimatedSpriteRenderer spriteRendererUp;
+    public AnimatedSpriteRenderer spriteRendererDown;
+    public AnimatedSpriteRenderer spriteRendererLeft;
+    public AnimatedSpriteRenderer spriteRendererRight;
+    public AnimatedSpriteRenderer spriteRendererDead;
+
+    private AnimatedSpriteRenderer activeSpriteRenderer;
+    private Transform target;
+
+    private void Awake()
+    {
+        rigidbody = GetComponent<Rigidbody2D>();
+        activeSpriteRenderer = spriteRendererDown;
+        GameObject player = GameObject.FindGameObjectWithTag("Player");
+        if (player != null)
+        {
+            target = player.transform;
+        }
+    }
+
+    private void OnEnable()
+    {
+        StartCoroutine(ChooseDirection());
+    }
+
+    private void Update()
+    {
+        // Movement direction affects which animation is active
+        if (direction == Vector2.up)
+        {
+            SetDirection(direction, spriteRendererUp);
+        }
+        else if (direction == Vector2.down)
+        {
+            SetDirection(direction, spriteRendererDown);
+        }
+        else if (direction == Vector2.left)
+        {
+            SetDirection(direction, spriteRendererLeft);
+        }
+        else if (direction == Vector2.right)
+        {
+            SetDirection(direction, spriteRendererRight);
+        }
+        else
+        {
+            SetDirection(Vector2.zero, activeSpriteRenderer);
+        }
+    }
+
+    private void FixedUpdate()
+    {
+        Vector2 position = rigidbody.position;
+        Vector2 translation = direction * speed * Time.fixedDeltaTime;
+        rigidbody.MovePosition(position + translation);
+    }
+
+    private IEnumerator ChooseDirection()
+    {
+        while (enabled)
+        {
+            yield return new WaitForSeconds(decisionTime);
+            Vector2 newDir = Vector2.zero;
+
+            if (target != null && Vector2.Distance(transform.position, target.position) <= followRange)
+            {
+                Vector2 diff = (target.position - transform.position).normalized;
+                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
+                {
+                    newDir = diff.x > 0 ? Vector2.right : Vector2.left;
+                }
+                else
+                {
+                    newDir = diff.y > 0 ? Vector2.up : Vector2.down;
+                }
+            }
+            else
+            {
+                int rand = Random.Range(0, 4);
+                switch (rand)
+                {
+                    case 0:
+                        newDir = Vector2.up;
+                        break;
+                    case 1:
+                        newDir = Vector2.down;
+                        break;
+                    case 2:
+                        newDir = Vector2.left;
+                        break;
+                    case 3:
+                        newDir = Vector2.right;
+                        break;
+                }
+            }
+
+            direction = newDir;
+        }
+    }
+
+    private void SetDirection(Vector2 newDir, AnimatedSpriteRenderer currentSpriteRenderer)
+    {
+        direction = newDir;
+        spriteRendererUp.enabled = currentSpriteRenderer == spriteRendererUp;
+        spriteRendererDown.enabled = currentSpriteRenderer == spriteRendererDown;
+        spriteRendererLeft.enabled = currentSpriteRenderer == spriteRendererLeft;
+        spriteRendererRight.enabled = currentSpriteRenderer == spriteRendererRight;
+        spriteRendererDead.enabled = currentSpriteRenderer == spriteRendererDead;
+        activeSpriteRenderer = currentSpriteRenderer;
+        activeSpriteRenderer.idle = newDir == Vector2.zero;
+    }
+
+    private void OnTriggerEnter2D(Collider2D collision)
+    {
+        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
+        {
+            DeadSequence();
+        }
+    }
+
+    private void DeadSequence()
+    {
+        StopAllCoroutines();
+        this.enabled = false;
+        spriteRendererUp.enabled = false;
+        spriteRendererDown.enabled = false;
+        spriteRendererLeft.enabled = false;
+        spriteRendererRight.enabled = false;
+        spriteRendererDead.enabled = true;
+        Invoke(nameof(OnDeathFinished), deathTime);
+    }
+
+    private void OnDeathFinished()
+    {
+        gameObject.SetActive(false);
+    }
+}
