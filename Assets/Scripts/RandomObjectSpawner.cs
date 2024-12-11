using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Yerle�tirilecek objelerin dizisi
    public Vector2 areaMin = new Vector2(-97.72f, 29.28f); // Sol alt k��e koordinatlar�
    public Vector2 areaMax = new Vector2(104.94f, 162.37f); // Sa� �st k��e koordinatlar�
    public float spacing = 7f; // Objeler aras�ndaki mesafe
    public float spawnChance = 0.5f; // Obje olu�ma olas�l��� (0.5 = %50)

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Olu�turulan objelerin listesi

    private void Start()
    {
        SpawnObjectsInArea();
    }

    private void SpawnObjectsInArea()
    {
        // X ve Y ekseninde s�rayla objeleri olu�tur
        for (float x = areaMin.x; x <= areaMax.x; x += spacing)
        {
            for (float y = areaMin.y; y <= areaMax.y; y += spacing)
            {
                // Belirli bir olas�l�kla obje olu�tur
                if (Random.value <= spawnChance) // Random.value 0 ile 1 aras�nda bir de�er d�ner
                {
                    // Rastgele bir obje se�
                    GameObject selectedPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

                    // Obje olu�turulacak pozisyon
                    Vector3 spawnPosition = new Vector3(x, y, 0f); // Z eksenini s�f�rda tutuyoruz
                    GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

                    // Obje listesini g�ncelle
                    spawnedObjects.Add(newObject);
                }
            }
        }
    }

    private void ClearObjects()
    {
        // Mevcut objeleri yok et
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear(); // Listeyi temizle
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece Player objesi Base alan�na girdi�inde �al��s�n
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Base alan�na girdi! Objeler yeniden olu�turuluyor...");

            // Mevcut objeleri s�f�rla ve yeniden olu�tur
            ClearObjects();
            SpawnObjectsInArea();
        }
    }
}
