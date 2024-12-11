using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject door; // Kap� objesi
    private string playerTag = "Player"; // Oyuncu etiketi
    private bool open = true;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        door.SetActive(open); // Ba�lang��ta kap�y� a��k yap
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag)) // Oyuncu kap�ya dokunursa
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag)) // Oyuncu kap�dan ��karsa
        {
            playerController.isTimeStopped = false;
            SetDoorState(false); 
        }
    }

    public void SetDoorState(bool state)
    {
        open = state;
        door.SetActive(open); // Kap�n�n durumunu g�ncelle
    }
}
