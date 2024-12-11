using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;  // Yol noktalarýný manuel olarak atayýn (Inspector'dan)
    public float moveSpeed = 2f;   // Enemy hareket hýzý
    public float reachThreshold = 0.1f;  // Yol noktasýna ulaþma eþiði

    private int currentWaypointIndex = 0;  // Þu anda gidilen yol noktasý
    private bool reverseDirection = false;  // Yol yönünü tersine çevirme durumu
    private Animator animator;  // Animator referansý

    private BoxCollider2D boxCollider;  // BoxCollider referansý
    private SpriteRenderer spriteRenderer;  // SpriteRenderer referansý
    private Color originalColor;  // Orijinal renk

    private bool isStunned = false;  // Stan durumunu takip eder

    void Start()
    {
        animator = GetComponent<Animator>();  // Animator component'ýný al
        boxCollider = GetComponent<BoxCollider2D>();  // BoxCollider component'ýný al
        spriteRenderer = GetComponent<SpriteRenderer>();  // SpriteRenderer component'ýný al
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
        // Eðer waypoint listesi boþsa, iþlem yapma
        if (waypoints.Length == 0)
            return;

        // Mevcut waypoint'e doðru hareket et
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Animator'a hareket bilgisi gönder
        animator.SetFloat("Look X", direction.x);  // Yatay yönü belirle
        animator.SetFloat("Look Y", direction.y);  // Dikey yönü belirle

        // Hedef noktaya doðru hareket et
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Hedef noktaya yakýnsan, bir sonraki noktaya geç
        if (Vector3.Distance(transform.position, targetWaypoint.position) < reachThreshold)
        {
            // Sonraki waypoint'e geç (yönü kontrol ederek)
            if (reverseDirection)
            {
                currentWaypointIndex--; // Geri git
                if (currentWaypointIndex <= 0)
                {
                    reverseDirection = false; // Baþlangýç noktasýna ulaþtýðýnda tekrar ileri git
                }
            }
            else
            {
                currentWaypointIndex++; // Ýleri git
                if (currentWaypointIndex >= waypoints.Length - 1)
                {
                    reverseDirection = true; // Son waypoint'e ulaþtýðýnda geri dön
                }
            }

            // currentWaypointIndex'i sýnýrla (dizinin dýþýna çýkmasýný engelle)
            currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, waypoints.Length - 1);
        }
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        // Stan durumu baþlat
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
