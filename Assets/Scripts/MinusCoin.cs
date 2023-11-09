using UnityEngine;

public class MinusCoin : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.name != "Player") return;

        GameManager.inst.DecrementScore();

        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }

}
