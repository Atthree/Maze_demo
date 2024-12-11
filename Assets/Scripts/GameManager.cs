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

    // Ana menüye dön
    public void GoToMainMenu()
    {
        // Ana menüye geçmeden önce son kaydý yap
        Debug.Log("Ana menüye geçiliyor, veriler kaydediliyor...");

        // PlayerController referansýnýn olup olmadýðýný kontrol et
        if (playerController != null)
        {
            playerController.SavePlayerData(); // Kaydedilen veriler base'deyken kaydedilmeli
        }
        else
        {
            Debug.LogError("PlayerController referansý eksik. Kaydedilemedi!");
        }

        Time.timeScale = 1f; // Eðer oyun duraklatýldýysa, devam etmesini saðla
        SceneManager.LoadScene("MainMenu"); // Ana menüye geçiþ
    }

    // Oyun kapanýrken verilerin kaydedilmesini saðla
    private void OnApplicationQuit()
    {
        if (playerController != null)
        {
            playerController.SavePlayerData(); // Uygulama kapanýrken verileri kaydet
        }
        else
        {
            Debug.LogError("PlayerController referansý eksik. Kaydedilemedi!");
        }
    }
}
