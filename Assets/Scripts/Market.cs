using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    public PlayerController controller;

    public Text hiz, zaman, canta;
    public Text hizTxt, zamanTxt, cantaTxt;
    public float hizFiyat = 10f;
    public float zamanFiyat = 10f;
    public float cantaFiyat = 10f;
    public Image market;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();

        if (controller != null && controller.isInBase)
        {
            // Fiyatlar� PlayerPrefs'ten y�kle
            hizFiyat = PlayerPrefs.GetFloat("HizFiyat", 10f);
            zamanFiyat = PlayerPrefs.GetFloat("ZamanFiyat", 10f);
            cantaFiyat = PlayerPrefs.GetFloat("CantaFiyat", 10f);

            hiz.text = controller.moveSpeed.ToString("F1");
            zaman.text = controller.initialGameTime.ToString("F1");
            canta.text = controller.maxExperiance.ToString("F1");
            cantaTxt.text = "Sat�n al " + cantaFiyat.ToString();
            hizTxt.text = "Sat�n al " + hizFiyat.ToString();
            zamanTxt.text = "Sat�n al " + zamanFiyat.ToString();
            UpdateUI();
        }
        else
        {
            Debug.LogError("PlayerController component not found in the scene.");
        }
    }

    public void HizArttir()
    {
        if (controller.currentExperiance >= hizFiyat && controller.moveSpeed < 10f)
        {
            controller.moveSpeed += 0.3f;
            controller.currentExperiance -= hizFiyat;
            hizFiyat += 1f;  // Fiyat� art�r

            // Fiyatlar� kaydet
            PlayerPrefs.SetFloat("HizFiyat", hizFiyat);

            controller.SetExperiance();
            UpdateUI();
        }
        else
        {
            return;
        }
    }

    public void ZamanArttir()
    {
        if (controller.currentExperiance >= zamanFiyat)
        {
            controller.initialGameTime += 2f;
            controller.currentExperiance -= zamanFiyat;
            zamanFiyat += 1f;  // Fiyat� art�r

            // Fiyatlar� kaydet
            PlayerPrefs.SetFloat("ZamanFiyat", zamanFiyat);

            controller.SetExperiance();
            UpdateUI();
        }
        else
        {
            return;
        }
    }

    public void CantaArttir()
    {
        if (controller.currentExperiance >= cantaFiyat)
        {
            controller.maxExperiance += 10f;
            controller.currentExperiance -= cantaFiyat;
            cantaFiyat += 1f;  // Fiyat� art�r

            // Fiyatlar� kaydet
            PlayerPrefs.SetFloat("CantaFiyat", cantaFiyat);

            controller.SetExperiance();
            UpdateUI();
        }
        else
        {
            return;
        }
    }

    private void UpdateUI()
    {
        hiz.text = controller.moveSpeed.ToString();
        zaman.text = controller.initialGameTime.ToString();
        canta.text = controller.maxExperiance.ToString();

        cantaTxt.text = "Sat�n al \n" + cantaFiyat.ToString();
        hizTxt.text = "Sat�n al \n" + hizFiyat.ToString();
        zamanTxt.text = "Sat�n al \n" + zamanFiyat.ToString();
    }

    public void CloseMarket()
    {
        market.gameObject.SetActive(false);
    }
}
