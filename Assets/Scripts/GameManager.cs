using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image pauseMenu;    
    private bool isPaused = false; // Pause durumunu kontrol eder
    public PlayerController playerController;

    // Oyunu duraklat/devam ettir
    public void PauseControl()
    {
        isPaused = !isPaused;
        pauseMenu.gameObject.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // Oyunu duraklat
        }
        else
        {
            Time.timeScale = 1f; // Oyunu devam ettir
        }
    }

    // Ana men�ye d�n
    public void GoToMainMenu()
    {
        // Ana men�ye ge�meden �nce son kayd� yap
        Debug.Log("Ana men�ye ge�iliyor, veriler kaydediliyor...");

        // PlayerController referans�n�n olup olmad���n� kontrol et
        if (playerController != null)
        {
            playerController.SavePlayerData(); // Kaydedilen veriler base'deyken kaydedilmeli
        }
        else
        {
            Debug.LogError("PlayerController referans� eksik. Kaydedilemedi!");
        }

        Time.timeScale = 1f; // E�er oyun duraklat�ld�ysa, devam etmesini sa�la
        SceneManager.LoadScene("MainMenu"); // Ana men�ye ge�i�
    }

    // Oyun kapan�rken verilerin kaydedilmesini sa�la
    private void OnApplicationQuit()
    {
        if (playerController != null)
        {
            playerController.SavePlayerData(); // Uygulama kapan�rken verileri kaydet
        }
        else
        {
            Debug.LogError("PlayerController referans� eksik. Kaydedilemedi!");
        }
    }
}
