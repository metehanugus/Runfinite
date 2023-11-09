using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public int score;
    public static GameManager inst;
    [SerializeField] Text scoreText;
    [SerializeField] PlayerMovement playerMovement;
    public int winningScore;
    [SerializeField] CountdownTimer timer;



    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE: " + score + "   /" + winningScore;
        playerMovement.speed += playerMovement.speedIncreasePerPoint;

    }

    public void DecrementScore()
    {
        score--;
        scoreText.text = "SCORE: " + score + "   /" + winningScore;
        playerMovement.speed -= playerMovement.speedIncreasePerPoint;

    }


    public void AddTime()
    {
        timer = GameObject.FindAnyObjectByType<CountdownTimer>();
        timer.timer += 2f;    }

    private void Awake()
    {
        inst = this;
    }

    public void LevelWon()
    {
        //Won the level if the timer still on and score reached winningScore
        
        if (score == winningScore)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
      

    }







}
