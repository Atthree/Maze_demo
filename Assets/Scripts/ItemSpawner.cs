using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;  // Toplanabilir ögelerin prefab'larý
    public Transform[] spawnPoints;  // Ögelerin spawn olacak pozisyonlar (Transform'lar)
    public PlayerController playerController;  // PlayerController referansý

    private void Start()
    {
        // PlayerController'ý bul
        playerController = FindObjectOfType<PlayerController>();

        // Sahnedeki ismi "Item" olan tüm GameObject'lerin transformlarýný al
        FindItemSpawnPoints();

        // Baþlangýçta ögeleri oluþtur
        SpawnItems();

        // Süreyi baþlat
        StartCoroutine(ResetItems());
    }

    // Sahnedeki ismi "Item" olan tüm GameObject'lerin transformlarýný al
    private void FindItemSpawnPoints()
    {
        // Sahnedeki tüm GameObject'leri al
        GameObject[] items = GameObject.FindGameObjectsWithTag("ItemTransform");

        // spawnPoints dizisini oluþturacak boyutta bir liste oluþtur
        spawnPoints = new Transform[items.Length];

        // Her "Item" GameObject'inin transformunu spawnPoints dizisine ekle
        for (int i = 0; i < items.Length; i++)
        {
            spawnPoints[i] = items[i].transform;  // Her bir item GameObject'inin transformunu ekle
        }
    }

    private void SpawnItems()
    {
        // Her spawn point için sadece bir item'ý oluþtur
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Rastgele bir item prefab'ý seç
            int randomIndex = Random.Range(0, itemPrefabs.Length);  // Rastgele bir prefab seç
            Instantiate(itemPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);  // Seçilen item'ý spawn et
        }
    }


    // Süre sonunda ögeleri yeniden oluþtur
    private IEnumerator ResetItems()
    {
        while (true)
        {
            // Oyuncunun süresini kullan
            yield return new WaitForSeconds(playerController.gameTime);  // Oyuncunun süre deðeri kadar bekle

            // Ögeleri temizle (varsa)
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);  // Ögeleri yok et
            }

            // Yeni ögeleri oluþtur
            SpawnItems();
        }
    }
    
}
