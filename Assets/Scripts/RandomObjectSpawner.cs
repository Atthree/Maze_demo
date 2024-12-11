using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Yerleþtirilecek objelerin dizisi
    public Vector2 areaMin = new Vector2(-97.72f, 29.28f); // Sol alt köþe koordinatlarý
    public Vector2 areaMax = new Vector2(104.94f, 162.37f); // Sað üst köþe koordinatlarý
    public float spacing = 7f; // Objeler arasýndaki mesafe
    public float spawnChance = 0.5f; // Obje oluþma olasýlýðý (0.5 = %50)

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Oluþturulan objelerin listesi

    private void Start()
    {
        SpawnObjectsInArea();
    }

    private void SpawnObjectsInArea()
    {
        // X ve Y ekseninde sýrayla objeleri oluþtur
        for (float x = areaMin.x; x <= areaMax.x; x += spacing)
        {
            for (float y = areaMin.y; y <= areaMax.y; y += spacing)
            {
                // Belirli bir olasýlýkla obje oluþtur
                if (Random.value <= spawnChance) // Random.value 0 ile 1 arasýnda bir deðer döner
                {
                    // Rastgele bir obje seç
                    GameObject selectedPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

                    // Obje oluþturulacak pozisyon
                    Vector3 spawnPosition = new Vector3(x, y, 0f); // Z eksenini sýfýrda tutuyoruz
                    GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

                    // Obje listesini güncelle
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
        // Sadece Player objesi Base alanýna girdiðinde çalýþsýn
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Base alanýna girdi! Objeler yeniden oluþturuluyor...");

            // Mevcut objeleri sýfýrla ve yeniden oluþtur
            ClearObjects();
            SpawnObjectsInArea();
        }
    }
}
