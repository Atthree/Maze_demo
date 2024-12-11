using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;  // Toplanabilir �gelerin prefab'lar�
    public Transform[] spawnPoints;  // �gelerin spawn olacak pozisyonlar (Transform'lar)
    public PlayerController playerController;  // PlayerController referans�

    private void Start()
    {
        // PlayerController'� bul
        playerController = FindObjectOfType<PlayerController>();

        // Sahnedeki ismi "Item" olan t�m GameObject'lerin transformlar�n� al
        FindItemSpawnPoints();

        // Ba�lang��ta �geleri olu�tur
        SpawnItems();

        // S�reyi ba�lat
        StartCoroutine(ResetItems());
    }

    // Sahnedeki ismi "Item" olan t�m GameObject'lerin transformlar�n� al
    private void FindItemSpawnPoints()
    {
        // Sahnedeki t�m GameObject'leri al
        GameObject[] items = GameObject.FindGameObjectsWithTag("ItemTransform");

        // spawnPoints dizisini olu�turacak boyutta bir liste olu�tur
        spawnPoints = new Transform[items.Length];

        // Her "Item" GameObject'inin transformunu spawnPoints dizisine ekle
        for (int i = 0; i < items.Length; i++)
        {
            spawnPoints[i] = items[i].transform;  // Her bir item GameObject'inin transformunu ekle
        }
    }

    private void SpawnItems()
    {
        // Her spawn point i�in sadece bir item'� olu�tur
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Rastgele bir item prefab'� se�
            int randomIndex = Random.Range(0, itemPrefabs.Length);  // Rastgele bir prefab se�
            Instantiate(itemPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);  // Se�ilen item'� spawn et
        }
    }


    // S�re sonunda �geleri yeniden olu�tur
    private IEnumerator ResetItems()
    {
        while (true)
        {
            // Oyuncunun s�resini kullan
            yield return new WaitForSeconds(playerController.gameTime);  // Oyuncunun s�re de�eri kadar bekle

            // �geleri temizle (varsa)
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);  // �geleri yok et
            }

            // Yeni �geleri olu�tur
            SpawnItems();
        }
    }
    
}
