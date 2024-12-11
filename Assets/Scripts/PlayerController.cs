using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float exp = 2f;
    public float initialGameTime = 60f; // Ba�lang��ta kalan s�re
    public float gameTime; // Geri say�m s�resi
    public Image countdownImage; // Geri say�m i�in kullan�lan Image (dolum �ubu�u)
    public Text countdownText;
    public Image healthBar, experianceBar;
    public Text experianceText;
    public float maxExperiance = 30f;
    public float currentExperiance;
    public Vector2 spawnPoint;
    public DoorController doorController;
    private float savedExperiance = 0f; // Base'de kaydedilen EXP
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 lastMovementInput; // Son hareket y�n�
    private Animator animator;
    public GameObject playerLight;
    public Joystick joystick;
    public Button shootButton; // UI butonu
    public bool isInBase = false;
    public bool isTimeStopped = false; // Zaman�n durup durmad���n� kontrol eden de�i�ken
    private bool open = true;
    public GameObject bulletPrefab; // Mermi prefab'�
    public Transform firePoint; // Merminin ��k�� noktas�
    public int maxBullets = 2; // Maksimum mermi say�s�
    private int currentBullets; // �u anda mevcut mermi say�s�

    private void Start()
    {
        doorController = FindAnyObjectByType<DoorController>();
        countdownText.text = Mathf.Ceil(initialGameTime).ToString();
        countdownImage.fillAmount = 1f;
        spawnPoint = transform.position;
        gameTime = initialGameTime; // Ba�lang�� zaman� ayarla
        LoadPlayerData(); // Oyuncu verilerini y�kle
        SetExperiance();

        // Shoot fonksiyonunu butona atay�n
        shootButton.onClick.AddListener(Shoot);
        currentBullets = maxBullets; // Mermi say�s�n� ba�lang��ta maksimuma ayarla
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Zaman kontrol�: Geri say�m yap
        if (!isInBase && !isTimeStopped)
        {
            gameTime -= Time.deltaTime;
            countdownImage.fillAmount = gameTime / initialGameTime; // Dolum �ubu�unu g�ncelle
            countdownText.text = Mathf.Ceil(gameTime).ToString();
            if (gameTime <= 0)
            {
                gameTime = 0;
                isTimeStopped = true; // Zaman� durdur
                doorController.SetDoorState(true);
                TeleportToSpawn(); // Zaman bitti�inde ���nla
                ResetExperiance(); // EXP'yi s�f�rla
            }
        }

        movementInput = joystick.Value;

        if (movementInput.magnitude > 1f)
        {
            movementInput.Normalize();
        }
        animator.SetFloat("Look X", joystick.Value.x);
        animator.SetFloat("Look Y", joystick.Value.y);
        animator.SetFloat("Speed", joystick.Value.magnitude);

        if (movementInput != Vector2.zero)
        {
            lastMovementInput = movementInput;
        }

        // Space tu�una bas�ld���nda ate� et
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movementInput * moveSpeed;
    }

    public void SetExperiance()
    {
        experianceBar.fillAmount = currentExperiance / maxExperiance;
        experianceText.text = $"{currentExperiance}/{maxExperiance}";
    }

    private void TakeExperiance(float takeExp)
    {
        if (!isInBase)
        {
            currentExperiance += takeExp;
            currentExperiance = Mathf.Clamp(currentExperiance, 0, maxExperiance);
            SetExperiance();
        }
        else
        {
            savedExperiance = currentExperiance;
            SavePlayerData();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            gameTime = Mathf.Max(0, gameTime - 5f); // Zaman 0'�n alt�na inmesin
            Debug.Log("D��mana �arp�ld�! Zaman 5 saniye azald�. Kalan Zaman: " + gameTime);
        }

        if (collision.CompareTag("Experiance"))
        {
            TakeExperiance(exp);
        }

        if (collision.CompareTag("Base"))
        {
            isInBase = true; // Base'e giri�
            Debug.Log("Base b�lgesine girildi. EXP kaydedilecek.");
            savedExperiance = currentExperiance;
            ReloadBullets();
            SavePlayerData();
            OpenCloseLightPlayer();
            doorController.SetDoorState(true);
            gameTime = initialGameTime; // Zaman� yeniden ba�lat
            isTimeStopped = false; // Zaman� tekrar �al��t�r
            Debug.Log("Base'e ula��ld�! S�re s�f�rland�.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Base"))
        {
            isInBase = false; // Base'den ��k��
            OpenCloseLightPlayer();
            Debug.Log("Base b�lgesinden ��k�ld�. EXP kaydedilmeyecek.");
        }
    }

    public void SavePlayerData()
    {
        if (isInBase)
        {
            PlayerPrefs.SetFloat("CurrentExperiance", currentExperiance);
            PlayerPrefs.SetFloat("MaxExperiance", maxExperiance);
            PlayerPrefs.SetFloat("MoveSpeed", moveSpeed);
            PlayerPrefs.SetFloat("InitialGameTime", initialGameTime);
            PlayerPrefs.Save();

            Debug.Log($"Player data saved! Kaydedilen EXP: {currentExperiance}");
        }
    }

    public void LoadPlayerData()
    {
        savedExperiance = PlayerPrefs.GetFloat("CurrentExperiance", 0f);
        maxExperiance = PlayerPrefs.GetFloat("MaxExperiance", 30f);
        initialGameTime = PlayerPrefs.GetFloat("InitialGameTime", 60f);
        currentExperiance = PlayerPrefs.GetFloat("CurrentExperiance", 0f);
        moveSpeed = PlayerPrefs.GetFloat("MoveSpeed", 5f);

        Debug.Log($"Player data loaded! Y�klenen EXP: {currentExperiance}");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Uygulama kapan�yor...");
        if (isInBase) SavePlayerData();
    }

    public void GoToMainMenu()
    {
        Debug.Log("Ana men�ye ge�iliyor...");
        if (isInBase) SavePlayerData();
        SceneManager.LoadScene("MainMenu");
    }

    private void TeleportToSpawn()
    {
        // Zaman bitince oyuncuyu spawn noktas�na ���nla
        transform.position = spawnPoint;
        Debug.Log("Zaman bitti! Oyuncu spawn noktas�na ���nland�.");
    }

    private void ResetExperiance()
    {
        // Zaman bitince EXP'yi kaybet
        currentExperiance = 0;
        SetExperiance();
        Debug.Log("Zaman bitti! EXP s�f�rland�.");
    }

    public void OpenCloseLightPlayer()
    {
        open = !open;
        playerLight.SetActive(open);
    }

    public void Shoot()
    {
        if (lastMovementInput.magnitude > 0 && currentBullets > 0)
        {
            Vector2 direction;
            if (Mathf.Abs(lastMovementInput.x) > Mathf.Abs(lastMovementInput.y))
            {
                direction = new Vector2(lastMovementInput.x, 0).normalized;
            }
            else
            {
                direction = new Vector2(0, lastMovementInput.y).normalized;
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.SetDirection(direction);
            Debug.Log("Ate�lenme Ger�ekle�ti " +currentBullets.ToString());

            currentBullets--; // Mermi say�s�n� azalt
        }
    }

    public void ReloadBullets()
    {
        currentBullets = maxBullets; // Mermi say�s�n� yenile
    }
}
