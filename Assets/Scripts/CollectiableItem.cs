using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiableItem : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Oyuncu ile çarpýþma kontrolü
        {
            Destroy(gameObject); // Objeyi sahneden sil

        }
    }
}


