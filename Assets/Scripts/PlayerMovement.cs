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
    private bool revivied;

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
        Invoke("DieUIActive", 1);
        
        
        


    }

    public void Revive()
    {   
        
        revivied = true;
        dieUI.SetActive(false);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z-8);
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


}