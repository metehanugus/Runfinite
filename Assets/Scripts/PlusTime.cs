using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusTime : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.inst.AddTime();

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }

}
