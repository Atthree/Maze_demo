using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public Image market;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            market.gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
