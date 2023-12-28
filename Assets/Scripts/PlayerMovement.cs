using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    [SerializeField] Rigidbody rb;
    public float speed = 5f;
    public float horizontalMultiplier = 1.7f;
    public float speedIncreasePerPoint = 0.1f;
    public GameManager gameManager;
    public GameObject dieUI;
    public Button reviveButton;
    private bool revived;
    public CanvasGroup buttonCanvasGroup;
    [SerializeField] float maxSpeed = 15f;  // Maksimum hız sınırını belirle

    void Start()
    {
        HideButton();
    }

    private void FixedUpdate()
    {
        if (!alive) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = Vector3.zero;

        // Bilgisayar için klavye kontrolleri
        float horizontalInput = Input.GetAxis("Horizontal"); // Klavyeden yatay girdi al
        horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;

        // Dokunmatik ekran desteği (Mobil cihazlar)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < Screen.width / 2)
            {
                horizontalMove = -transform.right * speed * Time.fixedDeltaTime * horizontalMultiplier;
            }
            else
            {
                horizontalMove = transform.right * speed * Time.fixedDeltaTime * horizontalMultiplier;
            }
        }

        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    void Update()
    {
        if (!alive) return;

        if (transform.position.y < -5)
        {
            Die();
        }

        if (gameManager)
        {
            int currentScore = gameManager.score;
            speed = Mathf.Min(5f + (speedIncreasePerPoint * currentScore), maxSpeed);
        }

        gameManager.LevelWon();
    }

    public void Die()
    {
        alive = false;
        Invoke("DieUIActive", 1);
        Invoke("ShowButton", 1);
    }

    public void Revive()
    {
        HideButton();
        revived = true;
        dieUI.SetActive(false);
        transform.position += Vector3.back * 8;
        alive = true;
    }

    private void DieUIActive()
    {
        dieUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void HideButton()
    {
        buttonCanvasGroup.alpha = 0;
        buttonCanvasGroup.interactable = false;
        buttonCanvasGroup.blocksRaycasts = false;
    }

    public void ShowButton()
    {
        buttonCanvasGroup.alpha = 1;
        buttonCanvasGroup.interactable = true;
        buttonCanvasGroup.blocksRaycasts = true;
    }
}
