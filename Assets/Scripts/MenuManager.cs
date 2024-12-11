using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menu; // Menü paneli
    public Image sure;      // Menüde gösterilen süreye ait görsel
    private bool open = false; // Menü açýk mý kapalý mý?

    // Menü görünümünü deðiþtir
    public void Sure()
    {
        open = !open; // Durumu ters çevir
        menu.gameObject.SetActive(!open); // Menü panelini aç/kapat
        sure.gameObject.SetActive(open); // Süre panelini aç/kapat
    }

    // Yeni oyun baþlat
    public void NewGame()
    {
        PlayerPrefs.DeleteAll(); // Tüm önceki kayýtlarý sil
        SceneManager.LoadScene("GameScene"); // Oyun sahnesine geç
    }

    // Oyuna devam et (eðer kayýt varsa)
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CurrentExperiance")) // Daha önce kayýt yapýlmýþ mý kontrol et
        {
            SceneManager.LoadScene("GameScene"); // Oyun sahnesine geç
        }
        else
        {
            Debug.Log("Devam edilecek kayýt bulunamadý!"); // Kayýt yoksa bilgi ver
        }
    }
}
