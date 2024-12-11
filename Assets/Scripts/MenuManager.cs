using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menu; // Men� paneli
    public Image sure;      // Men�de g�sterilen s�reye ait g�rsel
    private bool open = false; // Men� a��k m� kapal� m�?

    // Men� g�r�n�m�n� de�i�tir
    public void Sure()
    {
        open = !open; // Durumu ters �evir
        menu.gameObject.SetActive(!open); // Men� panelini a�/kapat
        sure.gameObject.SetActive(open); // S�re panelini a�/kapat
    }

    // Yeni oyun ba�lat
    public void NewGame()
    {
        PlayerPrefs.DeleteAll(); // T�m �nceki kay�tlar� sil
        SceneManager.LoadScene("GameScene"); // Oyun sahnesine ge�
    }

    // Oyuna devam et (e�er kay�t varsa)
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CurrentExperiance")) // Daha �nce kay�t yap�lm�� m� kontrol et
        {
            SceneManager.LoadScene("GameScene"); // Oyun sahnesine ge�
        }
        else
        {
            Debug.Log("Devam edilecek kay�t bulunamad�!"); // Kay�t yoksa bilgi ver
        }
    }
}
