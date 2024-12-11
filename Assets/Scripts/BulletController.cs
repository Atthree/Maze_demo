using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = direction * speed;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyPatrol enemy = collision.GetComponent<EnemyPatrol>();
            if (enemy != null)
            {
                enemy.Stun(5f); // 5 saniyelik stan
            }
            Destroy(gameObject); // Mermiyi yok et
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
