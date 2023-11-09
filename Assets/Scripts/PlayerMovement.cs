using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    [SerializeField] Rigidbody rb;
    public float speed = 5f;
    float horizontalInput;
    [SerializeField] float horizontalMultiplier = 1.7f;
    public float speedIncreasePerPoint = 0.1f;
    public GameManager gameManager;


    private void FixedUpdate()
    {
        if (!alive) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime*horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);

    }



    private void Update()
    {
        
        horizontalInput = Input.GetAxis("Horizontal");

        if(transform.position.y < -5)
        {
            Die();
        }

        gameManager.LevelWon();
        

    }

    public void Die()
    {
        alive = false;
        Invoke("Restart", 2);


    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }


}
