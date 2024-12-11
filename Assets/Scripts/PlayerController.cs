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
    public float initialGameTime = 60f; // Baþlangýçta kalan süre
    public float gameTime; // Geri sayým süresi
    public Image countdownImage; // Geri sayým için kullanýlan Image (dolum çubuðu)
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
    private Vector2 lastMovementInput; // Son hareket yönü
    private Animator animator;
    public GameObject playerLight;
    public Joystick joystick;
    public Button shootButton; // UI butonu
    public bool isInBase = false;
    public bool isTimeStopped = false; // Zamanýn durup durmadýðýný kontrol eden deðiþken
    private bool open = true;
    public GameObject bulletPrefab; // Mermi prefab'ý
    public Transform firePoint; // Merminin çýkýþ noktasý
    public int maxBullets = 2; // Maksimum mermi sayýsý
    private int currentBullets; // Þu anda mevcut mermi sayýsý

    private void Start()
    {
        doorController = FindAnyObjectByType<DoorController>();
        countdownText.text = Mathf.Ceil(initialGameTime).ToString();
        countdownImage.fillAmount = 1f;
        spawnPoint = transform.position;
        gameTime = initialGameTime; // Baþlangýç zamaný ayarla
        LoadPlayerData(); // Oyuncu verilerini yükle
        SetExperiance();

        // Shoot fonksiyonunu butona atayýn
        shootButton.onClick.AddListener(Shoot);
        currentBullets = maxBullets; // Mermi sayýsýný baþlangýçta maksimuma ayarla
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Zaman kontrolü: Geri sayým yap
        if (!isInBase && !isTimeStopped)
        {
            gameTime -= Time.deltaTime;
            countdownImage.fillAmount = gameTime / initialGameTime; // Dolum çubuðunu güncelle
            countdownText.text = Mathf.Ceil(gameTime).ToString();
            if (gameTime <= 0)
            {
                gameTime = 0;
                isTimeStopped = true; // Zamaný durdur
                doorController.SetDoorState(true);
                TeleportToSpawn(); // Zaman bittiðinde ýþýnla
                ResetExperiance(); // EXP'yi sýfýrla
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

        // Space tuþuna basýldýðýnda ateþ et
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
            gameTime = Mathf.Max(0, gameTime - 5f); // Zaman 0'ýn altýna inmesin
            Debug.Log("Düþmana çarpýldý! Zaman 5 saniye azaldý. Kalan Zaman: " + gameTime);
        }

        if (collision.CompareTag("Experiance"))
        {
            TakeExperiance(exp);
        }

        if (collision.CompareTag("Base"))
        {
            isInBase = true; // Base'e giriþ
            Debug.Log("Base bölgesine girildi. EXP kaydedilecek.");
            savedExperiance = currentExperiance;
            ReloadBullets();
            SavePlayerData();
            OpenCloseLightPlayer();
            doorController.SetDoorState(true);
            gameTime = initialGameTime; // Zamaný yeniden baþlat
            isTimeStopped = false; // Zamaný tekrar çalýþtýr
            Debug.Log("Base'e ulaþýldý! Süre sýfýrlandý.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Base"))
        {
            isInBase = false; // Base'den çýkýþ
            OpenCloseLightPlayer();
            Debug.Log("Base bölgesinden çýkýldý. EXP kaydedilmeyecek.");
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

        Debug.Log($"Player data loaded! Yüklenen EXP: {currentExperiance}");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Uygulama kapanýyor...");
        if (isInBase) SavePlayerData();
    }

    public void GoToMainMenu()
    {
        Debug.Log("Ana menüye geçiliyor...");
        if (isInBase) SavePlayerData();
        SceneManager.LoadScene("MainMenu");
    }

    private void TeleportToSpawn()
    {
        // Zaman bitince oyuncuyu spawn noktasýna ýþýnla
        transform.position = spawnPoint;
        Debug.Log("Zaman bitti! Oyuncu spawn noktasýna ýþýnlandý.");
    }

    private void ResetExperiance()
    {
        // Zaman bitince EXP'yi kaybet
        currentExperiance = 0;
        SetExperiance();
        Debug.Log("Zaman bitti! EXP sýfýrlandý.");
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
            Debug.Log("Ateþlenme Gerçekleþti " +currentBullets.ToString());

            currentBullets--; // Mermi sayýsýný azalt
        }
    }

    public void ReloadBullets()
    {
        currentBullets = maxBullets; // Mermi sayýsýný yenile
    }
}
