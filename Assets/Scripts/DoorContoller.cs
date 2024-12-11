using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject door; // Kapý objesi
    private string playerTag = "Player"; // Oyuncu etiketi
    private bool open = true;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        door.SetActive(open); // Baþlangýçta kapýyý açýk yap
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag)) // Oyuncu kapýya dokunursa
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag)) // Oyuncu kapýdan çýkarsa
        {
            playerController.isTimeStopped = false;
            SetDoorState(false); 
        }
    }

    public void SetDoorState(bool state)
    {
        open = state;
        door.SetActive(open); // Kapýnýn durumunu güncelle
    }
}
