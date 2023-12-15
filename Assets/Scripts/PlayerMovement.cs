using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    [SerializeField] Rigidbody rb;
    public float speed = 5f;
    public float horizontalMultiplier = 1.7f;
    public float speedIncreasePerPoint = 0.1f;
    public GameManager gameManager;
    public GameObject DieUI;

    private void FixedUpdate()
    {
        if (!alive) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = Vector3.zero;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < Screen.width / 2) // Ekranýn sol yarýsý
            {
                horizontalMove = -transform.right * speed * Time.fixedDeltaTime * horizontalMultiplier;
            }
            else // Ekranýn sað yarýsý
            {
                horizontalMove = transform.right * speed * Time.fixedDeltaTime * horizontalMultiplier;
            }
        }

        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    private void Update()
    {
        if (!alive) return;

        if (transform.position.y < -5)
        {
            Die();
        }

        gameManager.LevelWon();
    }

    public void Die()
    {
        alive = false;
        Invoke("Restart", 2);
        DieUI.SetActive(true);


    }



    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


}