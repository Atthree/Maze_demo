using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;  // Yol noktalar�n� manuel olarak atay�n (Inspector'dan)
    public float moveSpeed = 2f;   // Enemy hareket h�z�
    public float reachThreshold = 0.1f;  // Yol noktas�na ula�ma e�i�i

    private int currentWaypointIndex = 0;  // �u anda gidilen yol noktas�
    private bool reverseDirection = false;  // Yol y�n�n� tersine �evirme durumu
    private Animator animator;  // Animator referans�

    private BoxCollider2D boxCollider;  // BoxCollider referans�
    private SpriteRenderer spriteRenderer;  // SpriteRenderer referans�
    private Color originalColor;  // Orijinal renk

    private bool isStunned = false;  // Stan durumunu takip eder

    void Start()
    {
        animator = GetComponent<Animator>();  // Animator component'�n� al
        boxCollider = GetComponent<BoxCollider2D>();  // BoxCollider component'�n� al
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer component'�n� al
        originalColor = spriteRenderer.color;  // Orijinal rengi sakla
    }

    void Update()
    {
        if (!isStunned)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // E�er waypoint listesi bo�sa, i�lem yapma
        if (waypoints.Length == 0)
            return;

        // Mevcut waypoint'e do�ru hareket et
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Animator'a hareket bilgisi g�nder
        animator.SetFloat("Look X", direction.x);  // Yatay y�n� belirle
        animator.SetFloat("Look Y", direction.y);  // Dikey y�n� belirle

        // Hedef noktaya do�ru hareket et
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Hedef noktaya yak�nsan, bir sonraki noktaya ge�
        if (Vector3.Distance(transform.position, targetWaypoint.position) < reachThreshold)
        {
            // Sonraki waypoint'e ge� (y�n� kontrol ederek)
            if (reverseDirection)
            {
                currentWaypointIndex--; // Geri git
                if (currentWaypointIndex <= 0)
                {
                    reverseDirection = false; // Ba�lang�� noktas�na ula�t���nda tekrar ileri git
                }
            }
            else
            {
                currentWaypointIndex++; // �leri git
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    reverseDirection = true; // Son waypoint'e ula�t���nda geri d�n
                }
            }

            // currentWaypointIndex'i s�n�rla (dizinin d���na ��kmas�n� engelle)
            currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, waypoints.Length - 1);
        }
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        // Stan durumu ba�lat
        isStunned = true;
        boxCollider.enabled = false; 
        animator.SetTrigger("Hit"); // Hit animasyonunu tetikle

        yield return new WaitForSeconds(duration);

        // Stan durumunu bitir
        isStunned = false;
        boxCollider.enabled = true;
        spriteRenderer.color = originalColor;
    }
}
