using UnityEngine;

public class PlusCoin : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.inst.IncrementScore();
            GameManager.inst.IncrementIceCount();

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(0,0,turnSpeed*Time.deltaTime);
    }

}
