using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public int score;
    public int iceCount;
    public int fireCount;
    public static GameManager inst;
    [SerializeField] Text scoreText;
    [SerializeField] Text iceCountText;
    [SerializeField] Text fireCountText;
    [SerializeField] PlayerMovement playerMovement;
    public int winningScore;
    public int winningIceCount;
    public int winningFireCount;
    [SerializeField] CountdownTimer timer;
    public GameObject completeLevelUI;



    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE: " + score + "   /" + winningScore;
        playerMovement.speed += playerMovement.speedIncreasePerPoint;

    }

    public void IncrementFireCount()
    {
        if (fireCount < winningFireCount) 
        {  fireCount++;
           fireCountText.text = "Fire Crystal: " + fireCount + "   /" + winningFireCount; } 
        else if (fireCount >= winningFireCount)
        { fireCountText.text = "Fire Crystal: " + winningFireCount.ToString() + "   /" + winningFireCount; }
    }

        public void IncrementIceCount()
        {
        if (iceCount < winningIceCount)
        { iceCount++;
          iceCountText.text = "Ice Crystal: " + iceCount + "   /" + winningIceCount;}
        else if (iceCount == winningIceCount)
        { iceCountText.text = "Ice Crystal: " + winningIceCount.ToString() + "   /"+winningIceCount; }
        }



        public void DecrementScore()
        {
            score -= 2;
            scoreText.text = "SCORE: " + score + "   /" + winningScore;
            playerMovement.speed -= playerMovement.speedIncreasePerPoint;

        }


        public void AddTime()
        {
            timer = GameObject.FindAnyObjectByType<CountdownTimer>();
            timer.timer += 2f; }

        private void Awake()
        {
            inst = this;
        }

        public void LevelWon()
        {
            //Won the level if the timer still on and score reached winningScore

            if (score >= winningScore && iceCount == winningIceCount && fireCount == winningFireCount)
            {
             completeLevelUI.SetActive(true);
            Invoke("NextScene", 2);

                
            }            
        }

    private void NextScene() {  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }







}

