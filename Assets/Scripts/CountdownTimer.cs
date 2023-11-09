using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CountdownTimer : MonoBehaviour
{
    public float timer = 1f;
    private Text timerSeconds;
    [SerializeField] PlayerMovement playerMovement;


    void Start()
    {
     timerSeconds = GetComponent<Text>();   
    }

    
    void Update()
    {

        timer -= Time.deltaTime;
        timerSeconds.text = timer.ToString("f0");
        if(timer<=0)
        {
            timerSeconds.text = "0";
            playerMovement.Die();
        }

    }

  

}
